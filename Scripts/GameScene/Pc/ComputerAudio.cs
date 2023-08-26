using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene.Pc
{
    public class ComputerAudio : MonoBehaviour
    {
        [SerializeField] private string filename;
        [SerializeField] private TextMeshProUGUI fileName;
        [SerializeField] private AudioClip audioClip;
        private DesktopHandler _desktopHandler;

        private void Start()
        {
            _desktopHandler = GameObject.Find("Desktop").GetComponent<DesktopHandler>();
            Initialize();
        }

        public void SetName(string newName)
        {
            filename = newName;
        }

        public void SetClip(AudioClip clip)
        {
            audioClip = clip;
        }

        public void Initialize()
        {
            fileName.text = filename + ".mp3";
        }

        public void ViewContent()
        {
            _desktopHandler.ViewAudio(filename, audioClip);
        }
    }
}