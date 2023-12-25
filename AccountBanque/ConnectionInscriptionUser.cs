using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;
namespace AccountBanque
{
    class ConnectionInscriptionUser
    {
        private protected string connexionString = "Server=localhost;User ID=root;Password=753159852456;Database=evolutif";
        public void InscriptionUser(string nomUser, string prenomUser, string emailUser, string passwordUser)
        {
            try
            {
                string passwordHash = BC.HashPassword(passwordUser);
                Guid g = Guid.NewGuid();
                AddUserClass newUser = new AddUserClass
                {
                    uuid = g.ToString(),
                    nom = nomUser,
                    prenom = prenomUser,
                    email = emailUser,
                    password = passwordHash
                };
                using var connection = new MySqlConnection(connexionString);
                connection.Open();

                var command = new MySqlCommand($"INSERT INTO user(uuid, nom, prenom, email, password) VALUES ('{newUser.uuid}', '{newUser.nom}', '{newUser.prenom}', '{newUser.email}', '{newUser.password}')", connection);
                command.ExecuteNonQuery();
                Console.WriteLine("User ajouter avec success!!");
                

                var createAccount = new MySqlCommand($"INSERT INTO account(uuidAccount, uuidUser) VALUES ('{g}', '{newUser.uuid}')", connection);
                createAccount.ExecuteNonQuery();
                Console.WriteLine("Account as been created!!");
                return;

            } catch (Exception e)
            {
                Console.WriteLine($"Une erreur est survenue: {e.ToString()}");
            }
            
        }
        public List<userClass> ConnexionUser(string email, string password)
        {
            List<userClass> userList = new List<userClass>();
            try
            {
                using var connection = new MySqlConnection(connexionString);
                connection.Open();
                var command = new MySqlCommand($"SELECT * FROM user WHERE email = '{email}'", connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    userClass user = new userClass
                    {
                        id = reader.GetInt32("id"),
                        uuid = reader.GetString("uuid"),
                        nom = reader.GetString("nom"),
                        prenom = reader.GetString("prenom"),
                        email = reader.GetString("email"),
                        password = reader.GetString("password")
                    };

                    userList.Add(user);
                }

                if (userList.Count == 0)
                {
                    Console.WriteLine("Aucun utilisateur");
                }
                else
                {
                    Boolean verifPassword = BC.Verify(password, userList[0].password);
                    if (verifPassword)
                    {   
                        Console.WriteLine("Vous etes connecter");
                        Console.WriteLine("===================");
                        OperationAccount operationAccount = new OperationAccount();
                        operationAccount.TypeOperation(userList[0].uuid);
                    }
                    else
                    {
                        Console.WriteLine("Identifiant Incorect");
                    }

                }
                return userList;
            } catch (Exception ex)
            {
                Console.WriteLine($"Une erreur est survenue: {ex}");
                return userList;
            }
        }

    }
}
