using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class will hold all logic to interact with the material properties of the casting skill check bar.
 * This must follow the order below to ensure proper display and calculation.
 * ORDER: Fill bar (starts with 1 for emtpy and goes to 0 full) > 
 * Assign range (range is always added around succes zone so 0.1 is acutally 20% of the bar) >
 * Assign zone weight (set as a percent of the succes range so 0.5 = 50% of the range) >
 * Set success position on bar (automatically takes into account the size of the success range)
 */

public class CastBar : MonoBehaviour
{
    private Camera _cam;
    private Material _mat;
    [SerializeField] float _camDistance;
    [SerializeField] float _camHeight;

    private float _greatSize;
    private float _goodSize;
    private float _successRange;
    private float _successPos;

    void Awake()
    {
        _cam = transform.root.GetChild(0).GetComponent<Camera>();
        _mat = GetComponent<MeshRenderer>().material;
    }

    private void OnEnable()
    {
        transform.position = _cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / _camHeight, _camDistance));
        transform.rotation = Quaternion.LookRotation(transform.position - transform.root.position);
    }

    public void ClearBar()
    {
        _mat.SetFloat("_BarFill", 1);
    }

    private void AssignWeights(float greatWeight, float goodWeight)
    {
        _greatSize = _successRange * greatWeight;
        _goodSize = _successRange * goodWeight + _greatSize;
        _mat.SetFloat("_GreatSize", _greatSize);
        _mat.SetFloat("_GoodSize", _goodSize);
    }

    private void AssignSuccessRange(float range)
    {
        _successRange = range;
        _mat.SetFloat("_SuccessRange", range);
    }

    private void SetSuccessPos()
    {
        float pos = Random.Range(_successRange, 1 - _successRange);
        _successPos = pos;
        _mat.SetFloat("_SuccessZonePos", pos);
    }

    public float GetResults(float pos)
    {
        // pos - successPos less than the range to be in the success zone at all
        //no bigger than pos + size for in that zone 

        float result = Mathf.Abs(pos - _successPos);

        if (result > _successRange)
            return 0;

        if (_greatSize > 0 && result <= _greatSize)
            return 1.5f;
        else if (result <= _goodSize)
            return 1f;
        else
            return 0.8f;
    }
}
