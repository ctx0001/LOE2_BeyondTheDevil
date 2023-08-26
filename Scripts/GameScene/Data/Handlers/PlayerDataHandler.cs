using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameScene.Data.Handlers
{
    public class PlayerDataHandler : DataHandler
    {
        private PlayerData _playerData;
        [SerializeField] private Movement movement;
        [SerializeField] private GameObject evilDollHandler;

        private void Start()
        {
            SetPath("PlayerData.json");
            _playerData = FetchData<PlayerData>(GetPath());
            
            var currentPosition = new Vector3((float)_playerData.position[0], (float)_playerData.position[1],
                (float)_playerData.position[2]);
            if (currentPosition != Vector3.zero && !IsDeadFromDoll())
            {
                gameObject.transform.position = currentPosition;
            }
            else if(currentPosition == Vector3.zero || IsDeadFromDoll())
            {
                var defaultPosition = new Vector3((float)_playerData.defaultPosition[0], (float)_playerData.defaultPosition[1],
                    (float)_playerData.defaultPosition[2]);
                gameObject.transform.position = defaultPosition;
            }
            
            StartCoroutine(AutoSavePlayerPositionRoutine());
            evilDollHandler.SetActive(true);
        }

        public bool IsDeadFromDoll()
        {
            return _playerData.deathFromDoll;   
        }

        private IEnumerator AutoSavePlayerPositionRoutine()
        {
            while (SceneManager.GetActiveScene().name == "Game")
            {
                SavePlayerPosition();
                yield return new WaitForSeconds(60f);   
            }
        }

        private void SavePlayerPosition()
        {
            if(movement != null)
            {
                var position = movement.GetCurrentPosition();
                _playerData.position = new List<double>() { position.x, position.y, position.z };
                UpdateData(GetPath(), _playerData);
            }
        }

        public void SetDeadByDollStatus(bool status)
        {
            _playerData.deathFromDoll = status;
            UpdateData(GetPath(), _playerData);
        }

        public class PlayerData
        {
            public List<double> position { get; set; }
            public bool deathFromDoll { get; set; }
            public List<double> defaultPosition { get; set; }
        }
    }
}