using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NodeMovementBehaviour : MonoBehaviour
{
    // --------- VARIABLES ---------

    public MovementNode end;

    [Header("Movement Settings")]
    [SerializeField][Min(0)] private float movementSpeed;
    [SerializeField] private MovementNode startNode;

    private static readonly float MOVEMENT_ACCURACY = 0.01f;

    public event Action<MovementNode> OnMovementCompleted;

    private readonly Queue<MovementNode> _movementQueue = new();
    private MovementNode _currentTarget;
    private MovementNode _currentNode;


    public bool IsMoving => _currentTarget != null;

    private bool IsInPrecisionOfTarget => (_currentTarget.transform.position - transform.position).magnitude <= MOVEMENT_ACCURACY;

    // ------ DEFAULT METHODS ------

    private void Start()
    {
        _currentNode = startNode;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            PathToNode(end);
        }

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
    public void AddToMovementQueue(MovementNode node)
    {
        _movementQueue.Enqueue(node);
    }

    public void PathToNode(MovementNode destination)
    {
        NodeTraverser traverser = new NodeTraverser(_currentNode, destination);
        Queue<MovementNode> path = traverser.Path;

        int pathLength = path.Count;
        for (int index = 0; index < pathLength; index++)
        {
            AddToMovementQueue(path.Dequeue());
        }
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
        _currentNode = _currentTarget;
        OnMovementCompleted?.Invoke(_currentTarget);
        _currentTarget = null;
    }
    
}
