using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class DeathCutsceneShake : MonoBehaviour
    {

        [SerializeField] private Camera camera;
        float shakeAmount = 0.1f; 
        float shakeSpeed = 50f;

        Vector3 GetShakeVector(float time) {
            float x = Mathf.Sin(time * shakeSpeed) * shakeAmount;
            float y = Mathf.Cos(time * shakeSpeed * 2) * shakeAmount * 0.5f;
            return new Vector3(x, y, 0f);
        }

        private IEnumerator ShakeRoutine()
        {
            var time = 0f;
            while (time < 0.95f)
            {
                yield return new WaitForSeconds(0.01f);
                
                if(time > 0.81f && time < 0.95f)
                    camera.transform.position = GetShakeVector(time);
            }
        }
    }
}