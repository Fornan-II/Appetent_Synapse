using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Augment", menuName = "Augment")]
public class Augment : ScriptableObject, IRadialSelectable
{
    public bool IsActive = false;
    [SerializeField] protected Sprite _radialIcon;

    public Sprite GetIcon()
    {
        return _radialIcon;
    }

    public void Select(bool value)
    {
        IsActive = value;
    }

    public void Use(PlayerPawn user)
    {

    }
}