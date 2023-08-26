using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Enemy
{
    public class WaypointGroup : MonoBehaviour
    {
        [SerializeField] public List<Transform> waypoints;
        private Transform _currentNode;
        private Transform _nextNode;
        
        /*private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.50f, 0.30f, 0.50f, 1f);

            if (waypoints == null || waypoints.Count == 0)
                return;
            
            for (var i = 0; i < waypoints.Count - 1; i++)
            {
                _currentNode = waypoints[i];
                _nextNode = waypoints[i+1];
                Gizmos.DrawLine(_currentNode.position, _nextNode.position);    
            }

            for (var i = 0; i < waypoints.Count; i++)
            {
                _currentNode = waypoints[i];
                Handles.Label(_currentNode.position, i.ToString());
            }
        }*/
    }
    
}