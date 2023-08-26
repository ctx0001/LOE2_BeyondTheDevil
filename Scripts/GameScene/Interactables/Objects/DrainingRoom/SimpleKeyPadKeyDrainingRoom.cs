using GameScene.Interactables.SecurityKeyPad;
using UnityEngine;

namespace Interactable.SecurityKeyPad
{
    public class SimpleKeyPadKeyDrainingRoom : MonoBehaviour
    {
        [SerializeField] private KeyPadDrainingRoom keyPad;
        [SerializeField] private GameObject keyPadAudio;

        public void Interact()
        {
            keyPad.AddNumber(gameObject.name);
            Instantiate(keyPadAudio, transform.position, Quaternion.identity);
        }
    }
}