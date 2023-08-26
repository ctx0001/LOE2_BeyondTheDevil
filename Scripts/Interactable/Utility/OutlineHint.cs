using QuickOutline.Scripts;
using UnityEngine;

namespace Interactable.Utility
{
    public class OutlineHint : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Interactable"))
            {
                var outline = other.GetComponent<Outline>();
                
                if(outline != null)
                    outline.enabled = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Interactable"))
            {
                var outline = other.GetComponent<Outline>();
                
                if(outline != null)
                    outline.enabled = false;
            }
        }
    }
}