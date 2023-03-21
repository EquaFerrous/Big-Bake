using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeTraverser
{
    // --------- VARIABLES ---------

    private readonly Dictionary<MovementNode, NodeTraversalData> _nodeDataDict = new();
    private readonly List<MovementNode> _traversedNodes = new();
    private readonly Queue<MovementNode> _selectedRoute = new();

    private MovementNode _startNode;
    private MovementNode _endNode;

    public Queue<MovementNode> Path
    {
        get
        {
            return new(_selectedRoute);
        }
    }

    // ------ DEFAULT METHODS ------
    
    public NodeTraverser(MovementNode startNode, MovementNode endNode)
    {
        _startNode = startNode;
        _endNode = endNode;

        TraverseNode(startNode);
    }
    
    // ------ PUBLIC METHODS -------
    

    
    // ------ PRIVATE METHODS ------

    private NodeTraversalData GetNodeData(MovementNode node)
    {
        if (_nodeDataDict.ContainsKey(node))
        {
            return _nodeDataDict[node];
        }

        NodeTraversalData nodeData = new();
        _nodeDataDict.Add(node, nodeData);
        return nodeData;
    }

    
    private void TraverseNode(MovementNode currentNode)
    {
        // Setup the completed node data
        NodeTraversalData currentNodeData = GetNodeData(currentNode);
        if (currentNode.Equals(_startNode))
        {
            currentNodeData.FinaliseStartNode();
        }
        else
        {
            currentNodeData.FinaliseNode();
        }
        _traversedNodes.Add(currentNode);
        

        // Calculate working values for all connected nodes
        foreach (MovementNode connectedNode in currentNode.ConnectedNodes)
        {
            if (_traversedNodes.Contains(connectedNode))
            {
                continue;
            }

            NodeTraversalData connectedNodeData = GetNodeData(connectedNode);
            float totalDistance = currentNodeData.FinalDistance + currentNode.NodeDistances[connectedNode];
            if (connectedNodeData.WorkingValues.Count == 0 || totalDistance < connectedNodeData.GetLowestWorkingValue())
            {
                connectedNodeData.AddWorkingValue(totalDistance);
            }
        }

        // Traverse the next node
        MovementNode nextNode = GetNextNodeToTraverse();
        if (nextNode == null)
        {
            // Log error if the nodes are not connected
            Debug.LogError($"[Pathfinder] No route between {_startNode.gameObject.name} and {_endNode.gameObject.name}");
        }
        else if (nextNode.Equals(_endNode))
        {
            FinishTraversal();
        }
        else
        {
            TraverseNode(nextNode);
        }
    }

    /// <summary>
    /// Gets the next node with the lowest working value to traverse to.
    /// </summary>
    /// <returns>
    /// Returns the untraversed node with lowest working value.
    /// Or <c>null</c> if no untraversed nodes remain.
    /// </returns>
    private MovementNode GetNextNodeToTraverse()
    {
        MovementNode nextNode = null;
        float lowestWorkingValue = float.PositiveInfinity;

        foreach (MovementNode node in _nodeDataDict.Keys)
        {
            NodeTraversalData nodeData = GetNodeData(node);

            // If already traversed, don't go back to it
            if (_traversedNodes.Contains(node))
            {
                continue;
            }

            // Ensuring no erroneous nodes are chosen
            if (nodeData.WorkingValues.Count == 0)
            {
                continue;
            }

            float nodeWorkingValue = nodeData.GetLowestWorkingValue();
            if (nodeWorkingValue < lowestWorkingValue)
            {
                nextNode = node;
                lowestWorkingValue = nodeWorkingValue;
            }
        }

        return nextNode;
    }


    private void FinishTraversal()
    {
        NodeTraversalData endNodeData = GetNodeData(_endNode);
        endNodeData.FinaliseNode();
        _traversedNodes.Add(_endNode);

        CalculateRouteBackToStart();
    }

    private void CalculateRouteBackToStart()
    {
        Stack<MovementNode> route = new();
        MovementNode currentNode = _endNode;

        while (!currentNode.Equals(_startNode))
        {
            route.Push(currentNode);
            currentNode = FindPreviousNode(currentNode);

            if (currentNode == null)
            {
                Debug.LogError($"[Pathfinder] Error finding previous node from {route.Peek().gameObject.name}.");
            }
        }

        route.Push(_startNode);
        int routeLength = route.Count;
        
        for (int index = 0; index < routeLength; index++)
        {
            _selectedRoute.Enqueue(route.Pop());
        }
    }



    private MovementNode FindPreviousNode(MovementNode node)
    {
        NodeTraversalData nodeData = GetNodeData(node);
        MovementNode previousNode = null;

        foreach (MovementNode connectedNode in node.ConnectedNodes)
        {
            NodeTraversalData connectedNodeData = GetNodeData(connectedNode);
            if (connectedNodeData.FinalDistance < 0)
            {
                continue;
            }

            float testDistance = nodeData.FinalDistance - node.NodeDistances[connectedNode];
            if (Mathf.Approximately(testDistance, connectedNodeData.FinalDistance))
            {
                previousNode = connectedNode;
                break;
            }
        }

        return previousNode;
    }

}
