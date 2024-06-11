using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace loto
{
    public class LotoService : ILotoService, IPlayerService
    {
        static List<Player> players = new List<Player>();
        static Dictionary<string, IPlayerCallback> subscribers = new Dictionary<string, IPlayerCallback>();

        public void InitPlayer(string playerName, int firstNumber, int secondNumber, int amount)
        {
            var callback = OperationContext.Current.GetCallbackChannel<IPlayerCallback>();
            foreach (var person in players)
            {
                if (person.Name.Equals(playerName))
                {
                    callback.MessageArrived("VEC STE UPLATILI LISTIC ZA OVU RUNDU, SACEKAJTE");
                }
            }

            if (!subscribers.ContainsKey(playerName))
                subscribers.Add(playerName, callback);
            

            Player player = new Player {Name = playerName ,FirstNumber = firstNumber, SecondNumber = secondNumber, Amount = amount };
            players.Add(player);
        }

        public void CalculateLoto(int firstLotoNumber, int secondLotoNumber)
        {
            Dictionary<string, int> playerEarnings = new Dictionary<string, int>();
            Dictionary<string, int> playerMatches = new Dictionary<string, int>();

            foreach(var player in players)
            {
                int matches = 0;
                if (player.FirstNumber == firstLotoNumber )
                {
                    matches++;
                    if (player.SecondNumber == secondLotoNumber)
                        matches++;
                }
                else if (player.FirstNumber == secondLotoNumber)
                {
                    matches++;
                    if (player.SecondNumber == firstLotoNumber)
                        matches++;
                }
                else if (player.SecondNumber == firstLotoNumber || player.SecondNumber == secondLotoNumber) matches++;

                int earnings = 0;
                if (matches == 1) earnings = player.Amount;
                if (matches == 2) earnings = player.Amount * 5;

                playerEarnings[player.Name] = earnings - player.Amount;
                playerMatches[player.Name] = matches;
            }


            var sortedEarnings = new List<KeyValuePair<string, int>>(playerEarnings);
            sortedEarnings.Sort((x, y) => y.Value.CompareTo(x.Value));

            string leaderboard = string.Join(Environment.NewLine, sortedEarnings);


            foreach(var subscriber in subscribers)
            {
                string name = subscriber.Key;
                var callback = subscriber.Value;
                var totalEarnings = playerEarnings[name];
                var totalMatches = playerMatches[name];

                callback.LotoResult(name, firstLotoNumber, secondLotoNumber, totalMatches, (int) totalEarnings, leaderboard);
            }

            players = new List<Player>();
            subscribers = new Dictionary<string, IPlayerCallback>();

        }

    }
}
