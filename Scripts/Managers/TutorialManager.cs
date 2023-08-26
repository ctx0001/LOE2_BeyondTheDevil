using System.Collections;
using UnityEngine;

namespace Managers
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private GameObject tutorialInfoWindow;

        public void ToggleOffTutorialWindow()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
            tutorialInfoWindow.SetActive(false);
        }

        private IEnumerator ToggleOnTutorialWindow()
        {
            yield return new WaitForSeconds(6f);
            tutorialInfoWindow.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            PlayerPrefs.SetString("TutorialDone", "done");
        }

        private void Start()
        {
            if (PlayerPrefs.HasKey("TutorialDone"))
            {
                if(PlayerPrefs.GetString("TutorialDone") != "done")
                {
                    StartCoroutine(ToggleOnTutorialWindow());
                }
            }
            else
            {
                StartCoroutine(ToggleOnTutorialWindow());
            }
        }

    }
}