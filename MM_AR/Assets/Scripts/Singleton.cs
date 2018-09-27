using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMAR
{
    public class Singleton<T> : MonoBehaviour where T:MonoBehaviour
    {

        private static T myInstance;
        public static T Instance
        {
            get
            {
                if (myInstance == null)
                {
                    myInstance = FindObjectOfType<T>();
                    if (myInstance==null)
                    {
                        myInstance = new GameObject(typeof(T).Name).AddComponent<T>();
                    }
                }
                return myInstance;
            }
        }

    }
}