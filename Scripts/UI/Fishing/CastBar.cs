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
    [SerializeField] private float _camDistance;
    [SerializeField] private float _camHeight;
    private Coroutine _barFillCoroutine;

    #region Shader Accessors and Mutators
    public float barFill
    {
        get { return 1 - Mathf.Clamp01(_mat.GetFloat("_BarFill")); }
        private set { _mat.SetFloat("_BarFill", value); }
    }
    private float _greatSize
    {
        get { return _mat.GetFloat("_GreatSize"); }
        set { _mat.SetFloat("_GreatSize", value); }
    }
    private float _goodSize
    {
        get { return _mat.GetFloat("_GoodSize"); }
        set { _mat.SetFloat("_GoodSize", value); }
    }
    private float _successRange
    {
        get { return _mat.GetFloat("_SuccessRange"); }
        set { _mat.SetFloat("_SuccessRange", value); }
    }
    private float _successPos
    {
        get { return _mat.GetFloat("_SuccessZonePos"); }
        set { _mat.SetFloat("_SuccessZonePos", value); }
    }
    #endregion


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
        barFill = 1;
        _successRange = 0;
    }

    public void FillCastMeter(bool state)
    {
        if (state)
           _barFillCoroutine = StartCoroutine(FillCastMeter());
        else
            StopCoroutine(_barFillCoroutine);
    }

    public void UnfillCastMeter(bool state)
    {
        if (state)
            _barFillCoroutine = StartCoroutine(UnfillCastMeter());
        else
            StopCoroutine(_barFillCoroutine);
    }

    public void AssignSuccessVars(float range, float greatWeight, float goodWeight)
    {
        _successRange = range;
        _greatSize = _successRange * greatWeight;
        _goodSize = _successRange * goodWeight + _greatSize;

        if(_successRange >= barFill)
        {
            _successPos = barFill / 2f;
            return;
        }

        //increase (_successRange * num) num to make the success zone farther from the fill at min
        float pos = Random.Range(1 -_successRange, 1 - (barFill -(_successRange*3)));
        _successPos = pos;
    }

    public float GetResults()
    {
        // pos - successPos less than the range to be in the success zone at all
        //no bigger than pos + size for in that zone 
        float pos = Mathf.Abs(barFill - 1);
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

    private IEnumerator FillCastMeter()
    {
        float barFill = 1;
        float fillSpeed;

        while (barFill > 0)
        {
            fillSpeed = Mathf.Lerp(0.05f, 0.2f, barFill);
            barFill -= fillSpeed * Time.fixedDeltaTime;
            this.barFill = barFill;
            yield return null;
        }
    }

    private IEnumerator UnfillCastMeter()
    {
        float barFill = Mathf.Abs(this.barFill - 1);
        float unfillSpeed;

        while (barFill < 1)
        {
            unfillSpeed = Mathf.Lerp(0.2f, 0.05f, barFill);
            barFill += unfillSpeed * Time.fixedDeltaTime;
            this.barFill = barFill;
            yield return null;
        }
    }
}
