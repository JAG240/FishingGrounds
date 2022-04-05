using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLine : MonoBehaviour
{
    [SerializeField] private float _sag = 0.4f;
    private LineRenderer _bobberLine;
    private Transform _rodTip;
    private Transform _bobber;
    private Rigidbody _bobberBody;
    private float _defaultSag = 3.5f;
    private LineRenderer _rodLine;
    private LineRenderer _hookLine;
    private Transform _hook;
    private RodControls _rodControls;

    void OnEnable()
    {
        _bobberLine = GetComponent<LineRenderer>();
        _bobber = transform.parent.Find("bobber");
        _bobberBody = _bobber.GetComponent<Rigidbody>();
        _rodTip = transform.Find("RodTip");
        _hookLine = _bobber.GetComponent<LineRenderer>();
        _hook = _bobber.Find("hook_end").transform;
        _rodControls = _rodTip.parent.GetComponent<RodControls>();

        _bobberLine.positionCount = 2;
        Application.onBeforeRender += UpdateLine;
        GameEventManager.Instance.fishBite.invokedEvent += Bite;
        GameEventManager.Instance.fishHooked.invokedEvent += Hooked;
        _bobber.GetComponent<Bobber>().exitCast += Casted;
        _rodControls.updateReelState += UpdateReelState;
    }

    void OnDisable()
    {
        Application.onBeforeRender -= UpdateLine;
        GameEventManager.Instance.fishBite.invokedEvent -= Bite;
        GameEventManager.Instance.fishHooked.invokedEvent -= Hooked;
        _rodControls.updateReelState -= UpdateReelState;
    }

    private void UpdateLine()
    {
        UpdateBobberLine();
        UpdateHookLine();
    }

    #region Bobber Line Methods
    //Create a method to set sag if fish is not hooked successfully and bobber it returned? 
    //Create a method to decrease sag if the user is reeling

    private void UpdateBobberLine()
    {
        int positions = Mathf.Max(Mathf.RoundToInt(Vector3.Distance(_rodTip.position, _bobber.position)) * 4, 2);

        if (_bobberLine.positionCount != positions)
            _bobberLine.positionCount = positions;

        for (int x = 0; x < positions; x++)
        {
            Vector3 pos;
            float interpolator = (float)x / (positions - 1);
            pos.x = Mathf.Lerp(_rodTip.position.x, _bobber.position.x, interpolator);
            pos.z = Mathf.Lerp(_rodTip.position.z, _bobber.position.z, interpolator);
            pos.y = Mathf.Lerp(_rodTip.position.y, _bobber.position.y, Mathf.Pow(interpolator, Mathf.Pow(interpolator, _sag * interpolator)));
            _bobberLine.SetPosition(x, pos);
        }
    }

    private void Hooked()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeSag(0, 0.1f));
    }

    private void Casted(bool success)
    {
        if (!success)
            return; 

        StopAllCoroutines();
        StartCoroutine(ChangeSag(3.5f, 1f));
    }

    private void Bite()
    {
        StartCoroutine(PlayBite());
    }

    private IEnumerator PlayBite()
    {
        float currentSag = _sag;
        yield return StartCoroutine(ChangeSag(0.2f, 0.1f));
        StartCoroutine(ChangeSag(currentSag, 1f));
    }

    private IEnumerator ChangeSag(float targetSag, float changeTime)
    {
        float totalTime = 0f;
        float startSag = _sag;

        while(totalTime < changeTime)
        {
            _sag = Mathf.Lerp(startSag, targetSag, (totalTime/changeTime));
            totalTime += Time.deltaTime;
            yield return null;
        }

        _sag = targetSag;
        yield return null;
    }
    #endregion

    //true for reeling false for not reeling 
    private void UpdateReelState(bool state)
    {
        if(state)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeSag(0.2f, 0.1f));
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(ChangeSag(_defaultSag, 1f));
        }
    }

    private void UpdateHookLine()
    {
        _hookLine.SetPosition(0, _bobber.position);
        _hookLine.SetPosition(1, _hook.position);
    }

}
