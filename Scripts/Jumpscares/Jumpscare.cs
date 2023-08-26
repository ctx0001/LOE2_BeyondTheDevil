using GameScene.Data.Handlers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Jumpscares
{
    public class Jumpscare : MonoBehaviour
    {
        [SerializeField] private int id;
        [FormerlySerializedAs("jumpscareManager")] [SerializeField] private JumpscareDataHandler jumpscareDataHandler;
        [SerializeField] private GameObject trigger;

        public void Disable() => trigger.SetActive(false);
        public int GetId() => id;

        public void UpdateStatus(bool active) => jumpscareDataHandler.UpdateJumpScareStatus(id, active);
        public bool IsActive() => jumpscareDataHandler.SearchJumpScareStatus(id);
    }    
    
}