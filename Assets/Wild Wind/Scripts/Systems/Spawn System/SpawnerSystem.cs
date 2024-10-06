using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using WildWind.Combat;
using WildWind.Control;
using WildWind.Core;
using WildWind.Movement;
using WildWind.Systems;
using EventHandler = UnityEngine.EventHandler;
using Random = UnityEngine.Random;

namespace WildWind.Systems.Spawn
{

    public class SpawnerSystem : MonoSingleton<SpawnerSystem>
    {

        public float spawnDistance = 100;

        [SerializeField]
        private List<SpawnContainer> spawnContainers;
        private List<GameObject> spawnedObjects = new List<GameObject>();
        private PlayableDirector spawnDirector;

        public override void Awake()
        {

            base.Awake();

            GameSystem.Instance.OnGameStart += ClearObjects;
            PlayerController.OnDeathStatic += ResetSpawnDirector;

        }

        public override void Start()
        {

            base.Start();
            spawnDirector = GetComponent<PlayableDirector>();

            foreach (SpawnContainer a in spawnContainers)
            {

                PlayerController.OnStartStatic += a.Initialize;

            }

        }

        public void Update()
        {

            UpdateSpawnDirector();

            if (GameSystem.Instance.gameState == GameSystem.GameState.Playing)
            {

                for (int j = 0; j < spawnContainers.Count; j++)
                    InstantiateObjects(spawnContainers[j]);

            }

        }

        private void InstantiateObjects(SpawnContainer spawnContainer)
        {

            if (spawnContainer.CanAddObject())
            {

                List<int> chance = new List<int>();
                CalculateChances(spawnContainer, chance);

                int rand = Random.Range(0, spawnContainer.overalChance);

                for (int j = 0; j < chance.Count; j++)
                {

                    if (rand <= chance[j])
                    {

                        Vector3 pos;
                        Transform playerTransform = GameSystem.Instance.player.transform;

                        pos = RandomDirection(playerTransform.forward);
                        pos *= spawnDistance;
                        pos += playerTransform.position;

                        GameObject temp = Instantiate(spawnContainer.spawnObjects[j].gameObject, pos, Quaternion.identity);
                        if (temp.GetComponent<EventHandler>() != null)
                        {

                            temp.GetComponent<EventHandler>().OnStart += spawnContainer.IncreaseObjectCount;
                            temp.GetComponent<EventHandler>().OnDeath += spawnContainer.DecreaseObjectCount;

                        }
                        spawnedObjects.Add(temp);
                        break;

                    }

                }

            }

        }

        private static void CalculateChances(SpawnContainer spawnContainer, List<int> chance)
        {
            for (int j = 0; j < spawnContainer.spawnObjects.Count; j++)
            {

                if (chance.Count != 0)
                    chance.Add(chance[chance.Count - 1] + spawnContainer.spawnObjects[j].chance);
                else
                    chance.Add(spawnContainer.spawnObjects[j].chance);

            }
        }

        private Vector3 RandomDirection(Vector3 direction)
        {
            float randAngle = RandomAngle();
            return new Vector3(Mathf.Cos(randAngle) * direction.x - Mathf.Sin(randAngle) * direction.z, 0,
                Mathf.Sin(randAngle) * direction.x + Mathf.Cos(randAngle) * direction.z);
        }

        private float RandomAngle()
        {
            return Random.Range(0, Mathf.PI * 2);
        }

        private void UpdateSpawnDirector() => spawnDirector.time = 
            Mathf.Clamp(ScoringSystem.Instance.score,0,(float)spawnDirector.duration - 1);
        
        private void ResetSpawnDirector() => spawnDirector.time = 0;

        private void ClearObjects()
        {

            foreach(GameObject a in spawnedObjects)
            {

                Destroy(a);

            }

            spawnedObjects.Clear();

        }

    }

}
