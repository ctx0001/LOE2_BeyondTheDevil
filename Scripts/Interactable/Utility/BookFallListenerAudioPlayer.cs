using System;
using UnityEngine;

namespace Interactable.Utility
{
    public class BookFallListenerAudioPlayer : MonoBehaviour
    {
        [SerializeField] private GameObject onCollisionTrack;
        private float _canCollide;
        private const float IntervalInTime = 0.5f;

        private void OnCollisionEnter()
        {
            if (Time.time > 1 && Time.time > _canCollide)
            {
                _canCollide = Time.time + IntervalInTime;
                Instantiate(onCollisionTrack, transform.position, Quaternion.identity);
            }
        }
    }
}