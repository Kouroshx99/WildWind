using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System;
using UnityEngine.Timeline;

namespace WildWind.Systems.Spawn
{

    [System.Serializable]
    [TrackColor(53/255f, 152 / 255f, 219 / 255f)]
    [TrackBindingType(typeof(SpawnContainer))]
    [TrackClipType(typeof(SpawnContainerClip))]
    public class SpawnContainerTrack : TrackAsset
    {

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {

            return ScriptPlayable<SpawnContainerMixer>.Create(graph, inputCount);

        }

    }

}
