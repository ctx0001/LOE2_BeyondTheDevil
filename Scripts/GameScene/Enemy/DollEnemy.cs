using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemy;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GameScene.Enemy
{
    public class DollEnemy : MonoBehaviour
    {
        [SerializeField] private WaypointGroup waypointGroup;

        private List<Transform> _waypoints = new List<Transform>();
        private Transform _currentTarget;
        private int _index;

        [Header("References")]
        [SerializeField] private UnityEngine.AI.NavMeshAgent agent;
        [SerializeField] private Animator animator;
        private Transform player;
        [SerializeField] private Camera enemyCamera;
        [SerializeField] private GameObject deadCutscene;
        
        [Header("Settings")] 
        [SerializeField] private float walkSpeed = 1f;
        [SerializeField] private LayerMask eyeMask;
        
        private bool _inReverse;
        private bool _atEnd;
        private bool _moving = true;
        private bool _alerted;
        private bool _alive = true;

        [Header("Audio")] 
        [SerializeField] private GameObject[] messages;
        [SerializeField] private GameObject dollSawPlayerSource;

        private bool _followingPlayer;
        private Vector3 _lastKnownPosition;
        private bool _playerTryngEscape;
        private bool _checkingLastPosition;
        private bool _canTalk = true;
        
        private static readonly int Walking = Animator.StringToHash("Walking");
        private static readonly int Running = Animator.StringToHash("Running");
        
        [SerializeField] private AudioClip[] footstepSounds;
        [SerializeField] private AudioSource footstepSource;

        private IEnumerator PlaySteps()
        {
            while (_alive)
            {
                if (_moving || _alerted)
                {
                    if (Math.Abs(agent.speed - walkSpeed) < 0.1f)
                    {
                        yield return new WaitForSeconds(Random.Range(0.7f, 1f));
                    }
                    else
                    {
                        yield return new WaitForSeconds(Random.Range(0.3f, 0.5f));
                    }   
                    PlayFootstepSound();   
                }
                else
                {
                    yield return new WaitForSeconds(1f);
                }
            }
        }
        
        private void PlayFootstepSound()
        {
            if (footstepSounds.Length > 0)
            {
                var randomIndex = Random.Range(0, footstepSounds.Length);
                var footstepClip = footstepSounds[randomIndex];
                footstepSource.PlayOneShot(footstepClip);
            }
        }
        
        private void Start()
        {
            // Gathering References
            player = GameObject.Find("TargetRef").GetComponent<Transform>();
            waypointGroup = GameObject.Find("WaypointGroup").GetComponent<WaypointGroup>();
            
            // Initializing waypoints
            _waypoints = waypointGroup.waypoints;
            
            // Checks if waypoints > 0 and != null
            if (_waypoints.Count <= 0 || _waypoints[0] == null) return;
            _currentTarget = _waypoints[_index];
            agent.SetDestination(_currentTarget.position);
            
            // Starting basics Routines
            StartCoroutine(TalkRoutine());
            StartCoroutine(CheckForPlayerRoutine());
            StartCoroutine(PlaySteps());
            
            // Reproducing creepy signing
            if (messages.Length > 0)
            {
                var sound = Instantiate(messages[0], transform.position, Quaternion.identity);
                sound.transform.parent = transform;
                Destroy(sound, sound.GetComponent<AudioSource>().clip.length);     
            }
        }

        private IEnumerator OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player")) yield break;
            
            while (!_alerted)
            {
                var rayDirection = player.transform.position - transform.position;
                if (Physics.Raycast(transform.position, rayDirection, out var hit, 25f, eyeMask))
                {
                    // if there are no obstacle
                    if (hit.transform.CompareTag("Player"))
                    {
                        // alert the enemy, change the speed, then call FollowPlayerRoutine()
                        _alerted = true;
                        agent.speed = 3f; // run speed
                        Instantiate(dollSawPlayerSource, player.transform.position, Quaternion.identity);
                        StartCoroutine(FollowPlayerRoutine());
                    }
                }   
                yield return new WaitForSeconds(0.05f);
            }
        }

        /**
         * <summary>Independent routine, this reproduce a random sound every
         * 30-100 seconds.</summary>
         */
        private IEnumerator TalkRoutine()
        {
            while(_alive && messages.Length > 0)
            {
                yield return new WaitForSeconds(Random.Range(30, 100));
                if (_canTalk)
                {
                    var random = Random.Range(0, messages.Length);
                    var sound = Instantiate(messages[random], transform.position, Quaternion.identity);
                    sound.transform.parent = transform;
                    Destroy(sound, sound.GetComponent<AudioSource>().clip.length);   
                }
            }
        }

        public void SetTalkState(bool state)
        {
            _canTalk = state;
        }

        /**
         * <summary>Uses camera component to see if player is in view.</summary>
         * <returns>Whether is visible or not</returns>
         */
        private bool PlayerInView()
        {
            var planes = GeometryUtility.CalculateFrustumPlanes(enemyCamera);
            var point = player.transform.position;
            return planes.All(plane => !(plane.GetDistanceToPoint(point) < 0));
        }

        /**
         * <summary>Basic locomotion routine</summary>
         */
        private void MoveToNextWayPoint()
        {
            if (!_inReverse)
                _index++;
            
            if(_index < _waypoints.Count && !_inReverse)
            {
                /*if (_index == 1)
                    yield return new WaitForSeconds(Random.Range(3f, 6f));*/
                _currentTarget = _waypoints[_index];
            }
            else
            {
                if (!_atEnd)
                {
                    _atEnd = true;
                    //yield return new WaitForSeconds(Random.Range(3f, 6f));
                }

                _index--;
                _inReverse = true;

                if (_index == 0)
                {
                    _inReverse = false;
                    _atEnd = false;
                }

                _currentTarget = _waypoints[_index];
            }
            
            agent.SetDestination(_currentTarget.position);   
            _moving = true;
            agent.speed = walkSpeed;
        }

        private IEnumerator CheckForPlayerRoutine()
        {
            while (!_alerted)
            {
                // Player in camera bounds
                if (PlayerInView())
                {
                    // Calculating a raycast from this agent, to the player, to check if there are obstacle
                    var rayDirection = player.transform.position - transform.position;
                    if (Physics.Raycast(transform.position, rayDirection, out var hit, 25f, eyeMask))
                    {
                        // if there are no obstacle
                        if (hit.transform.CompareTag("Player"))
                        {
                            // alert the enemy, change the speed, then call FollowPlayerRoutine()
                            _alerted = true;
                            agent.speed = 3f; // run speed
                            Instantiate(dollSawPlayerSource, player.transform.position, Quaternion.identity);
                            StartCoroutine(FollowPlayerRoutine());
                        }
                    }
                }
                yield return new WaitForSeconds(0.05f);
            }
        }
        
        private IEnumerator FollowPlayerRoutine()
        {
            while (_alive && _alerted)
            {
                _lastKnownPosition = player.transform.position;
                agent.SetDestination(_lastKnownPosition);
                
                // if enemy distance from player less than 2 meters, then trigger die
                if (Vector3.Distance(transform.position, player.transform.position) <= 2f)
                {
                    //_ambienceMusicManager.EnableNormalAmbience();
                    GameObject.FindWithTag("Player").GetComponent<Movement>().Die();
                    _alive = false;
                    Instantiate(deadCutscene, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                
                if (!PlayerInView() && !_playerTryngEscape)
                {
                    StartCoroutine(MakePlayerEscape());
                }
                
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        private IEnumerator MakePlayerEscape()
        {
            _playerTryngEscape = true;
            const float timeToSeed = 3.5f;
            var currentTime = 0f;
            
            while (currentTime < timeToSeed && !PlayerInView())
            {
                yield return new WaitForSeconds(0.1f);
                currentTime += 0.1f;
            }

            if (currentTime > timeToSeed)
            {
                _alerted = false;
                StartCoroutine(GoCheckLastKnownPosition());
            }
            
            _playerTryngEscape = false;
        }
        
        private IEnumerator GoCheckLastKnownPosition()
        {
            _checkingLastPosition = true;
            agent.speed = 1f; // walk speed
            animator.SetBool(Running, false);
            animator.SetBool(Walking, true);
            agent.SetDestination(_lastKnownPosition);

            var distance = Vector3.Distance(transform.position, _lastKnownPosition);
            while (distance >= 2f)
            {
                distance = Vector3.Distance(transform.position, _lastKnownPosition);
                yield return new WaitForSeconds(0.1f);
            }
                        
            var currentTime = 0f;
            const float checkTime = 6f;
            
            while (currentTime < checkTime)
            {
                animator.SetBool(Walking, false);
                animator.SetBool(Running, false);

                //headReference.Rot = Vector3.Lerp(Vector3.zero, new Vector3(60, 0, 0), 2f);
                
                if (PlayerInView())
                {
                    // Calculating a raycast from this agent, to the player, to check if there are obstacle
                    var rayDirection = player.transform.position - transform.position;
                    if (Physics.Raycast(transform.position, rayDirection, out var hit, 25f, eyeMask))
                    {
                        // if there are no obstacle
                        if (hit.transform.CompareTag("Player") && !_alerted)
                        {
                            _alerted = true;
                            agent.speed = 3f; // run speed
                            animator.SetBool(Walking, false);
                            animator.SetBool(Running, true);
                            StartCoroutine(FollowPlayerRoutine());
                        }
                    }
                }

                currentTime += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
            
            // player seed doll
            //_ambienceMusicManager.EnableNormalAmbience();
            _checkingLastPosition = false;
            MoveToNextWayPoint();
        }
        
        private void Update()
        {
            if (!_checkingLastPosition)
            {
                animator.SetBool(Walking, _moving);
                animator.SetBool(Running, _alerted);   
            }

            if (_alerted)
            {
                _moving = false;
                return;   
            }

            if (Vector3.Distance(transform.position, _currentTarget.position) <= 1.5f && _moving)
            {
                _moving = false;
                MoveToNextWayPoint();   
            }
        }
    }
}