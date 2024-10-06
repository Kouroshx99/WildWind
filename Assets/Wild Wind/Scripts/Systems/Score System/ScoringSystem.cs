using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace WildWind.Systems
{

    public class ScoringSystem : MonoSingleton<ScoringSystem>
    {
        
        public int scoreMultiplier = 1;
        private int _score = 0;
        public int score
        {

            get
            {

                return _score;

            }
            private set
            {

                _score = value;

            }

        }

        [SerializeField] private int timeScore;
        private Coroutine timer;
        private int defaultScoreMultiplier = 1;

        public override void Start()
        {

            base.Start();
            defaultScoreMultiplier = scoreMultiplier;
            GameSystem.Instance.OnGameStart += StartTimer;
            GameSystem.Instance.OnFinished += StopTimer;
            GameSystem.Instance.OnGameStateChange += (() =>
            { 
                if (GameSystem.Instance.gameState == GameSystem.GameState.Home)
                    ResetScore();
            });
            GameSystem.Instance.OnLevelCleared+= ResetScore;
            GameSystem.Instance.OnLevelCleared += () => scoreMultiplier = defaultScoreMultiplier;
            GameSystem.Instance.OnFinished += RewardCoins;

        }

        private void ResetScore() => score = 0;

        internal void AddScore(int scoreToAdd) => score += (int)(scoreToAdd * scoreMultiplier);

        private void StartTimer() => timer = StartCoroutine("Timer",timer);

        private void StopTimer() => StopCoroutine(timer);

        private IEnumerator Timer()
        {

            while(true)
            {

                yield return new WaitForSeconds(1);
                AddScore(timeScore);

            }

        }

        private void RewardCoins() => SaveData.instance.balance += (int)Mathf.Log(Mathf.Clamp(score, 1, Mathf.Infinity));

    }

}