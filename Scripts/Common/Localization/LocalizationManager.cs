using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using MenuScene.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        private string _language;
        private string _data;

        [SerializeField] private List<TextMeshProUGUI> allTexts = new List<TextMeshProUGUI>();

        [SerializeField] private TMP_FontAsset chiniseFont;
        [SerializeField] private TMP_FontAsset koreanFont;
        [SerializeField] private TMP_FontAsset russianFont;

        [Header("UI Elements")] 
        [SerializeField] private TextMeshProUGUI pauseMenu_Header;
        [SerializeField] private TextMeshProUGUI pauseMenu_Resume;
        [SerializeField] private TextMeshProUGUI pauseMenu_Menu;
        [SerializeField] private TextMeshProUGUI pauseMenu_report;
        [SerializeField] private TextMeshProUGUI pauseMenu_ExitFromGame;
        [SerializeField] private TextMeshProUGUI canvas_InteractionText;

        [Header("UI Elements Menu")] 
        [SerializeField] private TextMeshProUGUI menu_Play;
        [SerializeField] private TextMeshProUGUI menu_Ratings;
        [SerializeField] private TextMeshProUGUI menu_Credits;
        [SerializeField] private TextMeshProUGUI menu_Exit;
        [SerializeField] private TextMeshProUGUI menu_Continue;
        [SerializeField] private TextMeshProUGUI menu_NewGame;
        [SerializeField] private TextMeshProUGUI menu_ExitConfirmPhrase;
        [SerializeField] private TextMeshProUGUI menu_ExitConfirmYes;
        [SerializeField] private TextMeshProUGUI menu_ExitConfirmNo;
        [SerializeField] private TextMeshProUGUI menu_DataOverridingConfirmTitle;
        [SerializeField] private TextMeshProUGUI menu_DataOverridingConfirmMsg;
        [SerializeField] private TextMeshProUGUI menu_DataOverridingConfirmYes;
        [SerializeField] private TextMeshProUGUI menu_DataOverridingConfirmNo;
        [SerializeField] private TextMeshProUGUI menu_MissingDataTitle;
        [SerializeField] private TextMeshProUGUI menu_MissingDataMsg;
        [SerializeField] private TextMeshProUGUI menu_MissingDataOk;

        [SerializeField] private TextMeshProUGUI menu_DataCollectionConsentTitle;
        [SerializeField] private TextMeshProUGUI menu_DataCollectionConsentDescription;
        [SerializeField] private TextMeshProUGUI menu_DataCollectionConsentAccept;
        [SerializeField] private TextMeshProUGUI menu_DataCollectionConsentRefuse;

        [Header("UI Elements Settings")] 
        [SerializeField] private TextMeshProUGUI settings_Header;
        [SerializeField] private TextMeshProUGUI settings_profile2;
        [SerializeField] private TextMeshProUGUI settings_profile3;
        [SerializeField] private TextMeshProUGUI settings_VRam2Hint;
        [SerializeField] private TextMeshProUGUI settings_VRam3Hint;
        [SerializeField] private TextMeshProUGUI settings_Ram2Hint;
        [SerializeField] private TextMeshProUGUI settings_Ram3Hint;
        [SerializeField] private TextMeshProUGUI settings_Reccomended2;
        [SerializeField] private TextMeshProUGUI settings_Reccomended3;
        [SerializeField] private TextMeshProUGUI settings_Choose2;
        [SerializeField] private TextMeshProUGUI settings_Choose3;

        [Header("UI Elements Advise")] 
        [SerializeField] private TextMeshProUGUI advise_header;
        [SerializeField] private TextMeshProUGUI advise_content;

        [Header("UI Elements Credits")]
        [SerializeField] private TextMeshProUGUI credits_Citation;

        [Header("UI Elements Loader")] 
        [SerializeField] private TextMeshProUGUI loadingText_Loader;

        public static LocalizationManager Instance;
        private bool _loaded;

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
            if (!PlayerPrefs.HasKey("Language"))
                PlayerPrefs.SetString("Language", "en");
            LoadData();
        }

        private void CheckLanguage()
        {
            if (PlayerPrefs.GetString("Language") != null)
            {
                _language = PlayerPrefs.GetString("Language");
            }
            else
            {
                Debug.Log("Switching from system language");
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        SettingsManager.Instance.SwitchToLanguage("en");
                        break;
                    case SystemLanguage.Spanish:
                        SettingsManager.Instance.SwitchToLanguage("es");
                        break;
                    case SystemLanguage.ChineseSimplified:
                        SettingsManager.Instance.SwitchToLanguage("zh");
                        break;
                    case SystemLanguage.French:
                        SettingsManager.Instance.SwitchToLanguage("fr");
                        break;
                    case SystemLanguage.German:
                        SettingsManager.Instance.SwitchToLanguage("de");
                        break;
                    case SystemLanguage.Russian:
                        SettingsManager.Instance.SwitchToLanguage("ru");
                        break;
                    case SystemLanguage.Japanese:
                        SettingsManager.Instance.SwitchToLanguage("jp");
                        break;
                    case SystemLanguage.Portuguese:
                        SettingsManager.Instance.SwitchToLanguage("pt");
                        break;
                    default:
                        SettingsManager.Instance.SwitchToLanguage("en");
                        break;
                }
                
                _language = PlayerPrefs.GetString("Language");
            }
        }

        public string GetContent(string id)
        {
            var doc = new XmlDocument();
            doc.Load(new StringReader(_data));
            
            var nodeList = doc.SelectNodes("//text[@id]");
            
            var content = "Error";
            foreach (XmlNode node in nodeList!)
            {
                var attribute = node.Attributes["id"];
                
                if (attribute != null && attribute.Value == id)
                {
                    content = node.InnerText;
                    break;
                }
            }

            content = Regex.Replace(content, @"\t+", " ");
            return content;
        }

        private void TranslateUIElementsInCredits()
        {
            credits_Citation.text = GetContent("130");
        }
        
        private void TranslateUIElemetsInGame()
        {
            // Pause
            pauseMenu_Header.text = GetContent("14");
            pauseMenu_Resume.text = GetContent("15");
            pauseMenu_Menu.text = GetContent("16");
            pauseMenu_report.text = GetContent("241");
            pauseMenu_ExitFromGame.text = GetContent("17");
        }

        private void TranslateUIElementsInLoader()
        {
            loadingText_Loader.text = GetContent("168");
        }

        private void TranslateUIElementsInAdvise()
        {
            advise_header.text = GetContent("153");
            advise_content.text = GetContent("154");
        }

        private void TranslateUIElementsInSettings()
        {
            settings_Header.text = GetContent("144");

            // element with id=145 can be removed from all data language files
            settings_profile2.text = GetContent("146");
            settings_profile3.text = GetContent("147");

            settings_Choose2.text = GetContent("151");
            settings_Choose3.text = GetContent("151");

            settings_Reccomended2.text = GetContent("152");
            settings_Reccomended3.text = GetContent("152");

            settings_VRam2Hint.text = GetContent("149") + "2gb";
            settings_VRam3Hint.text = GetContent("149") + "6gb";
            
            settings_Ram2Hint.text = GetContent("149") + "4gb";
            settings_Ram3Hint.text = GetContent("149") + "8gb";
        }

        private void TranslateUIElementsInMenu()
        {
            menu_Play.text = GetContent("22");
            menu_Ratings.text = GetContent("23");
            menu_Credits.text = GetContent("24");
            menu_Exit.text = GetContent("25");
            menu_Continue.text = GetContent("26");
            menu_NewGame.text = GetContent("27");
            menu_ExitConfirmPhrase.text = GetContent("28");
            menu_ExitConfirmYes.text = GetContent("29");
            menu_ExitConfirmNo.text = GetContent("30");
            menu_DataOverridingConfirmTitle.text = GetContent("31");
            menu_DataOverridingConfirmMsg.text = GetContent("32");
            menu_DataOverridingConfirmYes.text = GetContent("33");
            menu_DataOverridingConfirmNo.text = GetContent("34");
            menu_MissingDataTitle.text = GetContent("35");
            menu_MissingDataMsg.text = GetContent("36");
            menu_MissingDataOk.text = GetContent("37");
            menu_DataCollectionConsentTitle.text = GetContent("242");
            menu_DataCollectionConsentDescription.text = GetContent("243");
            menu_DataCollectionConsentAccept.text = GetContent("244");
            menu_DataCollectionConsentRefuse.text = GetContent("245");
        }

        public string GetCurrentLoadedLanguage()
        {
            return _language;
        }

        public void LoadData()
        {
            CheckLanguage();
            Debug.Log("Loading: " + _language);
            var languageAsset = Resources.Load<TextAsset>("Language/" + _language);
            _data = languageAsset.text;
            
            switch (_language)
            {
                case "zh" or "jp":
                    ChangeFontAssetForChinaJapan();
                    break;
                case "ko": 
                    ChangeFontAssetForKorea();
                    break;
                case "ru":
                    ChangeFontAssetForRussia();
                    break;
            }

            if (SceneManager.GetActiveScene().name == "Game")
            {
                TranslateUIElemetsInGame();
            }else if (SceneManager.GetActiveScene().name == "Menu")
            {
                TranslateUIElementsInMenu();
            }else if (SceneManager.GetActiveScene().name == "Settings")
            {
                TranslateUIElementsInSettings();
            }else if (SceneManager.GetActiveScene().name == "Advise")
            {
                TranslateUIElementsInAdvise();
            }else if (SceneManager.GetActiveScene().name == "Credits")
            {
                TranslateUIElementsInCredits();
            }else if (SceneManager.GetActiveScene().name == "Loader")
            {
                TranslateUIElementsInLoader();
            }
            
            _loaded = true;
        }

        public bool IsLoaded() => _loaded;
        
        private void ChangeFontAssetForChinaJapan()
        {
            foreach (var textComponent in allTexts)
            {
                textComponent.font = chiniseFont;
            }
        }
        
        private void ChangeFontAssetForKorea()
        {
            foreach (var textComponent in allTexts)
            {
                textComponent.font = koreanFont;
            }
        }

        private void ChangeFontAssetForRussia()
        {
            foreach (var textComponent in allTexts)
            {
                textComponent.font = russianFont;
            }
        }
    }
}