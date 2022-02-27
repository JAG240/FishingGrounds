using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodControls : MonoBehaviour
{
    [SerializeField] float _lookTimer = 1.0f;
    private Vector3 _activePos;
    private Vector3 _inactivePos;
    private Transform _eyes; 

    void Awake()
    {
        Vector3 localPos = transform.localPosition;
        _inactivePos = new Vector3(localPos.x, localPos.y, localPos.z);
        _activePos = transform.parent.Find("CenterPos").localPosition;
        _eyes = transform.parent.Find("Eyes");
    }

    void OnEnable()
    {
        transform.localPosition = _activePos;
    }

    void OnDisable()
    {
        transform.localPosition = _inactivePos;
        StopAllCoroutines();
    }

    void Update()
    {
        
    }

    public void RotateCamera(Transform lookTarget)
    {
        Quaternion targetRot = Quaternion.LookRotation(lookTarget.position - transform.position);
        StartCoroutine(Look(targetRot));
    }

    private IEnumerator Look(Quaternion target)
    {
        float totalTime = 0.0f;

        while(totalTime < _lookTimer)
        {
            _eyes.transform.rotation = Quaternion.Slerp(_eyes.transform.rotation, target, (totalTime / _lookTimer));
            totalTime += Time.deltaTime;
            yield return null;
        }

        _eyes.transform.rotation = target;
        yield return null;
    }
}
