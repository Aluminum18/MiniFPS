using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    [Header("Reference - Read")]
    [SerializeField]
    private FloatVariable _accuracy;
    [SerializeField]
    private FloatVariable _goodShot;
    [SerializeField]
    private FloatVariable _detection;

    [Header("Reference - Write")]
    [SerializeField]
    private FloatVariable _accuracyForView;
    [SerializeField]
    private FloatVariable _goodShotForView;
    [SerializeField]
    private FloatVariable _detectionForView;

    [Header("Config")]
    [SerializeField]
    private Image _accuracyImage;
    [SerializeField]
    private Image _goodShotImage;
    [SerializeField]
    private Image _detectionImage;
    [SerializeField]
    private float _statsChartShowTime = 1f;

    private IDisposable _updateImageStream;

    public void ShowStats()
    {
        _accuracyForView.Value = _accuracy.Value * 100f;
        _goodShotForView.Value = _goodShot.Value * 100f;
        _detectionForView.Value = _detection.Value * 100f;

        ShowStatsImageAnim();
    }

    public void ResetViewValues()
    {
        _accuracyForView.Value = 0f;
        _goodShotForView.Value = 0f;
        _detectionForView.Value = 0f;
        _accuracyImage.fillAmount = 0f;
        _goodShotImage.fillAmount = 0f;
        _detectionImage.fillAmount = 0f;
    }

    private void ShowStatsImageAnim()
    {
        _updateImageStream?.Dispose();

        _accuracyImage.fillAmount = 0f;
        _goodShotImage.fillAmount = 0f;
        _detectionImage.fillAmount = 0f;

        float time = _statsChartShowTime;
        _updateImageStream = Observable.EveryUpdate().Subscribe(_ =>
        {
            if (time <= 0f)
            {
                _accuracyImage.fillAmount = _accuracy.Value;
                _goodShotImage.fillAmount = _goodShot.Value;
                _detectionImage.fillAmount = _detection.Value;
                _updateImageStream.Dispose();
                return;
            }

            _accuracyImage.fillAmount += _accuracy.Value * Time.deltaTime;
            _goodShotImage.fillAmount += _goodShot.Value * Time.deltaTime;
            _detectionImage.fillAmount += _detection.Value * Time.deltaTime;

            time -= Time.deltaTime;
        });
    }
}
