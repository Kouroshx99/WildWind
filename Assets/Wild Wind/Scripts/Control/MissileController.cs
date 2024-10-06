using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using WildWind.Movement;

namespace WildWind.Control
{
    
    public class MissileController : MonoBehaviourMaster<MissileController>
    {

        public static Action onDestroy;

        [SerializeField] 
        private MoverData moverData;
        [SerializeField] 
        private ObjectType moverType;
        [SerializeField] 
        private float lifeTime = 30;
        private IMover _mover;
        private IMover mover
        {
            get
            {

                if (_mover == null)
                    _mover = Activator.CreateInstance(moverType.type) as IMover;

                return _mover;

            }
            set
            {
                _mover = value;
            }
        }
        private Transform target;

        public override void Start()
        {

            base.Start();
            SetTarget();
            transform.LookAt(target.position);
            transform.forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
            StartCoroutine(WaitForEndOfLife());

        }

        public void Update()
        {

            if (target != null)
            {

                float angleBetween;
                angleBetween = GetAngleBetweenMissileAndTarget();
                mover.Execute(moverData, transform, (Math.Sign(angleBetween)));

            }
            else
                mover.Execute(moverData, transform, 0);

        }

        private IEnumerator WaitForEndOfLife()
        {

            yield return new WaitForSeconds(lifeTime);
            Destroy(gameObject);

        }

        private float GetAngleBetweenMissileAndTarget()
        {

            return Vector3.SignedAngle(new Vector3(transform.forward.x, 0, transform.forward.z), (new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z)).normalized, transform.up);

        }

        private void SetTarget()
        {

            if (FindObjectOfType<PlayerController>() != null)
                target = FindObjectOfType<PlayerController>().transform;

        }

        public override void OnDestroy()
        {

            base.OnDestroy();
            if (onDestroy != null)
                onDestroy();

        }

    }

}
