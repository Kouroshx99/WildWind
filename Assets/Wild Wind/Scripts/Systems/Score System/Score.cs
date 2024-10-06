using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WildWind.Control;

namespace WildWind.Systems
{

    public class Score : MonoBehaviourMaster<Score>
    {

        [SerializeField] private int score;

        public override void Awake()
        {

            base.Awake();
            OnDeath += CountScore;
            PlayerController.OnDeathStatic += ExpireScore;

        }

        void CountScore()
        {

            ScoringSystem.Instance.AddScore(score);

        }

        void ExpireScore()
        {

            OnDeath -= CountScore;

        }

    }

}