using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace LotoMachine
{
    internal class Program
    {
        static ServiceReference1.LotoServiceClient machineClient = new ServiceReference1.LotoServiceClient();
        static readonly RNGCryptoServiceProvider rngCripto = new RNGCryptoServiceProvider();

        static private int GeneratRandomNumber()
        {
            byte[] randomBytes = new byte[1];
            rngCripto.GetBytes(randomBytes);
            return randomBytes[0] % 10 + 1;
        }
        static void Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("Loto runda se startuje");
                var firstLotoNumber = GeneratRandomNumber();
                var secondLotoNumber = GeneratRandomNumber();
                machineClient.CalculateLoto(firstLotoNumber, secondLotoNumber);
                Thread.Sleep(60000);
            }
        }
    }
}
