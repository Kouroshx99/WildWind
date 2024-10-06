using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WildWind.Core
{

    public class Star : MonoBehaviourMaster<Star>
    {

        private void OnTriggerEnter(Collider other)
        {

            if (other.tag == "Player")
                Destroy(gameObject);

        }

    }

}
