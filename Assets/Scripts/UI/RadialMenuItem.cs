using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuItem : MonoBehaviour
{
    public Image background;
    public Image icon;
    public RectTransform iconPivot;
    public Animator anim;

    [SerializeField]protected IRadialSelectable _representedItem;
    public IRadialSelectable RepresentedItem
    {
        get { return _representedItem; }
        set
        {
            _representedItem = value;
            if(_representedItem != null)
            {
                SetIcon(_representedItem.GetIcon());
            }
        }
    }

    protected bool _isSelected = false;
    public bool IsSelected
    {
        get { return _isSelected; }
        set
        {
            _isSelected = value;
            if(anim)
            {
                anim.SetBool("IsSelected", value);
            }
        }
    }

    protected virtual void Start()
    {
        if (anim)
        {
            anim.SetBool("IsSelected", _isSelected);
        }
    }

    protected virtual void OnEnable()
    {
        if (anim)
        {
            anim.SetBool("IsSelected", _isSelected);
        }
    }

    public void SetIcon(Sprite newIcon)
    {
        if(icon)
        {
            icon.sprite = newIcon;
        }
    }

    public void SetArcAndIndex(float arcLength, int index)
    {
        background.fillAmount = arcLength / (2.0f * Mathf.PI);
        transform.rotation = Quaternion.Euler(Vector3.forward * (Mathf.Rad2Deg * arcLength * index - 90.0f));

        if(iconPivot)
        {
            iconPivot.transform.localRotation = Quaternion.Euler(Vector3.forward * Mathf.Rad2Deg * arcLength * 0.5f);
            if(icon)
            {
                icon.transform.rotation = Quaternion.identity;
            }
        }
    }
}
