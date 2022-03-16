using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLine : MonoBehaviour
{
    [SerializeField] private float _sag = 0.4f;
    private LineRenderer _lineRenderer;
    private Transform _rodTip;
    private Transform _bobber;
    private Rigidbody _bobberBody;
    private float _defaultSag = 3.5f;

    void OnEnable()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _bobber = transform.parent.Find("bobber");
        _bobberBody = _bobber.GetComponent<Rigidbody>();
        _rodTip = transform.Find("RodTip");

        _lineRenderer.positionCount = 2;
        Application.onBeforeRender += UpdateLine;
        GameEventManager.Instance.fishBite.invokedEvent += Bite;
    }

    void OnDisable()
    {
        Application.onBeforeRender -= UpdateLine;
    }

    void Update()
    {

    }

    private void UpdateLine()
    {
        int positions = Mathf.Max(Mathf.RoundToInt(Vector3.Distance(_rodTip.position, _bobber.position)) * 4, 2);

        if(_lineRenderer.positionCount != positions)
                _lineRenderer.positionCount = positions;

        for(int x = 0; x < positions; x++)
        {
            Vector3 pos;
            float interpolator = (float)x / (positions - 1);
            pos.x = Mathf.Lerp(_rodTip.position.x, _bobber.position.x, interpolator);
            pos.z = Mathf.Lerp(_rodTip.position.z, _bobber.position.z, interpolator);
            pos.y = Mathf.Lerp(_rodTip.position.y, _bobber.position.y, Mathf.Pow(interpolator, Mathf.Pow(interpolator, _sag * interpolator)));
            _lineRenderer.SetPosition(x, pos);
        }
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
}
