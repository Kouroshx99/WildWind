using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using WildWind.Control;
using WildWind.Core;
using WildWind.Movement;

namespace WildWind.Powerup
{

    public class Booster : MonoBehaviour
    {

        [SerializeField] private int boosterTime = 5;
        [SerializeField] private float boosterSpeedMultiplication = 1.5f;

        private bool isConsumed = false;
        private const string playerTag = "Player";
        private static CancellationTokenSource cancellationTokenSource;
        private static Task task;
        MoverData boosterMoverData;

        private async void OnTriggerEnter(Collider other)
        {
            if (!isConsumed && other.CompareTag(playerTag))
            {

                isConsumed = true;
                Destroy(gameObject);
                PlayerController playerController = other.GetComponentInParent<PlayerController>();

                if (cancellationTokenSource != null) cancellationTokenSource.Cancel();

                if (task != null)
                {
                    await task;
                    cancellationTokenSource.Dispose();
                }

                ApplyBooster(playerController);

            }
        }

        private void ApplyBooster(PlayerController playerController)
        {

            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;

            boosterMoverData = ScriptableObject.CreateInstance<MoverData>();
            CreateBoosterMoverData(boosterMoverData, playerController);

            float initTime = Time.time;

            Func<bool> violationCondition = (() =>
            {
                return boosterTime + initTime < GameTime.Instance.time;
            });

            new ObjectSwapper<MoverData>(
                () => { return ref playerController.moverData; },
                () => { return ref boosterMoverData; }, 
                violationCondition, cancellationTokenSource, ref task);
        }

        private void CreateBoosterMoverData(MoverData moverData,PlayerController playerController)
        {
            moverData.speed = playerController.GetMoverData().speed * boosterSpeedMultiplication;
            moverData.yawRate = playerController.GetMoverData().yawRate;
            moverData.rollRate = playerController.GetMoverData().rollRate;
        }

    }

}