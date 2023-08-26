using GameScene.Interactables.SecurityKeyPad;
using UnityEngine;

namespace Interactable.SecurityKeyPad
{
    public class SimpleKeyPadKey : MonoBehaviour
    {
        [SerializeField] private KeyPad keyPad;
        [SerializeField] private GameObject keyPadAudio;

        public void Interact()
        {
            keyPad.AddNumber(gameObject.name);
            Instantiate(keyPadAudio, transform.position, Quaternion.identity);
        }
    }
}