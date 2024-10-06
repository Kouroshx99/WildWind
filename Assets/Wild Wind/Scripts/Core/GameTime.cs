using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WildWind.Systems;

namespace WildWind.Core
{

    public class GameTime : MonoSingleton<GameTime>
    {

        [HideInInspector]
        public float time = 0;
        [HideInInspector]
        public float deltaTime = 0;
        [HideInInspector]
        public float timeScale;

        private void Update()
        {

            time = Time.time;
            deltaTime = Time.deltaTime;
            timeScale = Time.timeScale;

        }

    }

}