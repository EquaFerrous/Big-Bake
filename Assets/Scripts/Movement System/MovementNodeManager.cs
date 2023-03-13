using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MovementNodeManager : MonoBehaviour
{
    // --------- VARIABLES ---------

    [SerializeField] private List<MovementNode> _nodeList = new();

    [Header("Debug")]
    [Tooltip("Show debug lines when object not selected.")]
    [SerializeField] private bool _permanentGizmos = false;

    public static MovementNodeManager Instance { get; private set; }


    public List<MovementNode> NodeList
    {
        get
        {
            return new List<MovementNode>(_nodeList);
        }
    }

    // ------ DEFAULT METHODS ------

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        _nodeList = new(); // Wipes the node list so nodes can be set up properly
    }

    private void OnDrawGizmosSelected()
    {
        if (!_permanentGizmos)
        {
            DrawDebugGizmos();
        }
    }

    private void OnDrawGizmos()
    {
        if (_permanentGizmos)
        {
            DrawDebugGizmos();
        }
    }

    // ------ PUBLIC METHODS -------

    /// <summary>
    /// Registers <paramref name="node"/> with the manager and sets it up correctly.
    /// Every node should be registered with the manager.
    /// </summary>
    /// <param name="node">The node to be registered.</param>
    public void RegisterNode(MovementNode node)
    {
        if (_nodeList.Contains(node))
        {
            return;
        }

        _nodeList.Add(node);
        foreach (MovementNode connectedNode in node.ConnectedNodes)
        {
            if (!connectedNode.ConnectedNodes.Contains(node))
            {
                connectedNode.ConnectNode(node);
            }
        }
    }

    /// <summary>
    /// Draws debug lines showing all nodes and their connections.
    /// </summary>
    public void DrawDebugGizmos()
    {
        // Draws all nodes
        foreach (MovementNode node in NodeList)
        {
            if (node.transform.Equals(Selection.activeTransform))
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.yellow;
            }
            Gizmos.DrawSphere(node.transform.position, 0.25f);
        }

        // Draws all node connections
        Gizmos.color = Color.cyan;
        List<MovementNode> drawnNodes = new();
        foreach (MovementNode node in NodeList)
        {
            //drawnNodes.Add(node); // Removed so lines show proprerly in inspector
            foreach (MovementNode connectedNode in node.ConnectedNodes)
            {
                if (!drawnNodes.Contains(connectedNode))
                {
                    Gizmos.DrawLine(node.transform.position, connectedNode.transform.position);
                }
            }
        }
    }

    // ------ PRIVATE METHODS ------
}
