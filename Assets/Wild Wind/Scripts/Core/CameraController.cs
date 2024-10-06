using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WildWind.Control;


namespace WildWind.Core
{

    public class CameraController : MonoBehaviourMaster<CameraController>
    {

        public void SetFollowTarget(GameObject followTarget)
        {

            GetComponentInChildren<CinemachineVirtualCamera>().Follow = followTarget.transform;

        }

    }

}
