using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Data.Handlers
{
    public class DollDataHandler : DataHandler

    {
        [SerializeField] private GameObject dollPrefab;

        private DollData _dollData;

        [SerializeField] private PlayerDataHandler playerDataManager;

        private void Start()
        {
            SetPath("DollData.json");
            _dollData = FetchData<DollData>(GetPath());

            if (_dollData.status == "active") // && AssignmentsDataHandler.Instance.CheckForAssignmentFromId(18)
            {
                //var currentPosition = new Vector3((float)_dollData.position[0], (float)_dollData.position[1],
                //    (float)_dollData.position[2]);
                /*if (currentPosition != Vector3.zero || !playerDataManager.IsDeadFromDoll())
                {
                    Debug.Log("Instantiating in last position");
                    Instantiate(dollPrefab, currentPosition, Quaternion.identity);
                }
                else
                {
                    Debug.Log("Instantiating in default position");
                }*/
                var defaultPosition = new Vector3((float)_dollData.defaultPosition[0],
                    (float)_dollData.defaultPosition[1], (float)_dollData.defaultPosition[2]);
                Instantiate(dollPrefab, defaultPosition, Quaternion.identity);
                playerDataManager.SetDeadByDollStatus(false);
            }
        }

        public void InstantiateDoll(Vector3 position)
        {
            Instantiate(dollPrefab, position, Quaternion.identity);
            SetDollStatus(true);
        }

        public string GetDollStatus()
        {
            return _dollData.status;
        }

        public void SetDollStatus(bool active)
        {
            _dollData.status = active ? "active" : "inactive";
            UpdateData(GetPath(), _dollData);
        }

        public class DollData
        {
            public string status { get; set; }
            public List<double> position { get; set; }
            public List<double> defaultPosition { get; set; }
        }
    }
}