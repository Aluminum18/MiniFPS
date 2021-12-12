using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatVariableToSlider : MonoBehaviour
{
    [SerializeField]
    private FloatVariable _float;
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private bool _getInitValueOnly = false;

    private void Start()
    {
        _slider.value = _float.Value;
        if (_getInitValueOnly)
        {
            return;
        }

        _float.OnValueChange += UpdateSliderValue;
    }

    private void OnDestroy()
    {
        if (_getInitValueOnly)
        {
            return;
        }

        _float.OnValueChange -= UpdateSliderValue;
    }

    private void UpdateSliderValue(float newValue)
    {
        _slider.value = newValue;
    }
}
