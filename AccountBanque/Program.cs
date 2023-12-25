using AccountBanque;
using MySqlConnector;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics;
using System.Linq.Expressions;


class PublicApp
{
    
    public static void Main(string[] args)
    {
        Console.WriteLine("Inscriptioon: 1 Connexion: 2");

        if (int.TryParse(Console.ReadLine(), out int choixUser))
        {
            Console.WriteLine("Le choix est bon");
            switch (choixUser)
            {
                case 1:
                    Console.WriteLine("Inscription user");
                    Console.WriteLine("===================");
                    InscriptionUser();
                    break;
                case 2:
                    Console.WriteLine("Connexion user");
                    Console.WriteLine("===================");
                    ConnexionUser();
                    break;
                default:
                    Console.WriteLine("Choix non reconnue");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Veuillez entrer un choix valid");
        }
    }
    public static void InscriptionUser()
    {
        Console.WriteLine("Entrez votre nom:");
        var nom = Console.ReadLine();
        Console.WriteLine("Entrez votre prenom:");
        var prenom = Console.ReadLine();
        Console.WriteLine("Entrez votre addresse email");
        var email = Console.ReadLine();
        Console.WriteLine("Entrez votre mot de passe:");
        var password = Console.ReadLine();
        if (nom != null && prenom != null && email != null && password != null)
        {
            ConnectionInscriptionUser connexionDB = new ConnectionInscriptionUser();
            connexionDB.InscriptionUser(nom, prenom, email, password);
        }
        else
        {
            Console.WriteLine("Veuillez saisir votre nom et prenom");
        }
    }

    public static void ConnexionUser()
    {
        Console.WriteLine("Entrez votre addresse email:");
        string? connexionEmail = Console.ReadLine();
        Console.WriteLine("Entrez votre mot de passe:");
        string? connexionPassword = Console.ReadLine();
        if (connexionEmail != null && connexionPassword != null)
        {
            ConnectionInscriptionUser connexionDB = new ConnectionInscriptionUser();
            connexionDB.ConnexionUser(connexionEmail, connexionPassword);
        }
        else
        {
            Console.WriteLine("Veuillez entre vos identifiant");
        }
    }
    
}