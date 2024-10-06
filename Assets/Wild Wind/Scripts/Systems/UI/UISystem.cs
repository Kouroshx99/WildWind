using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace WildWind.Systems
{
    [Serializable]
    public class UISystem : MonoSingleton<UISystem>
    {

        [Header("Home Menu"), Space(10)]
        #region Home Menu
        [SerializeField] private GameObject homeMenu;
        [SerializeField] Button nextPlaneButton;
        [SerializeField] Button previousPlaneButton;
        [SerializeField] Button startGameButton;
        [SerializeField] Button buyPlaneButton;
        [SerializeField] Button settingButton;
        [SerializeField] Text balance;
        #endregion
        [Space(20)]

        [Header("Game Menu"),Space(10)]
        #region Game Menu
        [SerializeField] private GameObject gameMenu;
        [SerializeField] Button pauseButton;
        [SerializeField] Text time;
        [SerializeField] Text score;
        #endregion
        [Space(20)]

        [Header("Pause Menu"), Space(10)]
        #region Pause Menu
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] Button playButton;
        [SerializeField] Button settingsButton;
        [SerializeField] Button goHomeButton;
        #endregion
        [Space(20)]

        [Header("Finished Menu"), Space(10)]
        #region Finished Menu
        [SerializeField] private GameObject finishedMenu;
        [SerializeField] Button playAgain;
        [SerializeField] Button homeButton;
        #endregion
        [Space(20)]

        [Header("Settings Menu"), Space(10)]
        #region Settings Menu
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] Button closeSettingButton;
        [SerializeField] Button tutorialButton;
        #endregion
        [Space(20)]

        [Header("Tutorial Menu"), Space(10)]
        #region Settings Menu
        [SerializeField] private GameObject tutorialMenu;
        [SerializeField] Button closeTutorialButton;
        #endregion

        public override void Awake()
        {

            base.Awake();
            GameSystem.Instance.OnGameStateChange += UpdateMenusStates;

        }

        public override void Start()
        {

            base.Start();

            #region Setup Home Menu
            nextPlaneButton.onClick.AddListener(GameSystem.Instance.NextPlane);
            nextPlaneButton.onClick.AddListener(UpdateMenusStates);
            previousPlaneButton.onClick.AddListener(GameSystem.Instance.PreviousPlane);
            previousPlaneButton.onClick.AddListener(UpdateMenusStates);
            startGameButton.onClick.AddListener(GameSystem.Instance.StartGameSession);
            buyPlaneButton.onClick.AddListener(GameSystem.Instance.BuyPlane);
            buyPlaneButton.onClick.AddListener(UpdateMenusStates);
            settingButton.onClick.AddListener(() => settingsMenu.SetActive(true));
            #endregion

            #region Setup Game Menu
            pauseButton.onClick.AddListener(GameSystem.Instance.PauseGame);
            #endregion

            #region Setup Pause Menu
            playButton.onClick.AddListener(GameSystem.Instance.ResumeGame);
            goHomeButton.onClick.AddListener(GameSystem.Instance.AbortSession);
            settingsButton.onClick.AddListener(() => settingsMenu.SetActive(true));
            #endregion

            #region Setup Finish Menu
            homeButton.onClick.AddListener(GameSystem.Instance.LoadHomeMenu);
            playAgain.onClick.AddListener(GameSystem.Instance.StartGameSession);
            #endregion

            #region Setup Settings Menu
            closeSettingButton.onClick.AddListener(() => settingsMenu.SetActive(false));
            tutorialButton.onClick.AddListener(() => tutorialMenu.SetActive(true));
            #endregion

            #region Setup Tutorial Menu
            closeTutorialButton.onClick.AddListener(() => tutorialMenu.SetActive(false));
            #endregion

            UpdateMenusStates();

            GameSystem.Instance.OnGameStart += (() =>
            {
                time.text = "00:00";
                score.text = "0";
            });

        }

        public void Update()
        {

            score.text = ScoringSystem.Instance.score.ToString();
            balance.text = SaveData.instance.balance.ToString();
            if (GameSystem.Instance.gameState == GameSystem.GameState.Playing)
                time.text = (int)(GameSystem.Instance.time / 60) + ":" + (int)((GameSystem.Instance.time % 60) / 10) + "" + (int)(GameSystem.Instance.time % 60) % 10;

        }

        private void UpdateMenusStates()
        {

            homeMenu.SetActive(GameSystem.Instance.gameState == GameSystem.GameState.Home);
            gameMenu.SetActive(GameSystem.Instance.gameState == GameSystem.GameState.Playing || GameSystem.Instance.gameState == GameSystem.GameState.Paused);
            pauseMenu.SetActive(GameSystem.Instance.gameState == GameSystem.GameState.Paused);
            finishedMenu.SetActive(GameSystem.Instance.gameState == GameSystem.GameState.Finished);
            pauseButton.gameObject.SetActive(GameSystem.Instance.gameState == GameSystem.GameState.Playing);
            startGameButton.gameObject.SetActive(SaveData.instance.unlockedPlanes.Contains(SaveData.instance.planeId));
            buyPlaneButton.gameObject.SetActive(!SaveData.instance.unlockedPlanes.Contains(SaveData.instance.planeId));

        }

    }

}
