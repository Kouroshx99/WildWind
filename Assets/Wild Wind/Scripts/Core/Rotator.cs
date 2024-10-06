using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WildWind.Core
{

    public class Rotator : MonoBehaviourMaster<Rotator>
    {

        [SerializeField] float rotationFrequency = 1;
        [SerializeField] Vector3 rotationVector = Vector3.forward;

        public void Update()
        {
            
            transform.Rotate(Vector3.forward, rotationFrequency * Time.deltaTime * 360f,Space.Self);

        }

    }

}
