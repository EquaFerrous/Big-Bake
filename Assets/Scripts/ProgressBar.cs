using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    // --------- VARIABLES ---------

    [SerializeField] private Transform valueBar;
    [SerializeField] private Transform underdoneBar;
    [SerializeField] private Transform overdoneBar;

    private ProgressValue _progressValue;

    public ProgressValue ProgressValue
    {
        get => _progressValue;
        set
        {
            if (_progressValue != null)
            {
                _progressValue.OnChange -= UpdateValueBar;
            }

            _progressValue = value;
            if (_progressValue != null)
            {
                _progressValue.OnChange += UpdateValueBar;
            }
            SetupProgressBar();
        }
    }

    // ------ DEFAULT METHODS ------

    private void Start()
    {
        ProgressValue = new ProgressValue(50, 30, 10);
    }

    // ------ PUBLIC METHODS -------



    // ------ PRIVATE METHODS ------

    private void SetupProgressBar()
    {
        float underdoneBarLength = ProgressValue.MinPerfectValue / ProgressValue.MaxValue;
        underdoneBar.localScale = new Vector3(underdoneBarLength, 1, 1);

        float overdoneBarLength = (ProgressValue.MaxValue - ProgressValue.MaxPerfectValue) / ProgressValue.MaxValue;
        overdoneBar.localScale = new Vector3(overdoneBarLength, 1, 1);

        UpdateValueBar();
    }

    private void UpdateValueBar()
    {
        float valueBarLength = ProgressValue.CurrentValue / ProgressValue.MaxValue;
        valueBar.localScale = new Vector3(valueBarLength, 1, 1);
    }
}
