using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Player
{
    public class Callback : ServiceReference1.IPlayerServiceCallback
    {
        public void LotoResult(string playerName, int firstNumber, int secondNumber, int matches, int earnings, string leaderboard)
        {
            Console.WriteLine("======================");
            Console.WriteLine($"Igrač: {playerName}");
            Console.WriteLine($"Izvučeni brojevi: {firstNumber}, {secondNumber}");
            Console.WriteLine($"Broj pogodaka: {matches}");
            Console.WriteLine($"Ukupna zarada: {earnings}");
            Console.WriteLine("Trenutna tabela:");
            Console.WriteLine(leaderboard);
            Console.WriteLine("======================");

        }

        public void MessageArrived(string message)
        {
            Console.WriteLine(message);
        }
    }

    internal class Program
    {
        static ServiceReference1.PlayerServiceClient playerClient;
        static void Main(string[] args)
        {
            InstanceContext ic = new InstanceContext(new Callback());
            playerClient = new ServiceReference1.PlayerServiceClient(ic);

            Console.WriteLine("Unesite korisničko ime:");
            string playerName = Console.ReadLine();

            while (true)
            {
                PlayRound(playerName);

                Console.WriteLine("Da li želite još da igrate? (da/ne)");
                string answer = Console.ReadLine().ToLower();

                if (answer != "da")
                {
                    break;
                }
            }


            Console.WriteLine("Čekanje rezultata...");
        }

        static void PlayRound(string playerName)
        {

            int number1 = GetValidNumber("Unesite prvi broj (1-10):");
            int number2 = GetValidNumber("Unesite drugi broj (1-10):");
            decimal amount = GetValidAmount("Unesite iznos za ulog:");

            playerClient.InitPlayer(playerName, number1, number2, (int) amount);
            

            Console.WriteLine("Čekanje rezultata runde...");
            Console.ReadLine();
        }


        static int GetValidNumber(string prompt)
        {
            int number;
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine();
                if (int.TryParse(input, out number) && number >= 1 && number <= 10)
                {
                    return number;
                }
                else
                {
                    Console.WriteLine("Nevažeći unos. Unesite broj između 1 i 10.");
                }
            }
        }

        static decimal GetValidAmount(string prompt)
        {
            decimal amount;
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine();
                if (decimal.TryParse(input, out amount) && amount > 0)
                {
                    return amount;
                }
                else
                {
                    Console.WriteLine("Nevažeći unos. Unesite pozitivan iznos.");
                }
            }
        }
    }
}
