using UnityEngine;

namespace Jumpscares
{
    public class HallJumpscareController : MonoBehaviour
    {
        [SerializeField] private GameObject JumpScare;

        public static HallJumpscareController Instance;

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

        public void EnableJumpscare()
        {
            JumpScare.SetActive(true);
        }
    }
}