using System.Collections;
using UnityEngine;

namespace Managers.Menu
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private float shakeDuration = 0.5f;
        [SerializeField][Range(0f, 1f)] private float shakeAmount = 0.1f;
        [SerializeField][Range(1f, 10f)] private float shakeRotationAmount = 1f;
        [SerializeField][Range(5f, 20f)] private float shakeSpeed = 5f;
        
        public float GetDuration()
        {
            return shakeDuration;
        }

        public IEnumerator Shake()
        {
            var transform1 = transform;
            var originalPosition = transform1.localPosition;
            var originalRotation = transform1.localRotation;
            var elapsed = 0f;

            while (elapsed < shakeDuration)
            {
                var x = originalPosition.x + Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) * shakeAmount * 2f - shakeAmount;
                var y = originalPosition.y + Mathf.PerlinNoise(0f, Time.time * shakeSpeed) * shakeAmount * 2f - shakeAmount;
                var z = originalPosition.z;

                transform.localPosition = new Vector3(x, y, z);

                var pitch = originalRotation.eulerAngles.x + Mathf.PerlinNoise(0f, Time.time * shakeSpeed) * shakeRotationAmount * 2f - shakeRotationAmount;
                var yaw = originalRotation.eulerAngles.y + Mathf.PerlinNoise(Time.time * shakeSpeed, Time.time * shakeSpeed) * shakeRotationAmount * 2f - shakeRotationAmount;
                var roll = originalRotation.eulerAngles.z;

                var rotation = Quaternion.Euler(pitch, yaw, roll);
                transform.localRotation = rotation;

                elapsed += Time.deltaTime;
                yield return null;
            }

            var transform2 = transform;
            transform2.localPosition = originalPosition;
            transform2.localRotation = originalRotation;
        }
    }
}