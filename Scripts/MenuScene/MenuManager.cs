using System.Collections;
using MenuScene;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class MenuManager : MonoBehaviour
    {
        [Header("GUI Elements")] 
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject exitPanel;
        [SerializeField] private GameObject playPanel;
        [SerializeField] private TextMeshProUGUI versionText;
        
        private AsyncOperation _sceneLoad;
        private bool _isLoaded;
        public static MenuManager Instance;
        
        private void Awake() 
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                versionText.text = "v" + Application.version;
            } 
        }

        public void CloseAll()
        {
            exitPanel.SetActive(false);
            mainPanel.SetActive(false);
            playPanel.SetActive(false);
        }
        
        public void OpenMainPanel()
        {
            mainPanel.SetActive(true);
        }

        public void CloseMainPanel()
        {
            mainPanel.SetActive(false);
        }

        public void RedirectToRatePage()
        {
            Application.OpenURL("https://ctx1000.itch.io/legacy-of-evil-beyond-the-devil/rate");
        }

        public void OpenExitPanel()
        {
            exitPanel.SetActive(true);
            CloseMainPanel();
        }

        public void CloseExitPanel()
        {
            exitPanel.SetActive(false);
            OpenMainPanel();
        }
        
        public void OpenPlayPanel()
        {
            playPanel.SetActive(true);
            CloseMainPanel();
        }

        public void ClosePlayPanel()
        {
            playPanel.SetActive(false);
            OpenMainPanel();
        }

        public void ExitFromGame()
        {
            Application.Quit();
        }

        public void OpenCredits()
        {
            GameManager.Instance.LoadLoaderWithReference("Credits");
        }
    }
}