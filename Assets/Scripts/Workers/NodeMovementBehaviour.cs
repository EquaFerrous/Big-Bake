using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NodeMovementBehaviour : MonoBehaviour
{
    // --------- VARIABLES ---------

    [Header("Movement Settings")]
    [SerializeField][Min(0)] private float movementSpeed;

    private static readonly float MOVEMENT_ACCURACY = 0.01f;

    public event Action<MovementNode> OnMovementCompleted;

    private readonly Queue<MovementNode> _movementQueue = new();
    private MovementNode _currentTarget;


    public bool IsMoving => _currentTarget != null;

    private bool IsInPrecisionOfTarget => (_currentTarget.transform.position - transform.position).magnitude <= MOVEMENT_ACCURACY;

    // ------ DEFAULT METHODS ------

    private void Update()
    {
        if (!IsMoving)
        {
            if (_movementQueue.Count > 0)
            {
                StartNewMovement();
            }
        }
        else
        {
            MoveTowardsTarget();
        }
    }

    // ------ PUBLIC METHODS -------

    /// <summary>
    /// Adds <paramref name="destinationNode"/> to the movement queue.
    /// </summary>
    /// <param name="destinationNode">The destination node to path to.</param>
    public void AddToMovementQueue(DestinationNode destinationNode)
    {
        _movementQueue.Enqueue(destinationNode);
    }
    
    // ------ PRIVATE METHODS ------
    
    private void StartNewMovement()
    {
        _currentTarget = _movementQueue.Dequeue();
    }

    private void MoveTowardsTarget()
    {
        if (IsInPrecisionOfTarget)
        {
            FinishCurrentMovement();
            return;
        }

        Vector3 targetDirection = (_currentTarget.transform.position - transform.position).normalized;
        Vector3 newPosition = transform.position + (movementSpeed * Time.deltaTime * targetDirection);
        transform.position = newPosition;
    }

    private void FinishCurrentMovement()
    {
        transform.position = _currentTarget.transform.position;
        OnMovementCompleted?.Invoke(_currentTarget);
        _currentTarget = null;
    }
    
}
