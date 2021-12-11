using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(UIElement))]
public class UITransition : MonoBehaviour
{
    [SerializeField]
    private TransitionType _transitionType;

    [SerializeField]
    private Vector3 _from;
    [SerializeField]
    private Vector3 _to;

    [SerializeField]
    private float _showTransitionTime;
    [SerializeField]
    private LeanTweenType _showLeanTweenType;

    [SerializeField]
    private float _hideTransitionTime;
    [SerializeField]
    private LeanTweenType _hideLeanTweenType;

    [SerializeField]
    private UnityEvent _onStartShow;
    [SerializeField]
    private UnityEvent _onFinishShow;
    [SerializeField]
    private UnityEvent _onStartHide;
    [SerializeField]
    private UnityEvent _onFinishHide;

    private UIPanel _parentPanel;

    private LTDescr _showDescription;
    private LTDescr _hideDescription;

    private Color _originColor;
    private Color _transparentColor;

    private WaitForSeconds _waitForshowDuration;
    private WaitForSeconds _waitForHideDuration;

    private CanvasGroup _canvasGroup;

    public void Init(UIPanel parent)
    {
        _parentPanel = parent;

        _waitForshowDuration = new WaitForSeconds(_showTransitionTime);
        _waitForHideDuration = new WaitForSeconds(_hideTransitionTime);
        _canvasGroup = GetComponent<CanvasGroup>();

    }

    public void PreShowSetup()
    {
        switch (_transitionType)
        {
            case TransitionType.Fade:
                {
                    if (_canvasGroup == null)
                    {
                        Debug.LogWarning("In order to use Fade Transition, add CanvasGroup Component to this object", this);
                        return;
                    }

                    LeanTween.alphaCanvas(_canvasGroup, _from.x, 0);
                    break;
                }
            case TransitionType.Move:
                {
                    transform.localPosition = _from;
                    break;
                }
            case TransitionType.Zoom:
                {
                    transform.localScale = _from;
                    break;
                }
            default:
                {
                    return;
                }
        }
    }

    public void ShowTransition()
    {
        _onStartShow.Invoke();
        Observable.Timer(System.TimeSpan.FromSeconds(_showTransitionTime)).Subscribe(_ =>
        {
            _onFinishShow.Invoke();
        });

        StartCoroutine(IE_NotifyShown());

        LTDescr showDescription;
        switch (_transitionType)
        {
            case TransitionType.Fade:
                {
                    if (_canvasGroup == null)
                    {
                        Debug.LogWarning("In order to use Fade Transition, add CanvasGroup Component to this object", this);
                        return;
                    }

                    LeanTween.alphaCanvas(_canvasGroup, _from.x, 0);
                    showDescription = LeanTween.alphaCanvas(_canvasGroup, _to.x, _showTransitionTime);
                    break;
                }
            case TransitionType.Move:
                {
                    transform.localPosition = _from;
                    showDescription = LeanTween.moveLocal(gameObject, _to, _showTransitionTime);
                    break;
                }
            case TransitionType.Zoom:
                {
                    transform.localScale = _from;
                    showDescription = LeanTween.scale(gameObject, _to, _showTransitionTime);
                    break;
                }
            default:
                {
                    return;
                }
        }

        showDescription.setEase(_showLeanTweenType);
    }

    public void HideTransition()
    {
        _onStartHide.Invoke();
        Observable.Timer(System.TimeSpan.FromSeconds(_hideTransitionTime)).Subscribe(_ =>
        {
            _onFinishHide.Invoke();
        });

        StartCoroutine(IE_NotifyHided());

        LTDescr hideDescription;
        switch (_transitionType)
        {
            case TransitionType.Fade:
                {
                    if (_canvasGroup == null)
                    {
                        Debug.LogWarning("In order to use Fade Transition, add CanvasGroup Component to this object", this);
                        return;
                    }

                    hideDescription = LeanTween.alphaCanvas(_canvasGroup, _from.x, _hideTransitionTime);
                    break;
                }
            case TransitionType.Move:
                {
                    hideDescription = LeanTween.moveLocal(gameObject, _from, _hideTransitionTime).setEase(_hideLeanTweenType);
                    break;
                }
            case TransitionType.Zoom:
                {
                    hideDescription = LeanTween.scale(gameObject, _from, _hideTransitionTime);
                    break;
                }
            default:
                {
                    return;
                }
        }

        hideDescription.setEase(_hideLeanTweenType);
    }

    private IEnumerator IE_NotifyShown()
    {
        yield return _waitForshowDuration;
        _parentPanel.NotifyElementShowed();
        Debug.Log($"{gameObject.name} {transform.localPosition} after");
    }

    private IEnumerator IE_NotifyHided()
    {
        yield return _waitForHideDuration;
        _parentPanel.NotifyElementHided();
    }
}

public enum TransitionType
{
    None = 0,
    Move = 1,
    Zoom = 2,
    Fade = 3
}
