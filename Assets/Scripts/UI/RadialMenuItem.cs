using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuItem : MonoBehaviour
{
    public Image background;
    public Image mask;

    public void SetArcAndIndex(float arcLength, int index)
    {
        background.fillAmount = arcLength / (2.0f * Mathf.PI);
        transform.rotation = Quaternion.Euler(Vector3.forward * (Mathf.Rad2Deg * arcLength * index - 90.0f));
    }
}
