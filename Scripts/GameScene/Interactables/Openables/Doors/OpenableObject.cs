using UnityEngine;

namespace GameScene.Interactables.Openables.Doors
{
    public class OpenableObject : MonoBehaviour
    {
        [SerializeField] private GameObject closed;
        [SerializeField] private GameObject opened;
        [SerializeField] private BoxCollider collider;

        [Header("Audio")]
        [SerializeField] private AudioSource openSound;
        
        private bool _isClosed = true;

        private void Interact()
        {
            if (!_isClosed) return;
            
            Instantiate(openSound, transform.position, Quaternion.identity);
            Destroy(collider);
            closed.SetActive(false);
            opened.SetActive(true);
            _isClosed = false;
        }
        
    }
}