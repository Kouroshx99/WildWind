using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WildWind.Systems
{

    [Serializable]
    public class SaveData
    {

        private static SaveData _instance;
        public static SaveData instance
        {

            get
            {

                if (_instance != null)
                    return _instance;

                if (PlayerPrefs.HasKey("LocalGameData"))
                    _instance = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("LocalGameData"));
                else
                {

                    _instance = new SaveData();
                    PlayerPrefs.SetString("LocalGameData", JsonUtility.ToJson(_instance));

                }

                return _instance;

            }
            set
            {

                _instance = value;

            }

        }

        [SerializeField] private int _highestRecord = 0;
        public int highestRecord
        {

            get
            {

                return _highestRecord;

            }
            set
            {

                if (value > _highestRecord)
                    _highestRecord = value;
                GooglePlayGamesSystem.Instance.SaveGameData();

            }

        }

        [SerializeField]private int _planeId = 0;
        public int planeId
        {

            get
            {

                return _planeId;

            }
            set
            {

                _planeId = value;
                if (GooglePlayGamesSystem.Instance != null)
                    GooglePlayGamesSystem.Instance.SaveGameData();
                else
                    Debug.LogError("platform is null");

            }

        }

        [SerializeField] private List<int> _unlockedPlanes = new List<int>() { 0};
        public List<int> unlockedPlanes
        {

            get
            {

                if (!_unlockedPlanes.Contains(1))
                    _unlockedPlanes.Add(1);
                return _unlockedPlanes;

            }
            set
            {

                _unlockedPlanes = value;

            }

        }

        [SerializeField] private int _balance = 0;
        public int balance
        {

            get
            {

                return _balance;

            }
            set
            {

                if (value >= 0)
                    _balance = value;
                else 
                    _balance = 0;

            }

        }

    }

}
