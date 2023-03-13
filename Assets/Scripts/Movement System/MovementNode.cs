using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A node for use in the movement network.
/// Must be registered with the MovementNodeManager to get set-up correctly.
/// </summary>
public class MovementNode : MonoBehaviour
{
    // --------- VARIABLES ---------

    [SerializeField] private List<MovementNode> _connectedNodes = new();

    public List<MovementNode> ConnectedNodes
    {
        get
        {
            return new List<MovementNode>(_connectedNodes);
        }
    }


    // ------ DEFAULT METHODS ------

    private void Start()
    {
        MovementNodeManager.Instance.RegisterNode(this); // Vital to be set-up correctly.
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.25f);

        Gizmos.color = Color.cyan;
        foreach (MovementNode node in ConnectedNodes)
        {
            if (!node.ConnectedNodes.Contains(this))
            {
                Gizmos.color = Color.yellow;
            }
            else
            {
                Gizmos.color = Color.green;
            }
            
            Gizmos.DrawSphere(node.transform.position, 0.25f);
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }

    // ------ PUBLIC METHODS -------

    /// <summary>
    /// Connects <paramref name="node"/> to this node.
    /// </summary>
    /// <param name="node">The other node to be connected.</param>
    public void ConnectNode(MovementNode node)
    {
        if (ConnectedNodes.Contains(node) || node.Equals(this))
        {
            return;
        }

        _connectedNodes.Add(node);
    }

    // ------ PRIVATE METHODS ------



}
