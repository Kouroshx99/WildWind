using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System;

namespace WildWind.Systems.Spawn
{

    [Serializable]
    public class SpawnContainerBehaviour : PlayableBehaviour
    {

        public List<SpawnObject> spawnObjects;
        public int maxActiveObjects;
        public Vector2 TimeRange;

    }

}
