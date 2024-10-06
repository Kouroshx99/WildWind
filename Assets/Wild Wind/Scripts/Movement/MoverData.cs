using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WildWind.Movement
{

    [CreateAssetMenu(fileName = "New Mover Data", menuName = "Wild Wind/Movement Components/Mover Data")]
    public class MoverData : ScriptableObject
    {

        /// <summary>
        /// unit is m/s
        /// </summary>
        [SerializeField] private float _speed;
        /// <summary>
        /// unit is m/s
        /// </summary>
        public float speed { get => _speed; set => _speed = value; }
        /// <summary>
        /// unit is degree/s, indicates the maximum rate at which the object Yaws
        /// </summary>
        [SerializeField] private float _yawRate;
        /// <summary>
        /// unit is degree/s, indicates the maximum rate at which the object Yaws
        /// </summary>
        public float yawRate { get => _yawRate; set => _yawRate = value; }
        /// <summary>
        /// unit is percent/second, indicates how fast the plane's ailerons raise and lower in a scale of -100% to 100%
        /// </summary>
        [SerializeField, Range(0, 1000)] private float _rollRate;
        /// <summary>
        /// unit is percent/second, indicates how fast the plane's ailerons raise and lower in a scale of -100% to 100%
        /// </summary>
        public float rollRate { get => _rollRate; set => _rollRate = value; }

    }

}