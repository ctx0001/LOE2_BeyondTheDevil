using UnityEngine;

namespace Enemy
{
    public class Waypoint : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.30f, 0.20f, 1f, 0.50f);
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }
}