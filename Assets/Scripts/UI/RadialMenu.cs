using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{
    protected bool isBeingHoveredOver = false;
    public RectTransform menuTransform;
    [Range(0.0f,1.0f)]
    public float deadzonePercent = 0.2f;
    [SerializeField]protected List<RadialMenuItem> _items;
    protected RadialMenuItem selectedItem;

    // Start is called before the first frame update
    void Start()
    {
        UpdateMenuItems();
    }

    // Update is called once per frame
    void Update()
    {
        if(isBeingHoveredOver && menuTransform)
        {
            //Debug.Log("MousePos: " + Input.mousePosition + "\nRectWidth: " + menuTransform.rect.width + "\nRectCenter: " + menuTransform.position);
            //Determine if the cursor is inside the circle part of the menu
            //Debug.Log("Distance: " + Vector3.Distance(Input.mousePosition, menuTransform.position) + "\nMaxDist: " + (menuTransform.rect.width * 0.5f));
            float distanceFromMouseToCenter = Vector3.Distance(Input.mousePosition, menuTransform.position);
            float radius = (menuTransform.rect.width * 0.5f);
            if (deadzonePercent * radius < distanceFromMouseToCenter && distanceFromMouseToCenter <= radius)
            {
                Vector2 coord = Input.mousePosition - menuTransform.position;
                float angle = Mathf.Atan2(coord.y, coord.x) + Mathf.PI;    //Value between 0 and 2PI. Counterclockwise from 9 o' Clock.

                float itemAngleSize = (2.0f * Mathf.PI) / _items.Count;
                int itemIndex = Mathf.Min(Mathf.FloorToInt(angle / itemAngleSize), _items.Count - 1);
                Debug.Log("Selected Item: " + itemIndex);
            }
        }
    }

    public void AddItem(RadialMenuItem item)
    {
        _items.Add(item);
        UpdateMenuItems();
    }
    public void RemoveItem(RadialMenuItem item)
    {
        _items.Remove(item);
        UpdateMenuItems();
    }
    public void SetItems(RadialMenuItem[] items)
    {
        _items = new List<RadialMenuItem>(items);
        UpdateMenuItems();
    }
    public void SetItems(List<RadialMenuItem> items)
    {
        _items = items;
        UpdateMenuItems();

    }

    public void UpdateMenuItems()
    {
        float itemAngleSize = (2.0f * Mathf.PI) / _items.Count;
        for(int i = 0; i < _items.Count; i++)
        {
            _items[i].SetArcAndIndex(itemAngleSize, i);
        }
    }

    public void OnPointerEnter(BaseEventData data)
    {
        isBeingHoveredOver = true;
    }

    public void OnPointerExit(BaseEventData data)
    {
        isBeingHoveredOver = false;
    }
}
