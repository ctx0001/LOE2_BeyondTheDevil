using UnityEngine;

namespace Managers
{
    public class PanelCloser : MonoBehaviour
    {
        [SerializeField] private GameObject panel;

        public void ClosePanel()
        {
            panel.SetActive(false);
        }
    }
}