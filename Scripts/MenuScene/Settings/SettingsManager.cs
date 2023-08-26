using Localization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MenuScene.Settings
{
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager Instance;

        [SerializeField] private TMP_Dropdown dropdown;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(Instance);
            }
            else
            {
                Instance = this;
                if(SceneManager.GetActiveScene().name == "Menu")
                    FetchLanguageDropdown();
            }
        }

        /**
         * <summary>Changes the language of the game given an abbreviation like
         * it, en, es ...</summary>
         * after updating the game language choice, it also translate the entire
         * game calling: LocalizationManager.LoadData()
         * 
         * <param name="abbr">abbreviation</param>
         * <param name="loadData">whether should translate or not</param>
         */
        public void SwitchToLanguage(string abbr)
        {
            PlayerPrefs.SetString("Language", abbr);
            StartCoroutine(LoadDataSafely());
        }

        public void SwitchToLanguageDropdown()
        {
            var index = dropdown.value;
            var abbreviations = new List<string>() { "en", "es", "ru", "de", "zh", "it", "fr", "jp", "kr", "pt" };
            PlayerPrefs.SetString("Language", abbreviations[index]);
            StartCoroutine(LoadDataSafely());
        }

        private IEnumerator LoadDataSafely()
        {
            // Assicura che il singleton sia inizializzato
            while(LocalizationManager.Instance == null)
            {
                yield return null;
            }

            // Assicura che abbia gia caricato i dati e la lingua di base
            while (!LocalizationManager.Instance.IsLoaded())
            {
                yield return null;
            }

            // prevent from loading the same language pack twice
            if (LocalizationManager.Instance.GetCurrentLoadedLanguage() == PlayerPrefs.GetString("Language")) yield break;

            LocalizationManager.Instance.LoadData();
        }

        private void FetchLanguageDropdown()
        {
            var currentLanguage = PlayerPrefs.GetString("Language");
            var abbreviations = new List<string>() { "en", "es", "ru", "de", "zh", "it", "fr", "jp", "kr", "pt" };
            var index = abbreviations.IndexOf(currentLanguage);
            dropdown.value = index;
        }
    }
}