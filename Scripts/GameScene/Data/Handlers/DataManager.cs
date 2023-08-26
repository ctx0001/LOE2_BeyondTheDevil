using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScene.Data.Handlers
{
    public class DataManager : MonoBehaviour
    {
        [SerializeField] private List<DataHandler> dataHandlers;

        public static DataManager Instance;
        private bool _loaded;
        
        private void Start()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                StartCoroutine(CheckForDataLoaded());
            }
        }

        public bool IsLoaded() => _loaded;

        private IEnumerator CheckForDataLoaded()
        {
            var loading = true;
            while (loading)
            {
                _loaded = true;
                foreach (var dataHandler in dataHandlers.Where(dataHandler => !dataHandler.IsLoaded()))
                    _loaded = false;

                if (_loaded)
                    loading = false;

                yield return null;
            }
        }
    }
}