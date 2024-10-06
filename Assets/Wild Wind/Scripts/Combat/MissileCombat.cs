using UnityEngine;

namespace WildWind.Combat
{

    public class MissileCombat : Combat
    {

        private const string playerTag = "Player";
        private const string missileTag = "Missile";

        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag(playerTag) || other.CompareTag(missileTag))
                Destroy(gameObject);

        }

    }

}