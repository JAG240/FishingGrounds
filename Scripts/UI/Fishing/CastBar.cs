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
    #region Private vars
    private Camera _cam;
    private Material _mat;
    private Coroutine _barFillCoroutine;
    #endregion

    #region Serialized Vars
    [SerializeField] private float _camDistance;
    [SerializeField] private float _camHeight;
    [SerializeField] private float _minBarFillSpeed = 0.05f;
    [SerializeField] private float _maxBarFillSpeed = 0.2f;
    [SerializeField] private float _closestRange = 3.0f;
    [SerializeField] private float _okayMultiplier = 0.8f;
    [SerializeField] private float _goodMultiplier = 1.0f;
    [SerializeField] private float _greatMultiplier = 1.2f;
    #endregion

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

    #region Monobehaviors
    void Awake()
    {
        _cam = transform.root.GetChild(0).GetComponent<Camera>();
        _mat = GetComponent<MeshRenderer>().material;
    }

    private void OnEnable()
    {
        //positions the bar on the screen in world position 
        transform.position = _cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / _camHeight, _camDistance));
        transform.rotation = Quaternion.LookRotation(transform.position - transform.root.position);
    }
    #endregion

    #region Cast Bar Logic
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

        //if the bar is not filled to the success range then set to middle of the bar
        if(_successRange >= barFill)
        {
            _successPos = barFill / 2f;
            return;
        }

        //randomly assigns success zone from the top of the bar to the bottom of the bar
        //taking into account the size of the success range and clostest possible range
        float pos = Random.Range(1 -_successRange, 1 - (barFill -(_successRange * _closestRange)));
        _successPos = pos;
    }

    public float GetMultiplier()
    {
        float pos = Mathf.Abs(barFill - 1);
        float result = Mathf.Abs(pos - _successPos);

        //If not in succes range at all failed skill check
        if (result > _successRange)
            return 0;

        //Determines which multiplier was achieved
        if (_greatSize > 0 && result <= _greatSize)
            return _greatMultiplier;
        else if (_goodSize > 0 && result <= _goodSize)
            return _goodMultiplier;
        else
            return _okayMultiplier;
    }

    private IEnumerator FillCastMeter()
    {
        float barFill = 1;
        float fillSpeed;

        while (barFill > 0)
        {
            fillSpeed = Mathf.Lerp(_minBarFillSpeed, _maxBarFillSpeed, barFill);
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
            unfillSpeed = Mathf.Lerp(_maxBarFillSpeed, _minBarFillSpeed, barFill);
            barFill += unfillSpeed * Time.fixedDeltaTime;
            this.barFill = barFill;
            yield return null;
        }
    }
    #endregion
}
