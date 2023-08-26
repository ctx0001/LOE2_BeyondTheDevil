using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Data.Handlers
{
    public class DoorDataHandler : DataHandler
    {
        private List<DoorData> _doorData;
        public static DoorDataHandler Instance;
        
        private void Awake() 
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this;
            } 
        }

        private void Start()
        {
            SetPath("DoorsData.json");
            _doorData = FetchData<List<DoorData>>(GetPath());
            MarkLoaded();
        }

        internal void UpdateDoorState(string id, bool unlocked, bool blocked)
        {
            foreach (var door in _doorData)
            {
                if (door.id != id) continue;
                door.unlocked = unlocked;
                door.blocked = blocked;
            }
            
            UpdateData(GetPath(), _doorData);
        }
        
        internal bool IsDoorBlocked(string doorId)
        {
            foreach (var door in _doorData)
            {
                if(door.id == doorId)
                    return door.blocked;
            }

            Debug.LogWarning($"No door with id {doorId} found.");
            return false;
        }
        
        internal bool IsDoorUnlocked(string doorId)
        {
            foreach (var door in _doorData)
            {
                if(door.id == doorId)
                    return door.unlocked;
            }

            Debug.LogWarning($"No door with id {doorId} found.");
            return false;
        }
    }
    
    public class DoorData
    {
        public string id { get; set; }
        public bool blocked { get; set; }
        public bool unlocked { get; set; }
    }
}