using TMPro;
using UnityEngine;

namespace GameScene.Pc
{
    public class ComputerImage : MonoBehaviour
    {
        [SerializeField] private string filename;
        [SerializeField] private TextMeshProUGUI fileName;
        [SerializeField] private Sprite image;
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

        public void SetImage(Sprite sprite)
        {
            image = sprite;
        }

        public void Initialize()
        {
            fileName.text = filename + ".jpg";
        }

        public void ViewContent()
        {
            _desktopHandler.ViewImagePreview(filename, image);
        }
    }
}