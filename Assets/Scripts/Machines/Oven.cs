using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Oven : MonoBehaviour
{
    // --------- VARIABLES ---------

    [SerializeField] private ProgressBar progressBar;
    [SerializeField][Min(1)] private float updatesPerSecond;

    [Header("Timings")]
    [SerializeField][Min(0)] private float perfectCookTime;
    [SerializeField][Min(0)] private float timeToBurn;
    [SerializeField][Min(0)] private float maxBurnTime;

    private ProgressValue _progressValue;
    private bool _machineActive = false;
    private Coroutine _currentProgressionCoroutine;

    // ------ DEFAULT METHODS ------

    private void Start()
    {
        CreateNewValue();
    }

    // ------ PUBLIC METHODS -------

    /// <summary>
    /// Sets the machine to be active or not.
    /// </summary>
    /// <param name="active">The new active state of the machine.</param>
    public void SetActive(bool active)
    {
        if (active == _machineActive)
        {
            return;
        }

        _machineActive = active;
        if (_machineActive)
        {
            _currentProgressionCoroutine = StartCoroutine(AddProgressCoroutine());
        }
        else
        {
            StopCoroutine(_currentProgressionCoroutine);
        }
    }

    // ------ PRIVATE METHODS ------

    private void CreateNewValue()
    {
        _progressValue = new ProgressValue(perfectCookTime, timeToBurn, maxBurnTime);
        progressBar.ProgressValue = _progressValue;
    }


    private IEnumerator AddProgressCoroutine()
    {
        while (true)
        {
            float waitTime = 1 / updatesPerSecond;
            if (_machineActive)
            {
                _progressValue.AddProgress(waitTime);
            }
            yield return new WaitForSeconds(waitTime);
        }
    }

}
