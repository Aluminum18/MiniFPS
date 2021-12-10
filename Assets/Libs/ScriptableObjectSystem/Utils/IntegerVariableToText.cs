using UnityEngine;
using TMPro;
using UniRx;

public class IntegerVariableToText : MonoBehaviour
{
    [SerializeField]
    private IntegerVariable _intVariable;
    [SerializeField]
    private TMP_Text _textMesh;
    [SerializeField]
    private bool _continueUpdate = true;
    [SerializeField]
    private bool _continuousChange = false;
    [SerializeField]
    private int _unitChangePerSec = 5;
    [SerializeField]
    private float _continuousChangeCapTime = 1f;
    [SerializeField]
    private string _format = "{0}";

    private CompositeDisposable _cd = new CompositeDisposable();

    private int _lastValue = 0;
    private void OnEnable()
    {
        UpdateValue(_intVariable.Value);
        
        if (_continueUpdate)
        {
            _intVariable.OnValueChange += UpdateValue;
        }
    }

    private void OnDisable()
    {
        if (_continueUpdate)
        {
            _intVariable.OnValueChange -= UpdateValue;
        }
    }

    private void UpdateValue(int newValue)
    {
        if (_continuousChange)
        {
            ContinuousUpdateValue(newValue);
            return;
        }
        _textMesh.text = string.Format(_format, newValue.ToString());
    }

    private void ContinuousUpdateValue(int newValue)
    {
        _cd.Clear();

        int diff = newValue - _lastValue;

        float expectedTime = (float)(Mathf.Abs(diff)) / _unitChangePerSec;

        float changePerSec = _unitChangePerSec * Mathf.Sign(diff);
        if (expectedTime > _continuousChangeCapTime)
        {
            changePerSec = diff / _continuousChangeCapTime;
            expectedTime = _continuousChangeCapTime;
        }

        float bufferValue = _lastValue;
        _lastValue = newValue;

        Observable.EveryUpdate().Subscribe(_ =>
        {
            bufferValue += changePerSec * Time.deltaTime;
            if (expectedTime < 0f)
            {
                _textMesh.text = string.Format(_format, newValue.ToString());
                _cd.Clear();
                return;
            }
            _textMesh.text = string.Format(_format, ((int)bufferValue).ToString());
            expectedTime -= Time.deltaTime;
        }).AddTo(_cd);
    }
}
