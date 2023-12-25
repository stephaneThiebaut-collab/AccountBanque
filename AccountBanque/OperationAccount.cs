using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBanque
{
    public  class OperationAccount
    {
        private protected string connexionString = "Server=localhost;User ID=root;Password=753159852456;Database=evolutif";
        public void TypeOperation(string uuid)
        {
            Console.WriteLine("Faire un retrait taper: 1");
            Console.WriteLine("===================");
            Console.WriteLine("Faire un depot taper 2");
            Console.WriteLine("===================");
            Console.WriteLine("Consulter votre solde taper 3");
            Console.WriteLine("===================");
            Console.WriteLine("Pour quitter taper 4");
            if (int.TryParse(Console.ReadLine(), out int choixUser))
            {
                switch (choixUser)
                {
                    case 1:
                        Console.WriteLine("Retrait");
                        Console.WriteLine("===================");
                        retraitAccount(uuid);
                        break;
                    case 2:
                        Console.WriteLine("Depot");
                        Console.WriteLine("===================");
                        depotAccount(uuid);
                        break;
                    case 3:
                        Console.WriteLine("Solde");
                        Console.WriteLine("===================");
                        soldeAccount(uuid);
                        break;
                    case 4:
                        Console.WriteLine("En cours de fermeture ...");
                        exitAccount();
                        break;
                    default:
                        Console.WriteLine("Choix non reconnue");
                        break;
                }
            } else
            {
                Console.WriteLine("Choix non reconnue");
            }
        }
        private protected List<AccountUser> retraitAccount(string uuid)
        {
            List<AccountUser> acountList = new List<AccountUser>();
            try 
            {
                using var connection = new MySqlConnection(connexionString);
                connection.Open();
                var command = new MySqlCommand($"SELECT * FROM account WHERE uuidUser = '{uuid}'", connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    AccountUser accountUser = new AccountUser()
                    {
                        id = reader.GetInt32("id"),
                        uuidUser = reader.GetString("uuidUser"),
                        uuidAccount = reader.GetString("uuidAccount"),
                        montant = reader.GetInt32("montant")
                    };
                    acountList.Add(accountUser);
                }
                connection.Close();
                Console.WriteLine($"Votre solde: {acountList[0].montant}");
                Console.WriteLine("Combien voulez-vous retirez ?");
                if (int.TryParse(Console.ReadLine(), out int retraitUser))
                {
                    if (retraitUser > acountList[0].montant)
                    {
                        Console.WriteLine("Fond insuffisant");
                    } else
                    {
                        int newMontant = acountList[0].montant - retraitUser;
                        connection.Open();
                        var updateMontant = new MySqlCommand($"UPDATE account SET montant = {newMontant}", connection);
                        updateMontant.ExecuteNonQuery();
                        Console.WriteLine("Retrait valider");

                        Guid g = Guid.NewGuid();
                        var insertHistorique = new MySqlCommand($"INSERT INTO historiqueaccount(uuid, uuidUser, typeOperation, montant) VALUES ('{g}', '{acountList[0].uuidUser}', '1', '{retraitUser}')", connection);
                        insertHistorique.ExecuteNonQuery();
                        Console.WriteLine("Opération enregistré");
                    }
                } else
                {
                    Console.WriteLine("Veuillez rentré un montant valid");
                }

                return acountList;
            } catch (Exception ex)
            {
                Console.WriteLine($"Une erreur est survenue: {ex}");
                return acountList;
            }
        }
        protected private List<AccountUser> depotAccount(string uuid) 
        {
            List<AccountUser> acountList = new List<AccountUser>();
            Console.WriteLine("Combien voulez-vous deposez ?");
            try
            {
                if (int.TryParse(Console.ReadLine(), out int depotUser))
                {
                    using var connection = new MySqlConnection(connexionString);
                    connection.Open();
                    var command = new MySqlCommand($"SELECT * FROM account WHERE uuidUser = '{uuid}'", connection);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        AccountUser accountUser = new AccountUser()
                        {
                            id = reader.GetInt32("id"),
                            uuidUser = reader.GetString("uuidUser"),
                            uuidAccount = reader.GetString("uuidAccount"),
                            montant = reader.GetInt32("montant")
                        };
                        acountList.Add(accountUser);
                    }
                    connection.Close();
                    int? depotMontant = depotUser + acountList[0].montant;
                    connection.Open();
                    var updateMontant = new MySqlCommand($"UPDATE account SET montant = {depotMontant}", connection);
                    updateMontant.ExecuteNonQuery();
                    Console.WriteLine("Depot valider");

                    Guid g = Guid.NewGuid();
                    var insertHistorique = new MySqlCommand($"INSERT INTO historiqueaccount(uuid, uuidUser, typeOperation, montant) VALUES ('{g}', '{acountList[0].uuidUser}', '2', '{depotUser}')", connection);
                    insertHistorique.ExecuteNonQuery();
                    Console.WriteLine("Opération enregistré");
                }
                else
                {
                    Console.WriteLine("Veuillez rentré un montant valid");
                }
                return acountList;
            } catch (Exception ex)
            {
                Console.WriteLine($"Une erreur est survenue: {ex}");
                return acountList;
            }
        }
        protected private List<AccountUser> soldeAccount(string uuid) 
        {
            List<AccountUser> acountList = new List<AccountUser>();
            try
            {
                var connection = new MySqlConnection(connexionString);
                connection.Open();
                var command = new MySqlCommand($"SELECT * FROM account WHERE uuidUser = '{uuid}'", connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    AccountUser accountUser = new AccountUser()
                    {
                        id = reader.GetInt32("id"),
                        uuidUser = reader.GetString("uuidUser"),
                        uuidAccount = reader.GetString("uuidAccount"),
                        montant = reader.GetInt32("montant")
                    };
                    acountList.Add(accountUser);
                }
                connection.Close();
                Console.WriteLine($"Le solde de votre compte est de: {acountList[0].montant}");
                return acountList;
            } catch (Exception ex)
            {
                Console.WriteLine($"Une erreur est survenue: {ex}");
                return acountList;
            }
        }
        protected private static void exitAccount() 
        { 
            Environment.Exit(0);
            return;
        }
    }
}
