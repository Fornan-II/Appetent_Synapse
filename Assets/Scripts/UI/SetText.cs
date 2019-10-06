using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetText : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void SetTextValue(int value)
    {
        text.text = value.ToString();
    }
}
