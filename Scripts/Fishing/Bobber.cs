using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    private Rigidbody _body;
    private Vector3 _localPosition;
    private Quaternion _localRotation;
    private float _waterLevel;

    public event Action<bool> exitCast;

    void Awake()
    {
        _localPosition = transform.localPosition;
        _localRotation = transform.localRotation;
        _body = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag != "Water")
        {
            Return();
            exitCast?.Invoke(false);
        }
        else
        {
            exitCast?.Invoke(true);
            _body.useGravity = false;
            _body.velocity = Vector3.zero;
            //start fishing anims
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Water")
        {
            Return();
            exitCast?.Invoke(false);
        }
        else
        {
            _waterLevel = other.transform.position.y + (other.transform.localScale.y / 2);
            transform.position = new Vector3(transform.position.x, _waterLevel, transform.position.z);
            exitCast?.Invoke(true);
            _body.useGravity = false;
            _body.velocity = Vector3.zero;
            //start fishing anims
        }
    }

    public void Cast(Transform player, float force)
    {
        _body.useGravity = true;
        _body.velocity = Vector3.zero;
        _body.AddForce(player.forward * force, ForceMode.VelocityChange);
    }

    public void Return()
    {
        _body.useGravity = false;
        _body.velocity = Vector3.zero;
        transform.localPosition = _localPosition;
        transform.localRotation = _localRotation;
    }
}
