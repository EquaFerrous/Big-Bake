using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDebug : MonoBehaviour
{
    // --------- VARIABLES ---------

    public MovementNode start;
    public MovementNode end;

    // ------ DEFAULT METHODS ------

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            new NodeTraverser(start, end);
        }
    }

    // ------ PUBLIC METHODS -------



    // ------ PRIVATE METHODS ------



}
