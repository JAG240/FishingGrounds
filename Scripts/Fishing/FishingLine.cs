using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLine : MonoBehaviour
{
    [SerializeField] private float _sag = 0.4f;
    private LineRenderer _lineRenderer;
    private Transform _rodTip;
    private Transform _bobber;

    void OnEnable()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _bobber = transform.parent.Find("bobber");
        _rodTip = transform.Find("RodTip");

        _lineRenderer.positionCount = 2;
        Application.onBeforeRender += UpdateLine;
    }

    private void OnDisable()
    {
        Application.onBeforeRender -= UpdateLine;
    }

    private void UpdateLine()
    {
        int segments = Mathf.Max(Mathf.RoundToInt(Vector3.Distance(_rodTip.position, _bobber.position)) * 4, 2);

        if(_lineRenderer.positionCount != segments)
                _lineRenderer.positionCount = segments;

        for(int x = 0; x < segments; x++)
        {
            Vector3 pos;
            float interpolator = (float)x / segments;
            pos.x = Mathf.Lerp(_rodTip.position.x, _bobber.position.x, interpolator);
            pos.z = Mathf.Lerp(_rodTip.position.z, _bobber.position.z, interpolator);
            pos.y = Mathf.Lerp(_rodTip.position.y, _bobber.position.y, Mathf.Pow(interpolator, Mathf.Pow(interpolator, _sag * interpolator)));
            _lineRenderer.SetPosition(x, pos);
        }
    }
}
