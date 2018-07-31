using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Actor {

    public float fovLerpRate = 0.2f;
    public float fovScalingAccuracy = 0.001f;

    protected Camera _camera;

    #region FOV Variables
    protected float _defaultFOV;
    protected float _targetFOV;
    protected bool _FOVCoroutineActive = false;
    #endregion

    protected virtual void Start()
    {
        _camera = gameObject.GetComponent<Camera>();
        if(_camera)
        {
            _defaultFOV = _camera.fieldOfView;
        }
        else
        {
            LOG_ERROR("CameraManager script not on camera object!");
        }
    }

    public virtual void ScaleFovBy(float fovModifier)
    {
        _targetFOV = _defaultFOV * fovModifier;
        if (!_FOVCoroutineActive)
        {
            StartCoroutine(Coroutine_ScaleFov());
        }
    }

    protected virtual IEnumerator Coroutine_ScaleFov()
    {
        _FOVCoroutineActive = true;
        //float overallFovModifier = _fovMultipliers.Product();
        if (_camera)
        {
            while(Mathf.Abs(_camera.fieldOfView - _targetFOV) > fovScalingAccuracy)
            {
                float newFOV = Mathf.Lerp(_camera.fieldOfView, _targetFOV, fovLerpRate);
                _camera.fieldOfView = newFOV;
                yield return null;
            }
        }
        //Snap FOV to target to remove innacuracies.
        _camera.fieldOfView = _targetFOV;
        _FOVCoroutineActive = false;
    }
}
