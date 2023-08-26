using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace GameScene.Enemy.Legacy
{
    public class DollEvil : MonoBehaviour
    {
        private Transform _currentTarget;
        private int _index;

        [Header("References")]
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] private Camera enemyCamera;
        [SerializeField] private GameObject deadCutscene;

        [Header("Settings")] 
        [SerializeField] private float runSpeed = 3.1f;
        [SerializeField] private LayerMask eyeMask;
        
        [Header("Debug State")]
        [SerializeField] private bool alive = true;

        [SerializeField] private GameObject normalDollPrefab;

        private static readonly int Walking = Animator.StringToHash("Walking");
        private static readonly int Running = Animator.StringToHash("Running");

        //private AmbienceMusicManager _ambienceMusicManager;
        private bool _playerTryngEscape;
        
        private Transform _player;
        private Vector3 _lastKnownPosition;
        private bool _alerted = true;
        
        private void Start()
        {
            // Initializing references
            _player = GameObject.Find("TargetRef").GetComponent<Transform>();
            //_ambienceMusicManager = GameObject.Find("AmbienceManager").GetComponent<AmbienceMusicManager>();
            
            // Setting attributes
            animator.SetBool(Running, true);
            //_ambienceMusicManager.EnableEvilAmbience();
            agent.speed = runSpeed;
            
            // Start basics routines
            StartCoroutine(FollowPlayerRoutine());
        }

        /**
         * <summary>Uses camera component to see if player is in view.</summary>
         * <returns>Whether is visible or not</returns>
         */
        private bool PlayerInView()
        {
            var planes = GeometryUtility.CalculateFrustumPlanes(enemyCamera);
            var point = _player.transform.position;
            return planes.All(plane => !(plane.GetDistanceToPoint(point) < 0));
        }

        /**
         * <summary>
         * This enstablish that the player have to hide for 3 seconds to seed the doll.
         * and checks if this condition is true or not. if true it instantiate the normal doll
         * </summary>
         */
        private IEnumerator MakePlayerEscape()
        {
            _playerTryngEscape = true;
            const float timeToSeed = 3f;
            var currentTime = 0f;
            
            while (currentTime < timeToSeed && !PlayerInView())
            {
                Debug.Log(currentTime);
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
                    var rayDirection = _player.transform.position - transform.position;
                    if (Physics.Raycast(transform.position, rayDirection, out var hit, 25f, eyeMask))
                    {
                        // if there are no obstacle
                        if (hit.transform.CompareTag("Player") && !_alerted)
                        {
                            _alerted = true;
                            agent.speed = runSpeed;
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
            alive = false;
            Instantiate(normalDollPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        /**
         * <summary>While the doll is alive, sets the destination to the player position
         *  and checks if the distance from the player is less than 2 meters, if true player dies.
         *  In addition it calls the MakePlayerEscape() Routine if player is not in view</summary>
         */
        private IEnumerator FollowPlayerRoutine()
        {
            while (alive && _alerted)
            {
                _lastKnownPosition = _player.transform.position;
                agent.SetDestination(_lastKnownPosition);
                
                // if enemy distance from player less than 2 meters, then trigger die
                if (Vector3.Distance(transform.position, _player.transform.position) <= 2f)
                {
                    //_ambienceMusicManager.EnableNormalAmbience();
                    GameObject.FindWithTag("Player").GetComponent<Movement>().Die();
                    alive = false;
                    Instantiate(deadCutscene, transform.position, Quaternion.identity);
                }
                
                if (!PlayerInView() && !_playerTryngEscape)
                {
                    StartCoroutine(MakePlayerEscape());
                }
                
                yield return new WaitForSeconds(0.1f);
            }
        }

    }
}