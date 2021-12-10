using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntegerVariableToColor : MonoBehaviour
{
    [SerializeField]
    private LeanTweenType _tweenType;
    [SerializeField]
    private float _changeDuration;

    [SerializeField]
    private SpriteRenderer _spriteTarget;
    [SerializeField]
    private IntegerVariable _refValue;
    [SerializeField]
    private bool _absoluteRefValue;
    [SerializeField]
    private int _minValue;
    [SerializeField]
    private Color _minColor;
    [SerializeField]
    private int _maxValue;
    [SerializeField]
    private Color _maxColor;
    [SerializeField]
    private int _thirdColorValue;
    [SerializeField]
    private Color _thirdColor;


    private void OnEnable()
    {
        _refValue.OnValueChange += UpdateColor;

        UpdateColor(_refValue.Value);
    }

    private void OnDisable()
    {
        _refValue.OnValueChange -= UpdateColor;
    }

    private LTDescr _ltd;
    private void UpdateColor(int newValue)
    {
        if (newValue == _thirdColorValue)
        {
            Color.RGBToHSV(_thirdColor, out float thirdHue, out float thirdSat, out float thirdLightness);
            TweenColor(thirdHue);
            return;
        }

        Color.RGBToHSV(_minColor, out float minHue, out float minSat, out float minLightness);
        Color.RGBToHSV(_maxColor, out float maxHue, out float maxSat, out float maxLightness);

        float ratio = (float)(newValue - _minValue) / (_maxValue - _minValue);
        float hue = Mathf.Lerp(minHue, maxHue, ratio);

        if (_absoluteRefValue)
        {
            ratio = (float)newValue / (_minValue + _maxValue) * 2f;
            hue = Mathf.Lerp(minHue, maxHue, Mathf.Abs(ratio));
        }

        TweenColor(hue);
    }

    private void TweenColor(float targetHue)
    {
        if (_ltd != null)
        {
            LeanTween.cancel(_ltd.id);
        }

        Color.RGBToHSV(_spriteTarget.color, out float currentHue, out float currentSat, out float currentBright);
        _ltd = LeanTween.value(currentHue, targetHue, _changeDuration).setEase(_tweenType).setOnUpdate(hueValue =>
        {
            _spriteTarget.color = Color.HSVToRGB(hueValue, 1f, 1f);
        }).setOnComplete(() => 
        {
            _ltd = null;
        });
    }
}
