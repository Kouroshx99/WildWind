using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using WildWind.Control;
using WildWind.Core;
using WildWind.Movement;
using WildWind.Systems;

namespace WildWind.Powerup
{

    public class ScoreMultiplier : MonoBehaviourMaster<ScoreMultiplier>
    {

        [SerializeField] private float scoreMultplierTime;
        [SerializeField] private int scoreMultplier;
        private bool isConsumed = false;
        private const string playerTag = "Player";
        private static CancellationTokenSource cancellationTokenSource;
        private static Task task;

        private async void OnTriggerEnter(Collider other)
        {

            if (!isConsumed && other.CompareTag(playerTag))
            {

                isConsumed = true;
                Destroy(gameObject);

                if (cancellationTokenSource != null) cancellationTokenSource.Cancel();

                if (task != null)
                {
                    await task;
                    cancellationTokenSource.Dispose();
                }

                ApplyScoreMultiplier();

            }

        }

        void ApplyScoreMultiplier()
        {

            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;

            float initTime = Time.time;

            Func<bool> violationCondition = (() =>
            {
                return scoreMultplierTime + initTime < GameTime.Instance.time;
            });
            new ObjectSwapper<int>(
                () => { return ref ScoringSystem.Instance.scoreMultiplier; },
                () => { return ref scoreMultplier; },
                violationCondition, cancellationTokenSource, ref task);

        }

    }

}
