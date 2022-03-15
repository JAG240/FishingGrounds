using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class are for the cotrols that the playewr will use when they are in the hook or reel state
 */

public class RodControls : MonoBehaviour
{
    #region Searlized Vars
    [SerializeField] private float _lookTimer = 1.0f;
    [SerializeField] private float _controlSpeed = 2.0f;
    #endregion

    #region Vars
    private InputManager _inputManager;
    private Vector3 _activePos;
    private Vector3 _inactivePos;
    private Transform _eyes;
    private bool _enableRodControls = false;
    private Vector3 _localRot;
    private Vector3 _localStartRot;
    #endregion

    #region Monobehavior
    void Awake()
    {
        Vector3 localPos = transform.localPosition;
        _inactivePos = new Vector3(localPos.x, localPos.y, localPos.z);
        _activePos = transform.parent.Find("CenterPos").localPosition;
        _eyes = transform.parent.Find("Eyes");
        _inputManager = InputManager.Instance;
        _localStartRot = transform.localRotation.eulerAngles;
    }

    void OnEnable()
    {
        transform.localPosition = _activePos;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        transform.localPosition = _inactivePos;
        StopAllCoroutines();
        _enableRodControls = false;
        transform.localRotation = Quaternion.Euler(_localStartRot);
    }

    void Update()
    {
        if (_enableRodControls)
            ControlRod();
    }
    #endregion

    #region Logic
    private void ControlRod()
    {
        Vector2 mouseInput = _inputManager.GetMouseDelta();
        _localRot = transform.localRotation.eulerAngles;

        float horizontalValue = Mathf.Clamp(_localRot.x - (mouseInput.y * Time.deltaTime * _controlSpeed), 270f, 340f);
        float verticalValue = Mathf.Clamp(_localRot.y + (mouseInput.x * Time.deltaTime * _controlSpeed), 120f, 240f);

        transform.localRotation = Quaternion.Euler(horizontalValue, verticalValue, _localRot.z);
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
        _enableRodControls = true;
        yield return null;
    }
    #endregion
}
