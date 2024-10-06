using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WildWind.Powerup
{
    public class Shield : MonoBehaviourMaster<Shield>
    {

        [SerializeField] EventChannel OnDestroyed;

        private void OnTriggerEnter(Collider other)
        {
            
            if(other.tag == "Player")
                Destroy(gameObject);

        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            OnDestroyed.RaiseEvent();
        }

    }

}