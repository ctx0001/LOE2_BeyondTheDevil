using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Managers
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textAlert;

        [Header("Cutscenes")] 
        [SerializeField] private PlayableDirector alertCutscene;

        [Header("Quests")] 
        [SerializeField] private TextMeshProUGUI questDescriptionText;
        [SerializeField] private TextMeshProUGUI questHintText;
        
        [Header("Audio")] 
        [SerializeField] private AudioClip alertSound;
        [SerializeField] private TextMeshProUGUI balanceText;

        [SerializeField] private TextMeshProUGUI interactionText;
        public static CanvasManager Instance;

        [SerializeField] private Slider staminaBar;

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

        public void UpdateResistence(float value, float max)
        {
            staminaBar.value = value;
            staminaBar.maxValue = max;
        }
        
        /**
         * <summary>Given a string of information, shown to the user
         * for 8 seconds the string as an alert in the upper right
         * corner displaying an animation</summary>
         * <param name="text">The string shown in the alert</param>
         */
        private IEnumerator CreateNewAlert(string text)
        {
            textAlert.text = text;
            AudioSource.PlayClipAtPoint(alertSound, transform.position);
            alertCutscene.Play();
            yield return new WaitForSeconds((float)alertCutscene.duration);
            alertCutscene.Stop();
        }

        public void UpdateBalance(int balance)
        {
            balanceText.text = "$" + balance;
        }
        
        public void EnableInteractionText(string text)
        {
            interactionText.text = text;
            interactionText.gameObject.SetActive(true);  
        } 
        public void DisableInteractionText() => interactionText.gameObject.SetActive(false);

        public void UpdateQuest(string questDescription, string questHint)
        {
            questDescriptionText.text = questDescription;
            questHintText.text = questHint;
        }
        
    }
}