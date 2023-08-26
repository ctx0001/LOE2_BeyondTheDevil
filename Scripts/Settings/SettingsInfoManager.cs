using System.Collections.Generic;
using Localization;
using MenuScene.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Settings
{
    public class SettingsInfoManager : MonoBehaviour
    {
        [Header("Graphic Profile")] 
        [SerializeField] private GameObject graphicPanel;
        [SerializeField] private Slider mediumLevelRamSlider;
        [SerializeField] private Slider highLevelRamSlider;
        [SerializeField] private Slider mediumLevelVRamSlider;
        [SerializeField] private Slider highLevelVRamSlider;
        [SerializeField] private List<TextMeshProUGUI> currentRamTexts;
        [SerializeField] private List<TextMeshProUGUI> currentVRamTexts;
        [SerializeField] private GameObject mediumLevelRecommended;
        [SerializeField] private GameObject highLevelRecommended;
        
        private const int MediumLevelRequirementRam = 4;
        private const int HighLevelRequirementRam = 2;
        private const int MediumLevelRequirementVRam = 2;
        private const int HighLevelRequirementVRam = 4;

        private string _selectedLanguage;
        private AsyncOperation _sceneLoad;
        
        [Header("Language")]
        [SerializeField] private GameObject languagePanel;

        [SerializeField] private GameObject checkmarkEn;
        [SerializeField] private GameObject checkmarkPt;
        [SerializeField] private GameObject checkmarkIt;
        [SerializeField] private GameObject checkmarkFr;
        [SerializeField] private GameObject checkmarkDe;
        [SerializeField] private GameObject checkmarkJp;
        [SerializeField] private GameObject checkmarkZh;
        [SerializeField] private GameObject checkmarkKo;
        [SerializeField] private GameObject checkmarkRu;
        [SerializeField] private GameObject checkmarkEs;

        private List<GameObject> _checkMarks;
        
        public void SelectLanguage(string language)
        {
            _selectedLanguage = language;

            foreach (var checkmark in _checkMarks)
            {
                checkmark.SetActive(false);
            }
            
            switch (_selectedLanguage)
            {
                case "en":
                    checkmarkEn.SetActive(true);
                    break;
                case "pt":
                    checkmarkPt.SetActive(true);
                    break;
                case "it":
                    checkmarkIt.SetActive(true);
                    break;
                case "fr":
                    checkmarkFr.SetActive(true);
                    break;
                case "de":
                    checkmarkDe.SetActive(true);
                    break;
                case "jp":
                    checkmarkJp.SetActive(true);
                    break;
                case "zh":
                    checkmarkZh.SetActive(true);
                    break;
                case "ko":
                    checkmarkKo.SetActive(true);
                    break;
                case "ru":
                    checkmarkRu.SetActive(true);
                    break;
                case "es":
                    checkmarkEs.SetActive(true);
                    break;
            }
        }

        public void ConfirmLanguageSelection()
        {
            SettingsManager.Instance.SwitchToLanguage(_selectedLanguage);
            languagePanel.SetActive(false);
            graphicPanel.SetActive(true);
        }
        
        private void Start()
        {
            _checkMarks = new List<GameObject>()
            {
                checkmarkEn, checkmarkPt, checkmarkIt, checkmarkFr, checkmarkDe,
                checkmarkJp, checkmarkZh, checkmarkKo, checkmarkRu, checkmarkEs
            };
            
            SelectLanguage(GetAbbrFromSysLanguage());
            ShowRam();
            ShowVRam();
            SuggestProfile();

            _sceneLoad = SceneManager.LoadSceneAsync("Loader");
            _sceneLoad.allowSceneActivation = false;
        }

        private static string GetAbbrFromSysLanguage()
        {
            return Application.systemLanguage switch
            {
                SystemLanguage.English => "en",
                SystemLanguage.Spanish => "es",
                SystemLanguage.ChineseSimplified => "zh",
                SystemLanguage.French => "fr",
                SystemLanguage.German => "de",
                SystemLanguage.Russian => "ru",
                SystemLanguage.Japanese => "jp",
                SystemLanguage.Portuguese => "pt",
                _ => "en"
            };
        }

        public void ChooseGraphicProfile(int index)
        {
            PlayerPrefs.SetInt("QualityLevel", index);
            PlayerPrefs.SetString("SceneToLoad", "Advise");
            _sceneLoad.allowSceneActivation = true;
        }

        private void SuggestProfile()
        {
            var currentSystemVRam = SystemInfo.graphicsMemorySize / 1000;
            var currentSystemRam = SystemInfo.systemMemorySize / 1000;

            
                if (currentSystemRam > MediumLevelRequirementRam  && currentSystemVRam > MediumLevelRequirementVRam)
                {
                    if (currentSystemRam > HighLevelRequirementRam && currentSystemVRam > HighLevelRequirementVRam)
                    {
                        highLevelRecommended.SetActive(true);
                    }
                    else
                    {
                        mediumLevelRecommended.SetActive(true);
                    }
            }
            else
            {
                mediumLevelRecommended.SetActive(true);
            }
        }

        private void ShowVRam()
        {
            var currentSystemVRam = SystemInfo.graphicsMemorySize / 1000;
            
            foreach (var currentVRamText in currentVRamTexts)
            {
                currentVRamText.text = $"{LocalizationManager.Instance.GetContent("148")}: {currentSystemVRam}gb";
            }
            
            mediumLevelVRamSlider.maxValue = MediumLevelRequirementVRam;
            highLevelVRamSlider.maxValue = HighLevelRequirementVRam;

            mediumLevelVRamSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = currentSystemVRam > MediumLevelRequirementVRam ? Color.green : Color.red;
            highLevelVRamSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = currentSystemVRam > HighLevelRequirementVRam ? Color.green : Color.red;
            
            mediumLevelVRamSlider.value = currentSystemVRam;
            highLevelVRamSlider.value = currentSystemVRam;
        }

        private void ShowRam()
        {
            var currentSystemRam = SystemInfo.systemMemorySize / 1000;

            foreach (var currentRamText in currentRamTexts)
            {
                currentRamText.text = $"{LocalizationManager.Instance.GetContent("150")}: {currentSystemRam}gb";
            }
            
            mediumLevelRamSlider.maxValue = MediumLevelRequirementRam;
            highLevelRamSlider.maxValue = HighLevelRequirementRam;

            mediumLevelRamSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = currentSystemRam > MediumLevelRequirementRam ? Color.green : Color.red;
            highLevelRamSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = currentSystemRam > HighLevelRequirementRam ? Color.green : Color.red;
            
            mediumLevelRamSlider.value = currentSystemRam;
            highLevelRamSlider.value = currentSystemRam;
        }
    }
}