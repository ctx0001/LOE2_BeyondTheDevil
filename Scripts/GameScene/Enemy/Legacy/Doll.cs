using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemy;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace GameScene.Enemy.Legacy
{
    
    public class Doll : MonoBehaviour
    {
        [SerializeField] private WaypointGroup waypointGroup;

        private List<Transform> _waypoints = new List<Transform>();
        private Transform _currentTarget;
        private int _index;

        [Header("References")]
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        private Transform player;
        [SerializeField] private Camera enemyCamera;

        [Header("Settings")] 
        [SerializeField] private float walkSpeed = 1f;
        [SerializeField] private LayerMask eyeMask;
        
        [Header("Debug State")]
        [SerializeField] private bool inReverse;
        [SerializeField] private bool atEnd;
        [SerializeField] private bool moving = true;
        [SerializeField] private bool alerted;
        [SerializeField] private bool alive = true;

        [Header("Audio")] 
        [SerializeField] private GameObject[] messages;

        private bool _followingPlayer;

        private static readonly int Walking = Animator.StringToHash("Walking");
        private static readonly int Running = Animator.StringToHash("Running");
        
        [SerializeField] private GameObject evilDollPrefab;

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
            
            // Reproducing creepy signing
            if (messages.Length > 0)
            {
                var sound = Instantiate(messages[0], transform.position, Quaternion.identity);
                sound.transform.parent = transform;
                Destroy(sound, sound.GetComponent<AudioSource>().clip.length);     
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag("Player")) return;
            alive = false;
            // alert the enemy, change the speed, then call FollowPlayerRoutine()
            Instantiate(evilDollPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        /**
         * <summary>Independent routine, this reproduce a random sound every
         * 30-100 seconds.</summary>
         */
        private IEnumerator TalkRoutine()
        {
            while(alive && messages.Length > 0)
            {
                yield return new WaitForSeconds(Random.Range(30, 100));   
                var random = Random.Range(0, messages.Length);
                var sound = Instantiate(messages[random], transform.position, Quaternion.identity);
                sound.transform.parent = transform;
                Destroy(sound, sound.GetComponent<AudioSource>().clip.length);
            }
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
            if (!inReverse)
                _index++;
            
            if(_index < _waypoints.Count && !inReverse)
            {
                /*if (_index == 1)
                    yield return new WaitForSeconds(Random.Range(3f, 6f));*/
                _currentTarget = _waypoints[_index];
            }
            else
            {
                if (!atEnd)
                {
                    atEnd = true;
                    //yield return new WaitForSeconds(Random.Range(3f, 6f));
                }

                _index--;
                inReverse = true;

                if (_index == 0)
                {
                    inReverse = false;
                    atEnd = false;
                }

                _currentTarget = _waypoints[_index];
            }
            
            agent.SetDestination(_currentTarget.position);   
            moving = true;
            agent.speed = walkSpeed;
        }

        private IEnumerator CheckForPlayerRoutine()
        {
            while (alive)
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
                            alive = false;
                            // alert the enemy, change the speed, then call FollowPlayerRoutine()
                            Instantiate(evilDollPrefab, transform.position, Quaternion.identity);
                            Destroy(gameObject);
                        }
                    }
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        private void Update()
        {
            animator.SetBool(Walking, moving);
            animator.SetBool(Running, alerted);

            if (alerted)
            {
                moving = false;
                return;   
            }

            if (Vector3.Distance(transform.position, _currentTarget.position) <= 1.5f && moving)
            {
                Debug.Log("Starting another time");
                moving = false;
                MoveToNextWayPoint();   
            }
        }
    }
}