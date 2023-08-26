using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MenuScene
{
    public class GameManager : MonoBehaviour
    {
        [Header("Resources")]
        [SerializeField] private List<string> fileNames;
        
        [Header("References")]
        [SerializeField] private CameraManager cameraManager;
        [SerializeField] private AudioSource onPlaySource;
        [SerializeField] private GameObject ambienceMusic;

        [Header("UI")]
        [SerializeField] private GameObject continueButton;
        [SerializeField] private GameObject overWriteAlert;
        [SerializeField] private GameObject errorAlert;

        public static GameManager Instance;
        private AsyncOperation _load;
        
        [Header("Debug")]
        [SerializeField] private bool loaded;
        [SerializeField] private bool canOverWrite;
        [SerializeField] private string userAccepted;
        
        private string _savePath;
        
        private void Awake() 
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this;
                _savePath = Application.persistentDataPath;
            } 
        }
        
        private void Start()
        {
            StartCoroutine(PreloadGame());
            UpdateUi();
        }

        public void NewGame()
        {
            StartCoroutine(NewGameRoutine());
        }
        
        private IEnumerator NewGameRoutine()
        {
            canOverWrite = true;
            if (PlayerPrefs.HasKey("GameStatus"))
            {
                if (PlayerPrefs.GetString("GameStatus") == "NotCompleted")
                {
                    canOverWrite = false;
                    overWriteAlert.SetActive(true);
                }
            }

            while (!canOverWrite)
            {
                switch (userAccepted)
                {
                    case "ok":
                        canOverWrite = true;
                        break;
                    case "cancel":
                        yield break;
                    default:
                        yield return null;
                        break;
                }
            }

            if (canOverWrite)
            {
                PlayerPrefs.SetString("GameStatus", "NotCompleted");
                CreateData();
                StartCoroutine(LoadGame());   
            }

            canOverWrite = false;
            userAccepted = "";
        }

        public void ConsentOverWrite(bool consent)
        {
            if (consent)
            {
                userAccepted = "ok";
            }
            else
            {
                userAccepted = "cancel";
            }
            
            overWriteAlert.SetActive(false);
        }

        public void ContinueGame()
        {
            if (!CheckForMissingFiles())
            {
                errorAlert.SetActive(true);
                RestoreGameData();
                Debug.Log("Missing data patched");   
            }
            else
            {
                StartCoroutine(LoadGame());   
            }
        }

        // returns true if all data files are ok
        private bool CheckForMissingFiles()
        {
            return fileNames.All(fileName => File.Exists(_savePath + "/" + fileName + "Data.json"));
        }

        public void DisableErrorAlert()
        {
            errorAlert.SetActive(false);
            StartCoroutine(LoadGame());
        }

        private void RestoreGameData()
        {
            PlayerPrefs.SetString("GameStatus", "NotCompleted");
            CreateData();
        }
        
        private void CreateData()
        {
            PlayerPrefs.SetInt("FirstGameLoad", 1);
            PlayerPrefs.SetInt("Balance", 0);
            foreach (var fileName in fileNames)
            {
                var path = _savePath + "/"+ fileName + "Data.json";
                try
                {
                    // Read data
                    var preset = Resources.Load<TextAsset>("Data/" + fileName + "Preset");

                    // Create new data
                    using var streamWriter = File.CreateText(path);
                    streamWriter.Write(preset.text);
                    streamWriter.Close();
                }
                catch (Exception)
                {
                    //TODO: add a way to handle the exception
                }   
            }
        }
        
        public IEnumerator PreloadGame()
        {
            _load = SceneManager.LoadSceneAsync("Loader");
            _load.allowSceneActivation = false;

            while (_load.progress >= 0.9f) {
                yield return null;
            }

            loaded = true;
        }

        private IEnumerator LoadGame()
        {
            while (!loaded)
            {
                yield return null;
            }

            MenuManager.Instance.CloseAll();
            
            ambienceMusic.SetActive(false);
            onPlaySource.Play();
            cameraManager.InstantiateEntranceDoor();
            
            PlayerPrefs.SetString("SceneToLoad", "Game");
            yield return new WaitForSeconds(5.5f);
            _load.allowSceneActivation = true;
        }

        public void LoadLoaderWithReference(string reference)
        {
            PlayerPrefs.SetString("SceneToLoad", reference);
            _load.allowSceneActivation = true;
        }

        private void UpdateUi()
        {
            if (PlayerPrefs.GetString("GameStatus") == "NotCompleted") return;
            var btn = continueButton.GetComponent<Button>();
            btn.enabled = false;
            btn.gameObject.GetComponent<Image>().color = Color.gray;
        }

    }
}