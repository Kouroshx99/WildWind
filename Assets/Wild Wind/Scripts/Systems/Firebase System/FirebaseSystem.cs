using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Analytics;

namespace WildWind.Systems
{

    public class FirebaseSystem : MonoSingleton<FirebaseSystem>
    {
        public override void Awake()
        {

            base.Awake();

        }
        public override void Start()
        {

            base.Start();

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            });

        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override void OnEnable()
        {
            base.OnEnable();
        }

    }

}
