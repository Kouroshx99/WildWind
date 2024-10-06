using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine
{

    [CreateAssetMenu(fileName = "New OnNotify Event Channel",menuName = "Events/OnNotify Event Channel")]
    public class EventChannel : ScriptableObject
    {

        public event Action OnNotify;

        public void RaiseEvent()
        {

            OnNotify?.Invoke();

        }

    }

}