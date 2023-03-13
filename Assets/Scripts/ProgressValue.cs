using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to track the progress value of an object.
/// </summary>
public class ProgressValue
{
    // --------- VARIABLES ---------

    public event Action OnChange;

    private float _currentValue = 0;
    private float _underdoneLength = 0;
    private float _perfectLength = 0;
    private float _overdoneLength = 0;


    // ------ PARAMETERS ---------

    /// <summary>
    /// The state of the progress value.
    /// <list type="table">
    ///     <item>
    ///         <term>Underdone</term>
    ///         <description>The value is below perfect progress.</description>
    ///     </item>
    ///     <item>
    ///         <term>Perfect</term>
    ///         <description>The value is within perfect progress.</description>
    ///     </item>
    ///     <item>
    ///         <term>Overdone</term>
    ///         <description>The value is above perfect progress.</description>
    ///     </item>
    /// </list>
    /// </summary>
    public ProgressState CurrentProgressState
    {
        get
        {
            if (CurrentValue <= _underdoneLength)
            {
                return ProgressState.Underdone;
            }
            else if (CurrentValue <= _perfectLength)
            {
                return ProgressState.Perfect;
            }
            else
            {
                return ProgressState.Overdone;
            }
        }
    } 

    public float CurrentValue
    {
        get => _currentValue;
        private set
        {
            _currentValue = value;
            OnChange?.Invoke();
        }
    }

    public float MaxValue { get; private set; }

    public float MinPerfectValue => _underdoneLength;

    public float MaxPerfectValue
    {
        get
        {
            return _underdoneLength + _perfectLength;
        }
    }

    // ------ DEFAULT METHODS ------

    /// <summary>
    /// A class to track the progress value of an object.
    /// </summary>
    /// <param name="underdoneLength">How long the value is considered <c>Underdone</c>.</param>
    /// <param name="perfectLength">How long the value is considered <c>Perfect</c> after being <c>Underdone</c>.</param>
    /// <param name="overdoneLength">How long the value is considered <c>Overdone</c> after being <c>Perfect</c>.</param>
    public ProgressValue(float underdoneLength, float perfectLength, float overdoneLength)
    {
        _underdoneLength = underdoneLength;
        _perfectLength = perfectLength;
        _overdoneLength = overdoneLength;
        MaxValue = underdoneLength + perfectLength + overdoneLength;
    }
    
    // ------ PUBLIC METHODS -------
    
    /// <summary>
    /// Adds <paramref name="amount"/> to the progress value.
    /// Clamps the value to the max value after being "overdone".
    /// </summary>
    /// <param name="amount">The amount to be added to the value.</param>
    /// <returns>The new progress value.</returns>
    public float AddProgress(float amount)
    {
        float newValue = CurrentValue + amount;
        CurrentValue = Mathf.Min(newValue, MaxValue);

        return CurrentValue;
    }

    
    // ------ PRIVATE METHODS ------
    
    
    
}
