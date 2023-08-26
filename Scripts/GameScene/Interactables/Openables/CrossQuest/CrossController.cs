using UnityEngine;

namespace GameScene.Interactables.Openables.CrossQuest
{
    public class CrossController : MonoBehaviour
    {
        [SerializeField] private Cross cross1;
        [SerializeField] private Cross cross2;
        [SerializeField] private Cross cross3;

        [SerializeField] private SecretPassage secretPassage;
        
        public void CheckCrossRotation()
        {
            if (cross1.GetFacing() == 3 && cross2.GetFacing() == 2 && cross3.GetFacing() == 1)
            {
                Debug.Log("UNLOCKING");
                secretPassage.Unlock();
            }
        }
    }
}