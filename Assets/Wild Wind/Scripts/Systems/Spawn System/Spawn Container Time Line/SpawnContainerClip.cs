using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace WildWind.Systems.Spawn
{

    public class SpawnContainerClip : PlayableAsset, ITimelineClipAsset
    {

        public ClipCaps clipCaps
        {

            get
            {

                return ClipCaps.Blending;

            }

        }

        [SerializeField]
        SpawnContainerBehaviour spawnContainerBehaviour = new SpawnContainerBehaviour();

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {

            return ScriptPlayable<SpawnContainerBehaviour>.Create(graph, spawnContainerBehaviour);

        }

    }

}
