using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPositioner : MonoBehaviour
{
    private float _smoothFactor = 2;
    [SerializeField] Transform _target;
    private Vector3 _offset = new Vector3(0, 0.15f, 0.85f);
    private Vector3 _euler = new Vector3(10, 0, 0);

    private void Start()
    {
        _target = Camera.main.transform;
        transform.position = GetTargetPos();
        transform.rotation = GetTargetRot();
    }

    private void Update()
    {
        Vector3 targetPos = GetTargetPos();
        transform.position = Vector3.Lerp(transform.position, targetPos, _smoothFactor * Time.deltaTime);

        Quaternion targetRot = GetTargetRot();
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, _smoothFactor * Time.deltaTime);
    }

    private Vector3 GetTargetPos()
    {
        Vector3 targetPos = _target.TransformPoint(_offset);
        Vector3 forward = Vector3.ProjectOnPlane(_target.forward, Vector3.up);

        targetPos = _target.position + (forward * _offset.z);
        targetPos.y = _target.position.y - _offset.y;

        return targetPos;
    }

    private Quaternion GetTargetRot()
    {
        return Quaternion.Euler(_euler.x, _target.eulerAngles.y, _euler.z);
    }
}
