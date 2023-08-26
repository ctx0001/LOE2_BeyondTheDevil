using System;
using UnityEngine;

namespace Player
{
    public class HeadBobber : MonoBehaviour
    {
        [SerializeField] private float walkingBobbingSpeed = 5.6f;
        [SerializeField] private float runningBobbingSpeed = 11.2f;
        [SerializeField] private float bobbingAmount = 0.05f;
        [SerializeField] private Movement playerMovementManager;
        
        private float _defaultPosY;
        private float _defaultPosX;
        private float _timer;
        
        private void Start()
        {
            _defaultPosY = transform.localPosition.y;
        }
        
        private void Update()
        {
            if(playerMovementManager.GetVelocity() != Vector3.zero)
            {
                if (Math.Abs(playerMovementManager.GetSpeed() - 1.3) < 0.1)
                {
                    _timer += Time.deltaTime * walkingBobbingSpeed;
                    var localPosition = transform.localPosition;
                    localPosition = new Vector3(localPosition.x, _defaultPosY + Mathf.Sin(_timer) * bobbingAmount, localPosition.z);
                    transform.localPosition = localPosition;
                }
                else if (Math.Abs(playerMovementManager.GetSpeed() - 2.4) < 0.1 && Time.timeScale != 0)
                {
                    _timer += Time.deltaTime * runningBobbingSpeed;
                    var localPosition = transform.localPosition;
                    localPosition = new Vector3(localPosition.x, _defaultPosY + Mathf.Sin(_timer) * bobbingAmount, localPosition.z);
                    transform.localPosition = localPosition;
                }
            }
            else
            {
                _timer = 0;
                var transform1 = transform.localPosition;
                transform.localPosition = new Vector3(transform1.x, Mathf.Lerp(transform1.y,
                    _defaultPosY, Time.deltaTime * walkingBobbingSpeed), transform1.z);
            }
        }
    }
}