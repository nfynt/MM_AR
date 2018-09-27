using UnityEngine;


namespace MMAR
{
    public class LogManager : Singleton<LogManager>
    {

        public static void Log(string msg)
        {
            Debug.Log(msg);
        }
    }
}
