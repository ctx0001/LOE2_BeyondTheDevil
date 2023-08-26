using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private List<GameObject> archPrefab;
        [SerializeField] private int index;
        [SerializeField] private float speed = 1.5f;

        [SerializeField] private Vector3 startPos;
        [SerializeField] private float distance = 4.236f;
        [SerializeField] private GameObject entranceDoor;

        private Vector3 _entranceDoorPosition;
        private List<GameObject> _archContainer = new List<GameObject>();
        
        private bool _scenePlayed;
        private bool _firstAnimationTime = true;
        private PlayableDirector _leftDoorCutscene;
        private PlayableDirector _rightDoorCutscene;

        public void InstantiateEntranceDoor()
        {
            speed = 10f;
            _scenePlayed = true;
            startPos = new Vector3(startPos.x, startPos.y, startPos.z + distance);
            var entrance = Instantiate(entranceDoor, startPos, Quaternion.identity * Quaternion.Euler (0f, 180f, 0f));
            _entranceDoorPosition = entrance.transform.position;
            _leftDoorCutscene = GameObject.Find("LeftDoor").GetComponent<PlayableDirector>();
            _rightDoorCutscene = GameObject.Find("RightDoor").GetComponent<PlayableDirector>();
        }
        
        private void CreateNewArch()
        {
            startPos = new Vector3(startPos.x, startPos.y, startPos.z + distance);
            var arch = Instantiate(archPrefab[index], startPos, Quaternion.identity);
            _archContainer.Add(arch);
            index++;

            if (index >= archPrefab.Count)
                index = 0;
            
            foreach (var archTransform in _archContainer.ToList())
            {
                if (!(archTransform.transform.position.z < mainCamera.transform.position.z)) continue;
                _archContainer.Remove(archTransform);
                Destroy(archTransform);
            }
        }

        private void Update()
        {
            if (!_scenePlayed)
            {
                mainCamera.transform.position += transform.forward * (speed * Time.deltaTime);
            }
            else
            {
                if (mainCamera.transform.position.z < (_entranceDoorPosition.z - 5f))
                {
                    mainCamera.transform.position += transform.forward * (speed * Time.deltaTime);
                }
                else if (_firstAnimationTime)
                {
                    _rightDoorCutscene.Play();
                    _leftDoorCutscene.Play();
                    _firstAnimationTime = false;
                }
            }

            if (mainCamera.transform.position.z + 35 > startPos.z && !_scenePlayed)
            {
                CreateNewArch();
            }
        }
    }
}