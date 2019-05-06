using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    public Image[] Icons;

    public virtual void SetValue(int value)
    {
        for(int i = 0; i < Icons.Length; i++)
        {
            Icons[i].enabled = i < value;
        }
    }
}
