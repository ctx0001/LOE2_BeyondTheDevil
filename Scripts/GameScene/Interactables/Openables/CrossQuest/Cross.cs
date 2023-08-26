using System.Collections;
using Common.Localization.Speaking;
using GameScene.Data.Handlers;
using Localization;
using UnityEngine;

namespace GameScene.Interactables.Openables.CrossQuest
{
    public class Cross : Openable
    {
        [SerializeField] private GameObject rotationSound;
        [SerializeField] private CrossController crossController;
        
        private bool _isOn;
        private AudioClip _objectNotUseful;
        private bool _busy;

        private int facing = 0;

        private void Start()
        {
            _objectNotUseful = Resources.Load<AudioClip>("Audio/object-not-useful");
            _objectNotUseful.LoadAudioData();
        }

        protected override void Interact()
        {
            if (AssignmentsDataHandler.Instance.Exists(20) && !_busy)
            {
                StartCoroutine(RotateObject());
                Instantiate(rotationSound, transform.position, Quaternion.identity);
            }
            else if(!AssignmentsDataHandler.Instance.Exists(19))
            {
                SpeakManager.Instance.SpeakSingle(LocalizationManager.Instance.GetContent("117"), _objectNotUseful, 0.3f);
            }
        }
        
        private IEnumerator RotateObject()
        {
            facing++;
            _busy = true;
            var rotation = transform.rotation;
            var targetRotation = rotation * Quaternion.AngleAxis(90f, Vector3.right);
            const float duration = 1.0f;
            var time = 0f;

            while (time < duration)
            {
                var t = time / duration;
                Transform transform1;
                (transform1 = transform).rotation = Quaternion.Lerp(rotation, targetRotation, t);

                var angleX = transform1.rotation.eulerAngles.x;
                switch (angleX)
                {
                    case < -90f:
                        angleX += 360f;
                        break;
                    case > 180f:
                        angleX -= 360f;
                        break;
                }

                var rotation1 = transform.rotation;
                rotation1 = Quaternion.Euler(angleX, rotation1.eulerAngles.y, rotation1.eulerAngles.z);
                transform.rotation = rotation1;

                time += Time.deltaTime;
                yield return null;
            }

            transform.rotation = targetRotation;
            _busy = false;

            if (facing == 4)
                facing = 0;

            crossController.CheckCrossRotation();
        }

        public int GetFacing()
        {
            return facing;
        }

    }
}