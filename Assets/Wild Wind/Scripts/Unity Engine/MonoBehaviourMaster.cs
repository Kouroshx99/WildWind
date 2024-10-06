using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    [System.Serializable]
    public class MonoBehaviourMaster<T> : MonoBehaviour where T : MonoBehaviourMaster<T>
    {

        public static Action OnEnableStatic;
        public static Action OnAwakeStatic;
        public static Action OnStartStatic;
        public static Action OnTickStatic;
        public static Action OnTickLateStatic;
        public static Action OnDeathStatic;
        public Action OnEnabled;
        public Action OnAwake;
        public Action OnStart;
        public Action OnTick;
        public Action OnTickLate;
        public Action OnDeath;

        public virtual void OnEnable()
        {
            
            if(OnEnabled != null)
            {

                OnEnabled();

            }
            if(OnEnableStatic != null)
            {

                OnEnableStatic();

            }

        }

        public virtual void Awake()
        {

            if (OnAwake != null)
                OnAwake();
            if (OnAwakeStatic != null)
            {
                OnAwakeStatic();
            }

        }

        public virtual void Start()
        {

            if (OnStart != null)
                OnStart();
            if (OnStartStatic != null)
            {
                OnStartStatic();

            }

        }


        /*public virtual void Update()
        {

            if (OnTick != null)
                OnTick();
            if (OnTickStatic != null)
                OnTickStatic();

        }

        public virtual void LateUpdate()
        {

            if (OnTickLate != null)
                OnTickLate();
            if (OnTickLateStatic != null)
                OnTickLateStatic();

        }*/

        public virtual void OnDestroy()
        {

            if (OnDeath != null)
                OnDeath();
            if (OnDeathStatic != null)
                OnDeathStatic();

        }

    }

}
