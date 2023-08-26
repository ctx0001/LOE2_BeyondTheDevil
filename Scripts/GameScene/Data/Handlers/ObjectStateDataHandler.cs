using System;
using System.Collections.Generic;
using System.Linq;
using DebugFolder;
using Newtonsoft.Json;
using UnityEngine;

namespace GameScene.Data.Handlers
{
    public class ObjectStateDataHandler : DataHandler
    {
        [SerializeField] private GameObject[] objects;
        private List<DataObject> _interactableObjects;
        public static ObjectStateDataHandler Instance;
        
        private void Awake() 
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        private void Start()
        {
            SetPath("ObjectsData.json");
            _interactableObjects = FetchData<List<DataObject>>(GetPath());
            MarkLoaded();
            InitializeObjectState();
        }

        /**
         * Checks if interactable object is already taken by his state.
         * if is already used it sets to inactive object.
         * This function gets the data from ObjectsData.json
         */
        private void InitializeObjectState()
        {
            foreach (var interactable in _interactableObjects)
            {
                foreach (var obj in objects)
                {
                    if (!obj.name.Equals(interactable.name)) continue;
                    obj.SetActive(interactable.state.Equals("active"));
                }
            }
        }

        public void UpdateObjectState(string objectName, string state)
        {
            foreach (var interactable in _interactableObjects.Where(interactable => interactable.name == objectName))
            {
                interactable.state = state;
            }
            UpdateData(GetPath(), _interactableObjects);
        }

        [Serializable]
        private class DataObject
        {
            public string name { get; set; }
            public string state { get; set; }
        }
    }
}