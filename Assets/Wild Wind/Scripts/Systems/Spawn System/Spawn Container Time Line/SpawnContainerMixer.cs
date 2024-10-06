using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using WildWind.Systems.Spawn;

public class SpawnContainerMixer : PlayableBehaviour
{

    List<SpawnObject> spawnObjects;
    public int maxActiveObjects;
    public Vector2 TimeRange;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {

        int inputCount = playable.GetInputCount();
        SpawnContainer spawnContainer = playerData as SpawnContainer;
        spawnObjects = new List<SpawnObject>();

        float totalWeight = 0;
        List<float> blendedChance = new List<float>();
        float blendedMaxObjects = 0;
        Vector2 blendedTimeRange = Vector2.zero;

        foreach (SpawnObject a in spawnContainer.spawnObjects)
        {

            SpawnObject b = new SpawnObject();
            b.gameObject = a.gameObject;
            b.chance = a.chance;
            spawnObjects.Add(b);
            blendedChance.Add(0);

        }

        for (int j = 0; j < inputCount; j++)
        {

            float inputWeight = playable.GetInputWeight(j);
            ScriptPlayable<SpawnContainerBehaviour> inputPlayable = (ScriptPlayable<SpawnContainerBehaviour>)playable.GetInput(j);
            SpawnContainerBehaviour behaviour = inputPlayable.GetBehaviour();
            if (behaviour == null)
                return;

            for (int i = 0; i < behaviour.spawnObjects.Count; i++)
            {

                blendedChance[i] += (behaviour.spawnObjects[i].chance * inputWeight);
                spawnObjects[i].chance = (int)blendedChance[i];
            }

            blendedMaxObjects += (behaviour.maxActiveObjects * inputWeight);
            blendedTimeRange += (behaviour.TimeRange * inputWeight);

            totalWeight += inputWeight;

        }

        spawnContainer.maxActiveObjects = (int)blendedMaxObjects;
        spawnContainer.spawnObjects = spawnObjects;
        spawnContainer.timeRange = blendedTimeRange;

    }

}
