using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using WildWind.Control;
using WildWind.Core;
using WildWind.Movement;

namespace WildWind.Systems
{

    public class GameSystem : MonoSingleton<GameSystem>
    {

        #region Variables
        [SerializeField] private PlayerController[] planes;
        [SerializeField] private CameraController P_cameraController;
        private CameraController cameraController;
        [HideInInspector] public PlayerController player { get; private set; }
        private PlayerController plane;
        public float time { get; private set; }
        private bool isGameAborted { get; set; }
        #endregion

        #region Events
        internal Action OnGameStart { get; set; }
        internal Action OnFinished { get; set; }
        internal Action OnLevelCleared { get; set; }
        #endregion

        #region Game State
        public enum GameState { Home, Paused, Playing, Finished, None };
        private GameState _gameState;
        public GameState gameState 
        { 
            get
            {

                return _gameState;

            }
            private set 
            {

                _gameState = value;
                OnGameStateChange(); 

            } 
        }
        internal Action OnGameStateChange;
        #endregion

        public override void Start()
        {

            base.Start();

            LoadHomeMenu();
            OnLevelCleared += ResetTime;
            OnLevelCleared += () => Time.timeScale = 1;
            Application.targetFrameRate = 120;

        }

        public void Update() => HandleGameState();

        #region Game Mechanics

        /// <summary>
        /// Handles the mechanics related to each state of the game
        /// </summary>
        private void HandleGameState()
        {

            switch (gameState)
            {

                case GameState.Playing:
                    {

                        time += Time.deltaTime;
                        if (FindObjectOfType<CloudsControl>() != null)
                        {

                            FindObjectOfType<CloudsControl>().SetCenterOfSpawn(player.transform.position);

                        }
                        break;

                    }
                default:
                    {

                        break;

                    }

            }

        }

        /// <summary>
        /// resets the time for each game session
        /// </summary>
        private void ResetTime() => time = 0;

        /// <summary>
        /// Sets the player as the alert center, GUI alerts calculate their position using the alertCenter transform
        /// </summary>
        private void SetPlayerAsAlertCenter()
        {

            Alert.alertCenter = player.transform;

        }

        /// <summary>
        /// Instantiates the orphographic game camera
        /// </summary>
        private void SetupCamera()
        {

            cameraController = Instantiate(P_cameraController);
            cameraController.SetFollowTarget(player.gameObject);

        }

        /// <summary>
        /// instantiates the player
        /// </summary>
        private void InstantiatePlayer()
        {

            gameState = GameState.Playing;
            player = Instantiate(planes[SaveData.instance.planeId], Vector3.zero, Quaternion.identity);
            SetupCamera();
            SetPlayerAsAlertCenter();
            player.OnDeath += HandleOnGameFinish;

        }

        /// <summary>
        /// returns an array contatining all of the planes present in game
        /// </summary>
        /// <returns></returns>
        public PlayerController[] GetPlanes()
        {

            return planes;

        }

        internal void BuyPlane()
        {

            int balance = SaveData.instance.balance;
            bool buyProccessed = planes[SaveData.instance.planeId].Buy(ref balance);
            if (buyProccessed)
            {

                SaveData.instance.balance = balance;
                SaveData.instance.unlockedPlanes.Add(SaveData.instance.planeId);

            }

        }

        #endregion

        #region Game session Management
        internal void StartGameSession()
        {

            StartCoroutine(E_StartGameSession());

        }

        private IEnumerator E_StartGameSession()
        {

            for (int j = 0; j < SceneManager.sceneCount; j++)
            {

                Scene scene = SceneManager.GetSceneAt(j);
                if (scene.name == "S_Home")
                    yield return SceneManager.UnloadSceneAsync("S_Home");
                if (scene.name == "S_Game")
                {

                    AsyncOperation operation = SceneManager.UnloadSceneAsync("S_Game");
                    operation.completed += ((op) =>
                    {

                        if (OnLevelCleared != null)
                            OnLevelCleared();

                    });
                    yield return operation;

                }

            }

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("S_Game", LoadSceneMode.Additive);
            asyncOperation.completed += ((op) =>
            {

                SceneManager.SetActiveScene(SceneManager.GetSceneByName("S_Game"));
                HandleOnGameStart();

            });
            yield return asyncOperation;

        }

        private void HandleOnGameStart()
        {

            if (OnGameStart != null)
                OnGameStart();
            gameState = GameState.Playing;
            InstantiatePlayer();

        }

        private void HandleOnGameFinish()
        {
            
            if (isGameAborted)
                return;
            if (OnFinished != null)
                OnFinished();
            SaveData.instance.highestRecord = ScoringSystem.Instance.score;
            gameState = GameState.Finished;
            isGameAborted = false;

        }

        internal void PauseGame()
        {

            gameState = GameState.Paused;
            Time.timeScale = 0;

        }

        internal void ResumeGame()
        {

            gameState = GameState.Playing;
            Time.timeScale = 1;

        }

        internal void AbortSession()
        {

            isGameAborted = true;
            LoadHomeMenu();
            isGameAborted = false;

        }

        #endregion

        #region Home Menu

        #region Load Home Menu

        internal void LoadHomeMenu()
        {

            for (int j = 0; j < SceneManager.sceneCount; j++)
            {

                Scene scene = SceneManager.GetSceneAt(j);
                if (scene.name == "S_Game")
                {

                    SceneManager.UnloadSceneAsync(scene).completed += ((op) =>
                    {

                        if (OnLevelCleared != null)
                            OnLevelCleared();
                        gameState = GameState.None;

                    });

                }

            }

            SceneManager.LoadSceneAsync("S_Home", LoadSceneMode.Additive).completed += (op =>
            {
                gameState = GameState.Home;
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("S_Home"));
                InstantiatePlane();
            });

        }

        #endregion

        /// <summary>
        /// Instantiates the next plane
        /// </summary>
        internal void NextPlane()
        {

            SaveData.instance.planeId = Mathf.Clamp(SaveData.instance.planeId + 1, 0, GetPlanes().Length - 1);

            InstantiatePlane();

        }
        
        /// <summary>
        /// Instantiates the previous plane
        /// </summary>
        internal void PreviousPlane()
        {

            SaveData.instance.planeId = Mathf.Clamp(SaveData.instance.planeId - 1, 0, GetPlanes().Length - 1);
            InstantiatePlane();

        }

        /// <summary>
        /// Instantiates the plane from list with the current Plane ID, disables all controls 
        /// </summary>
        private void InstantiatePlane()
        {

            if (plane != null)
                Destroy(plane.gameObject);
            plane = Instantiate(GetPlanes()[SaveData.instance.planeId]);
            plane.GetComponent<PlayerController>().enabled = false;
            plane.GetComponent<Combat.Combat>().enabled = false;
            plane.transform.position = Vector3.zero;

        }
        #endregion

    }

}
