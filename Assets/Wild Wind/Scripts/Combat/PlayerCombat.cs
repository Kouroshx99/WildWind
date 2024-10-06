using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WildWind.Combat
{

    public class PlayerCombat : Combat
    {

        [SerializeField] 
        private EventChannel OnShieldCollected;
        [SerializeField] 
        private Image shieldImg;
        private int _shields = 0;
        private int shields
        {

            get
            {

                return _shields;

            }
            set
            {

                _shields = Mathf.Clamp(value, 0, 3);
                if(shieldImg == null)
                    shieldImg = GetComponentInChildren<Image>();
                shieldImg.gameObject.SetActive(_shields >= 1);

            }

        }
        private bool isDestructible { get => shields == 0; }
        private const string missileTag = "Missile";
        private bool isColliding = false;

        public override void Start()
        {

            base.Start();
            OnShieldCollected.OnNotify += AddShield;

        }

        private void Update() => isColliding = false;


        private void OnTriggerEnter(Collider other)
        {

            if (isColliding)
                return;
            isColliding = true;

            if (other.CompareTag(missileTag))
            {
                if (isDestructible)
                {
                    OnShieldCollected.OnNotify -= AddShield;
                    Destroy(gameObject);
                }
                shields--;
            }

        }

        private void AddShield() => shields++;

    }

}