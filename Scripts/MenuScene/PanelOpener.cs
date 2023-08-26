using UnityEngine;

namespace Managers
{
    public class PanelOpener : MonoBehaviour
    {
        [SerializeField] private GameObject panel;

        public void OpenPanel()
        {
            panel.SetActive(true);
        }
    }
}