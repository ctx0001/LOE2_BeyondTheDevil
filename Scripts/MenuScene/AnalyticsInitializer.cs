using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

namespace Assets.Scripts.MenuScene
{
    internal class AnalyticsInitializer : MonoBehaviour
    {
        [SerializeField] private GameObject askForDataCollectionConsentWindow;
        
        async void Start()
        {
            await UnityServices.InitializeAsync();
            AskForConsent();
        }

        private void AskForConsent()
        {
            if (PlayerPrefs.HasKey("DataCollectionEnabled"))
            {
                if(PlayerPrefs.GetInt("DataCollectionEnabled") == 1)
                {
                    AnalyticsService.Instance.StartDataCollection();
                }

                return;
            }
            else
            {
                askForDataCollectionConsentWindow.SetActive(true);
            }
        }

        public void OnConsent()
        {
            PlayerPrefs.SetInt("DataCollectionEnabled", 1);
            AnalyticsService.Instance.StartDataCollection();
            askForDataCollectionConsentWindow.SetActive(false);
        }

        public void OnDenied()
        {
            PlayerPrefs.SetInt("DataCollectionEnabled", 0);
            askForDataCollectionConsentWindow.SetActive(false);
        }

        // to implement
        /*public void RequestDataDeletion()
        {
            AnalyticsService.Instance.RequestDataDeletion();
        }*/
    }
}
