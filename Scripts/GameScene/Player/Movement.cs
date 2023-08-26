using System;
using System.Collections;
using Managers;
using Player;
using SimpleFPSController;
using UnityEngine;
using UnityEngine.Serialization;

public class Movement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speedOnWalk = 2.5f;
    [SerializeField] private float speedOnRun = 4.5f;
    [SerializeField] private Visual visual;

    [Header("Audio")] 
    [SerializeField] private GameObject walkAudio;
    [SerializeField] private GameObject runAudio;

    [FormerlySerializedAs("_pauseManager")] [SerializeField] private PauseManager pauseManager;
    

    private float _speed;
    private bool _canMove = true;
    private bool _running;
    private int _maxResistence = 160;
    private int _resistence;
    private bool _isAlive = true;
    private bool _restoring;
    
    private const float Tolerance = 0.1f;
    private Vector3 _velocity;
    private float _gravity = 9.8f;


    private void Start()
    {
        _resistence = _maxResistence;
        StartCoroutine(CheckResistence());
        visual.Init(transform, visual.transform);
    }

    public void Die()
    {
        _isAlive = false;
        //canvasManager.gameObject.SetActive(false);
        walkAudio.SetActive(false);
        runAudio.SetActive(false);
        //playerDataManager.SetDeadByDollStatus(true);
        pauseManager.ReloadScene(10f);
        Destroy(gameObject);
    }

    private IEnumerator CheckResistence()
    {
        while (_isAlive)
        {
            if (_running && _resistence > 0)
            {
                _resistence -= 1;
                CanvasManager.Instance.UpdateResistence(_resistence, _maxResistence);
            }
            else if(!_running && !_restoring)
            {
                    
                StartCoroutine(RestoreResistenceRoutine());
                    
            }

            yield return new WaitForSeconds(0.1f);   
        }
    }

    private IEnumerator RestoreResistenceRoutine()
    {
        _restoring = true;
        yield return new WaitForSeconds(3f);
        while (_resistence < _maxResistence && !_running)
        {
            _resistence += 1;
            CanvasManager.Instance.UpdateResistence(_resistence, _maxResistence);
            yield return new WaitForSeconds(0.05f);
        }
        _restoring = false;
    }

    public void Update()
    {
        if (_canMove && _isAlive)
        {
            HandleMovement();   
            HandleAudioEffects();
        }

        visual.LookRotation(transform, visual.transform);
    }
    
    private void HandleMovement()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var transform1 = transform;
        var direction = transform1.right * horizontal + transform1.forward * vertical;

        if (Input.GetKey(KeyCode.LeftShift) && _resistence > 0)
        {
            _speed = speedOnRun;
        }
        else
        {
            _speed = speedOnWalk;
        }
        
        _velocity = direction * _speed;
        if(!controller.isGrounded)
            _velocity += _gravity * Vector3.down;

        controller.Move(_velocity * Time.deltaTime);
    }

    private void HandleAudioEffects()
    {
        if (controller.velocity != Vector3.zero && Time.timeScale != 0)
        {
            if (Math.Abs(_speed - speedOnWalk) < 0.1)
            {
                if (!walkAudio.activeSelf) 
                {
                    walkAudio.SetActive(true);
                    runAudio.SetActive(false);
                }
                _running = false;
            }
            else if (Math.Abs(_speed - speedOnRun) < 0.1 && Time.timeScale != 0)
            {
                if (!runAudio.activeSelf) 
                {
                    walkAudio.SetActive(false);
                    runAudio.SetActive(true);
                }
                _running = true;
            }
            controller.Move(_velocity * Time.deltaTime);
        }
        else
        {
            walkAudio.SetActive(false);
            runAudio.SetActive(false);
            _running = false;
        }
    }

    public void SetMoveStatus(bool status)
    {
        _canMove = status;
        if (_canMove) return;
        walkAudio.SetActive(false);
        runAudio.SetActive(false);
        _running = false;
        _velocity = Vector3.zero;
        //animator.SetBool(Walking, false);
    }

    public float GetSpeed()
    {
        return _speed;
    }

    public Vector3 GetCurrentPosition()
    {
        return transform.position;
    }

    public Vector3 GetVelocity()
    {
        return _velocity;
    }
}