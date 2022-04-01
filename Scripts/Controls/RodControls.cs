using System;
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
    [SerializeField] private float _hookTimer = 1.0f;
    //[SerializeField] private float _hookStrength = 50.0f; //50 is a good starting point, increase this value to make the player pull hard on the mouse to hook
    [SerializeField] private float _reelSpeed = 2f;
    [SerializeField] private float _reelInDistance = 2f;
    #endregion

    #region Vars
    public Fish hookedFish;
    public FishBase bitingFish;
    public Action<bool> reelIn; 
    private InputManager _inputManager;
    private Vector3 _activePos;
    private Vector3 _inactivePos;
    private Transform _eyes;
    private bool _enableRodControls = false;
    private Vector3 _localRot;
    private Vector3 _localStartRot;
    [SerializeField] private Bobber _bobber;
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
        _bobber = transform.parent.Find("bobber").GetComponent<Bobber>();
    }

    void OnEnable()
    {
        transform.localPosition = _activePos;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameEventManager.Instance.fishBite.invokedEvent += StartBiteTimer;
    }

    void OnDisable()
    {
        transform.localPosition = _inactivePos;
        StopAllCoroutines();
        _enableRodControls = false;
        transform.localRotation = Quaternion.Euler(_localStartRot);
        GameEventManager.Instance.fishBite.invokedEvent -= StartBiteTimer;
    }

    void Update()
    {
        if (!_enableRodControls)
            return;
        
        if(Vector3.Distance(_bobber.transform.position, transform.position) <= _reelInDistance)
        {
            bool fishOn = hookedFish ? true : false;
            reelIn?.Invoke(fishOn);
        }

        ControlRod();
        ControlReel();
        
    }
    #endregion

    #region Logic
    private void ControlRod()
    {
        Vector2 mouseInput = _inputManager.GetMouseDelta();
        _localRot = transform.localRotation.eulerAngles;

        float horizontalValue = Mathf.Clamp(_localRot.x - (mouseInput.y * Time.deltaTime * _controlSpeed), 270f, 330f);
        float verticalValue = Mathf.Clamp(_localRot.y + (mouseInput.x * Time.deltaTime * _controlSpeed), 120f, 240f);

        transform.localRotation = Quaternion.Euler(horizontalValue, verticalValue, _localRot.z);
    }

    private void ControlReel()
    {
        if(InputManager.Instance.LeftActionHeld())
        {
            if(!hookedFish)
            {
                Vector3 direction = _bobber.transform.position - transform.position;
                direction.Normalize();
                direction.y = 0;

                _bobber.transform.position -= direction * Time.deltaTime * _reelSpeed;
            }
            else
            {
                //reel in the fish
            }
        }
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

    private void StartBiteTimer()
    {
        StartCoroutine(CheckHook());
    }

    //y need to be negative for the rod to move up and abs of x together to make this work 
    private IEnumerator CheckHook()
    {
        float timer = 0.0f;
        while(timer < _hookTimer)
        {
            Vector2 mouseDelta = InputManager.Instance.GetMouseDelta();
            timer += Time.deltaTime;

            if(mouseDelta.y < 0 && Mathf.Abs(mouseDelta.y) + Mathf.Abs(mouseDelta.x) > bitingFish.hookStrength)
            {
                GameEventManager.Instance.fishHooked.InvokeEvent();
                break;
            }

            yield return null;
        }

        //for later to make the player lose bait on bites put logic here
        //if(timer >= _hookTimer)
    }
    #endregion
}
