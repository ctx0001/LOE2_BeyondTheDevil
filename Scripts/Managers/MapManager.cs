using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GameScene.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace Managers
{
    public class MapManager : DataHandler
    {
        [SerializeField] private GameObject lights;
        [SerializeField] private GameObject lightsOnAudio;
        [SerializeField] private GameObject lightsOffAudio;

        private Root _mapData;
        public static MapManager Instance;

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
            SetPath("MapData.json");
            _mapData = FetchData<Root>(GetPath());
            
            if (_mapData.lights.Equals("off"))
                DisableLights();
            else
                Debug.Log("Lights are on");
        }

        internal void EnableLights()
        {
            Instantiate(lightsOnAudio, transform.position, Quaternion.identity);
            lights.SetActive(true);
            _mapData.lights = "on";
            UpdateData(GetPath(), _mapData);
        }

        internal void DisableLights()
        {
            Instantiate(lightsOffAudio, transform.position, Quaternion.identity);
            lights.SetActive(false);
            _mapData.lights = "off";
            UpdateData(GetPath(), _mapData);
        }

        [Serializable]
        public class Root
        {
            public string lights { get; set; }
        }
        
    }
}