using GameScene.Data.Handlers;
using UnityEngine;

namespace GameScene.Player
{
    public class Torch : MonoBehaviour
    {
        [SerializeField] private GameObject spotLight;
        [SerializeField] private GameObject switchSFX;
        public static Torch Instance;
        private bool _charged = true;

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

        private void PlayAudioEffect()
        {
            var audioEffect = Instantiate(switchSFX, transform.position, Quaternion.identity);
            Destroy(audioEffect, 3f);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F) && _charged && InventoryDataHandler.Instance.CheckIfItemIsInHand("torch"))
            {
                PlayAudioEffect();
                spotLight.SetActive(!spotLight.activeSelf);
            }
        }

        public void EnableTorch()
        {
            spotLight.SetActive(true);
        }

        public void DisableTorch()
        {
            spotLight.SetActive(false);
        }
    }
}