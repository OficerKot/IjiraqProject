using UnityEngine;
using UnityEngine.InputSystem;
using System;
using VContainer;

public class CharacterMovement : PauseBehaviour
{
    [SerializeField] float walkSpeed = 7f;
    [SerializeField] private bool isMoving = false;
    bool isActive = true, movementIsAlloved = true;
    [SerializeField] public Transform targetPosition;
    public LayerMask whatAllowsMovement;
    public static event Action <bool> OnMovementAttempted;

    private CharacterStates charStates;

    HandManager _handManager;
    SignsManager _signsManager;
    [Inject]
    void Construct(HandManager handManager, SignsManager signsManager)
    {
        _handManager = handManager;
        _signsManager = signsManager;
    }
    private void Awake()
    {
        targetPosition.parent = null;
        charStates = GetComponent<CharacterStates>();
    }
    public override void OnGamePaused(bool isGamePaused)
    {
        isActive = !isGamePaused;
    }
   
    public void OnWalk(InputValue inputValue)
    {
        if (!isActive || isMoving) return;
        Vector2 destination = inputValue.Get<Vector2>();
        if(AvailableDestinaton(destination))
        {
            Clock.Instance.TimeTick();
            Move(destination);
            AudioManager.Play(SoundType.Step);
        }
        
    }
    void Move(Vector2 destination)
    {
        if (movementIsAlloved)
        {
            if(OnMovementAttempted != null) OnMovementAttempted.Invoke(true);
            targetPosition.position += new Vector3(destination.x, destination.y, 0);
            isMoving = true;
        }
        else
        {
            if (OnMovementAttempted != null) OnMovementAttempted.Invoke(false);
            charStates.CheckState();
        }
    }
    public void ControlMovement(bool val)
    {
        movementIsAlloved = val;
    }
    bool AvailableDestinaton(Vector3 destination)
    {
        return Mathf.Abs(destination.x + destination.y) == 1 && !_handManager.WhatInHand() && Physics2D.OverlapCircle(targetPosition.position + destination, .01f, whatAllowsMovement);
    }
    void FixedUpdate()
    {
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition.position, walkSpeed*Time.fixedDeltaTime);
            if (transform.position.x == targetPosition.position.x && transform.position.y == targetPosition.position.y)
            {
                isMoving = false;
                charStates.CheckState();
                _signsManager.CastSignsRays();
            }
        }
    }
}
