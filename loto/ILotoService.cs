using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace loto
{
    [ServiceContract]
    public interface ILotoService
    {
        [OperationContract(IsOneWay =true)]
        void CalculateLoto(int firstNumber, int secondNumber);
    }

    
    [ServiceContract(CallbackContract = typeof(IPlayerCallback))]
    public interface IPlayerService
    {
        [OperationContract(IsOneWay =true)]
        void InitPlayer(string playerName, int firstNumber, int secondNumber, int amount);
    }

    public interface IPlayerCallback
    {
        [OperationContract(IsOneWay = true)]
        void LotoResult(string playerName, int firstNumber, int secondNumber, int matches, int earnings, string leaderboard);

        [OperationContract(IsOneWay =true)]
        void MessageArrived(string message);
    }
}
