using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_SetTimeScale : MonoBehaviour
{
    public float TimeScale = 1.0f;

    private void OnValidate()
    {
        if(UnityEditor.EditorApplication.isPlaying)
        {
            Time.timeScale = TimeScale;
        }
    }
}
