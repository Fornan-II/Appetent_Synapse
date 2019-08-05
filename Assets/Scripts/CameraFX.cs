using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFX : MonoBehaviour
{
    [SerializeField]protected Camera _camera;

    [SerializeField] protected Modifier _FOV = new Modifier(80.0f, Modifier.CalculateMode.ADD);
    public Modifier FOV
    {
        get
        {
            return _FOV;
        }
        set
        {
            Debug.Log("Setting " + value.Value);
            _FOV = value;
            UpdateFOV();
            _FOV.OnValueChanged += UpdateFOV;
        }
    }
    public float FOVBlendTime = 0.3f;

    protected Coroutine FOVBlendRoutine;

    protected virtual void Awake()
    {
        _FOV.OnValueChanged += UpdateFOV;
    }

    protected virtual void UpdateFOV()
    {
        if (_camera)
        {
            if(FOVBlendRoutine != null)
            {
                StopCoroutine(FOVBlendRoutine);
            }

            FOVBlendRoutine = StartCoroutine(BlendFOV());
        }
        else
        {
            Debug.LogError("Could not properly assign camera FOV, _camera is not assigned!");
        }
    }

    protected IEnumerator BlendFOV()
    {
        AnimationCurve blend = AnimationCurve.EaseInOut(0.0f, _camera.fieldOfView, FOVBlendTime, _FOV.Value);
        for(float t = 0.0f; t < FOVBlendTime; t += Time.deltaTime)
        {
            _camera.fieldOfView = blend.Evaluate(t);
            yield return null;
        }
        _camera.fieldOfView = _FOV.Value;
        FOVBlendRoutine = null;
    }

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        if(!_camera)
        {
            _camera = GetComponent<Camera>();
        }

        _camera.fieldOfView = _FOV.Value;
    }
#endif
}
