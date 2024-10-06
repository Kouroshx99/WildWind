using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WildWind.Systems
{

    public abstract class MonoSingleton<T> : MonoBehaviourMaster<MonoSingleton<T>> where T : MonoSingleton<T>
    {

        private static T _instance;

        public static T Instance
        {

            get
            {

                if (_instance == null)
                {

                    _instance = GameObject.FindObjectOfType(typeof(T)) as T;

                    if (_instance == null)
                    {

                        _instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();

                    }

                }
                return _instance;
            }

        }

        public override void Awake()
        {

            base.Awake();
            if (_instance == null)
            {
                _instance = this as T;
            }
            else
            {
                if (_instance != this)
                {
                    DestroyImmediate(this);
                    return;
                }

            }

            DontDestroyOnLoad(this);

        }

    }

}
