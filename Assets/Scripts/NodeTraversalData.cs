using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeTraversalData
{
    // --------- VARIABLES ---------

    private readonly List<float> _workingValues = new();
    private float _finalDistance = -1;
    

    public List<float> WorkingValues
    {
        get
        {
            return new(_workingValues);
        }
    }

    public float FinalDistance => _finalDistance;
    
    // ------ DEFAULT METHODS ------
    
    
    
    // ------ PUBLIC METHODS -------
    
    public void FinaliseNode()
    {
        _finalDistance = GetLowestWorkingValue();
    }

    public void FinaliseStartNode()
    {
        _finalDistance = 0;
    }

    public void AddWorkingValue(float value)
    {
        _workingValues.Add(value);
    }

    public float GetLowestWorkingValue()
    {
        List<float> sortedList = WorkingValues;
        sortedList.Sort();
        return sortedList[0];
    }

    // ------ PRIVATE METHODS ------
    
    
    
}
