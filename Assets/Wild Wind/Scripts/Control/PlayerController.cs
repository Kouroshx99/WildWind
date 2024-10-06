using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WildWind.Movement;
using WildWind.Core;
using UnityEngine.Playables;
using UnityEditor;

namespace WildWind.Control
{

    [System.Serializable]

    public class PlayerController : MonoBehaviourMaster<PlayerController>, IBuyable
    {

        [SerializeField] public MoverData moverData;

        [SerializeField] private ObjectType moverType;
        private IMover _mover;
        public IMover mover
        {
            get
            {

                if (_mover == null)
                    _mover = Activator.CreateInstance(moverType.type) as IMover;

                return _mover;

            }
        }
        [SerializeField]private int _price;
        public int price { get { return _price; } set { _price = value; } }

        public void Update()
        {

            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, -mover.GetRotation() * 30);

            if (Application.platform == RuntimePlatform.WindowsEditor)
            {

                if ((Input.mousePosition.x > Screen.width / 2 && Input.GetMouseButton(0)) || Input.GetKey(KeyCode.RightArrow))
                {
                    mover.Execute(moverData, transform, 1f);
                    return;
                }
                if ((Input.mousePosition.x < Screen.width / 2 && Input.GetMouseButton(0)) || Input.GetKey(KeyCode.LeftArrow))
                {
                    mover.Execute(moverData, transform, -1f);
                    return;
                }

                mover.Execute(moverData, transform, 0f);

            }
            if (Application.platform == RuntimePlatform.Android)
            {

                mover.Execute(moverData, transform, 0f);
                if (Input.touchCount == 0)
                    return;

                if (Input.GetTouch(0).position.x > Screen.width / 2)
                    mover.Execute(moverData, transform, 1f);
                if (Input.GetTouch(0).position.x < Screen.width / 2)
                    mover.Execute(moverData, transform, -1f);

            }

        }

        public bool Buy(ref int balance)
        {

            bool canBuy = balance >= price;
            if (canBuy)
                balance -= price;
            return canBuy;

        }

        public MoverData GetMoverData()
        {

            return moverData;

        }

    }

}