using System;
using System.Collections.Generic;
using System.Threading.Tasks;
//using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WildWind.Systems.Spawn
{

    [CreateAssetMenu(fileName = "New Spawn Container", menuName = "Spawn Container", order = 0)]
    [Serializable]
    public class SpawnContainer : ScriptableObject
    {

        [SerializeField]
        public List<SpawnObject> spawnObjects = new List<SpawnObject>();

        public int overalChance
        {

            get
            {

                int chance = 0;
                foreach (SpawnObject a in spawnObjects)
                {

                    chance += a.chance;

                }

                return chance;

            }

        }

        public int maxActiveObjects;
        public Vector2 timeRange;
        private int activeObjects = 0;
        private int limitCap;

        private void OnValidate() => limitCap = maxActiveObjects;

        public void IncreaseObjectCount() => activeObjects++;

        public void DecreaseObjectCount()
        {

            activeObjects--;
            limitCap--;
            if (activeObjects < 0)
                activeObjects = 0;

        }

        public bool CanAddObject() => activeObjects < limitCap;

        public void Initialize()
        {

            TimerDelay();
            ResetValues();

        }

        public void ResetValues()
        {

            limitCap = maxActiveObjects;
            activeObjects = 0;

        }

        public async void TimerDelay()
        {

            while (GameSystem.Instance.gameState == GameSystem.GameState.Playing || GameSystem.Instance.gameState == GameSystem.GameState.Paused)
            {

                if (limitCap != maxActiveObjects && GameSystem.Instance.gameState == GameSystem.GameState.Playing)
                {

                    await Task.Delay((int)Random.Range(timeRange.x, timeRange.y) * 1000);
                    limitCap = Mathf.Clamp(limitCap + 1, 0, maxActiveObjects);

                }
                else
                    await Task.Delay(100);

            }

        }

    }

    [Serializable]
    public class SpawnObject
    {

        [SerializeField]
        public GameObject gameObject;
        [SerializeField]
        [Range(0,100)]
        public int chance;

    }

}
