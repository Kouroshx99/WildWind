using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace WildWind.Core
{

    public class CloudsControl : MonoBehaviourMaster<CloudsControl>
    {

        private VisualEffect clouds;
        private Vector3 centerOfSpawn = Vector3.zero;

        public override void Start()
        {

            base.Start();

            clouds = GetComponent<VisualEffect>();

        }

        public void Update() => clouds.SetVector3("Center of Spawn",centerOfSpawn);

        public void SetCenterOfSpawn(Vector3 vector3) => centerOfSpawn = vector3;

    }

}
