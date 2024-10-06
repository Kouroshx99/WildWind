using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WildWind.Systems;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Text;

namespace WildWind.Systems
{

    public class GooglePlayGamesSystem : MonoSingleton<GooglePlayGamesSystem>
    {

        private PlayGamesPlatform platform;
        private bool isSaving { get; set; }
        [SerializeField] private string savedGameName = "wildwind";
        private SignInStatus signInStatus = SignInStatus.AlreadyInProgress;
        private Action OnUserSignedIn;

        public bool authenticated
        {
            get
            {
                return platform.IsAuthenticated();
            }
        }

        public override void Awake()
        {

            base.Awake();

        }

        public override void Start()
        {

            base.Start();

            OnUserSignedIn += LoadSavedGame;

            InitializePlayGamesPlatform();
            SignInUser();

        }     

        public override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        private void OnApplicationQuit()
        {
            SaveGameData();
        }

        #region Platform Initilization
        /// <summary>
        /// Initializes the play games platform and assigns it to platform variable
        /// </summary>
        private void InitializePlayGamesPlatform()
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            platform = PlayGamesPlatform.Activate();
        }       
        #endregion

        #region Authentication
        private SignInStatus SignInUser()
        {

            signInStatus = SignInStatus.NotAuthenticated;
            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (op) =>
            {

               signInStatus = op;
               if (OnUserSignedIn != null)
               {

                   OnUserSignedIn.Invoke();

               }

            });

            return signInStatus;

        }
        #endregion

        #region Saved Game

        /// <summary>
        /// Reads the saved game from cloud, if network is not available the cashed data is used
        /// </summary>
        public void LoadSavedGame()
        {

            if (Application.platform != RuntimePlatform.Android)
                return;
            isSaving = false;
            platform.SavedGame.OpenWithAutomaticConflictResolution(
                savedGameName,
                DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime,
                OnSavedGameOpened);

        }

        /// <summary>
        /// Saves the game data to playerPrefs, then if network is available the data is saved to cloud, otherwise it is cashed locally
        /// </summary>
        public void SaveGameData()
        {

            try
            {
                SaveOfflineGameData();
                if (Application.platform != RuntimePlatform.Android || platform == null)
                    return;
                isSaving = true;
                platform.SavedGame.OpenWithAutomaticConflictResolution(
                    savedGameName,
                    DataSource.ReadCacheOrNetwork,
                    ConflictResolutionStrategy.UseLongestPlaytime,
                    OnSavedGameOpened);
            }
            catch(Exception e)
            {

                Debug.LogWarning(e.Message);

            }

        }

        /// <summary>
        /// Retrieves the saved game data, then decided wether we are writing data or reading data
        /// </summary>
        /// <param name="status"></param>
        /// <param name="gameMetadata"></param>
        private void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata gameMetadata)
        {

            switch (status)
            {

                case SavedGameRequestStatus.Success:
                    {

                        if (isSaving)
                        {

                            byte[] data = ConvertDataToByte();
                            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
                            SavedGameMetadataUpdate updatedMetadata = builder.Build();
                            platform.SavedGame.CommitUpdate(gameMetadata,
                                updatedMetadata,
                                data,
                                SavedGameWritten);

                        }
                        else
                        {

                            platform.SavedGame.ReadBinaryData(gameMetadata, OnSavedGameLoaded);

                        }
                        break;

                    }

            }

        }

        /// <summary>
        /// loads the latest save game data and loads the data into SaveData class
        /// </summary>
        /// <param name="status"></param>
        /// <param name="data"></param>
        private void OnSavedGameLoaded(SavedGameRequestStatus status, byte[] data)
        {

            switch (status)
            {

                case SavedGameRequestStatus.Success:
                    {

                        if (data == null)
                        {

                            return;

                        }
                        string progress = ConvertByteToData(data);
                        Merge(progress);
                        break;

                    }

            }

        }

        /// <summary>
        /// saves the current state of the game
        /// </summary>
        /// <param name="status"></param>
        /// <param name="gameMetadata"></param>
        private void SavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata gameMetadata)
        {

            switch (status)
            {

                case SavedGameRequestStatus.Success:
                    {

                        break;

                    }

            }

        }

        /// <summary>
        /// Converts JSON data to SaveData and stores it in memory
        /// </summary>
        /// <param name="progress"></param>
        private void Merge(string progress)
        {

            if (progress != "")
            {

                SaveData.instance = JsonUtility.FromJson<SaveData>(progress);

            }

        }

        /// <summary>
        /// Converts data present in saveData to byte
        /// </summary>
        /// <returns></returns>
        private byte[] ConvertDataToByte()
        {

            string data = JsonUtility.ToJson(SaveData.instance);
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            return bytes;

        }

        /// <summary>
        /// converts the given bytes into json data
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private string ConvertByteToData(byte[] bytes)
        {

            string decodedData = Encoding.UTF8.GetString(bytes);
            return decodedData;

        }

        #region Offline Save Game
        private void LoadOfflineGameData()
        {
            if (PlayerPrefs.HasKey("LocalGameData"))
                SaveData.instance = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("LocalGameData"));
            else 
                SaveData.instance = new SaveData();
        }
        private void SaveOfflineGameData()
        {
            PlayerPrefs.SetString("LocalGameData",JsonUtility.ToJson(SaveData.instance));
        }
        #endregion
        #endregion

    }

}
