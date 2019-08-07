using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{
    protected bool isBeingHoveredOver = false;
    public RectTransform menuTransform;
    public RectTransform indicatorTransform;
    [Range(0.0f,1.0f)]
    public float deadzonePercent = 0.2f;
    [SerializeField]protected List<RadialMenuItem> _items;
    protected RadialMenuItem selectedItem;

    public GameObject radialMenuItemPrefab;

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
            float distanceFromMouseToCenter = Vector3.Distance(Input.mousePosition, menuTransform.position);
            float radius = (menuTransform.rect.width * 0.5f);
            if (deadzonePercent * radius < distanceFromMouseToCenter && distanceFromMouseToCenter <= radius)
            {
                Vector2 coord = Input.mousePosition - menuTransform.position;
                float angle = Mathf.Atan2(coord.y, coord.x) + Mathf.PI;    //Value between 0 and 2PI. Counterclockwise from 9 o' Clock.

                if(indicatorTransform)
                {
                    indicatorTransform.gameObject.SetActive(true);
                    indicatorTransform.rotation = Quaternion.Euler(Vector3.forward * (Mathf.Rad2Deg * angle - 90.0f));
                }

                float itemAngleSize = (2.0f * Mathf.PI) / _items.Count;
                int itemIndex = Mathf.Min(Mathf.FloorToInt(angle / itemAngleSize), _items.Count - 1);

                if(selectedItem)
                {
                    selectedItem.IsSelected = false;
                }
                selectedItem = _items[itemIndex];
                selectedItem.IsSelected = true;
            }
            else if (indicatorTransform.gameObject.activeSelf)
            {
                indicatorTransform.gameObject.SetActive(false);
            }
        }
    }

    public void AddItem(IRadialSelectable item)
    {
        GameObject newItem = Instantiate(radialMenuItemPrefab, transform);
        RadialMenuItem newRadialItem = newItem.GetComponent<RadialMenuItem>();
        newRadialItem.RepresentedItem = item;
        _items.Add(newRadialItem);
        UpdateMenuItems();
    }
    public void RemoveItem(IRadialSelectable item)
    {
        bool searchingForItem = true;
        for(int i = 0; i < _items.Count && searchingForItem; i++)
        {
            if(_items[i].RepresentedItem == item)
            {
                Destroy(_items[i].gameObject);
                _items.RemoveAt(i);
                searchingForItem = false;
            }
        }
        UpdateMenuItems();
    }
    public void SetItems(IRadialSelectable[] items)
    {
        for(int i = 0; i < items.Length || i < _items.Count; i++)
        {
            if(i < items.Length && i < _items.Count)
            {
                _items[i].RepresentedItem = items[i];
            }
            else if(i >= _items.Count)
            {
                AddItem(items[i]);
            }
            else if(i >= items.Length)
            {
                Destroy(_items[i].gameObject);
                _items.RemoveAt(i);
                i--;
            }
        }

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
