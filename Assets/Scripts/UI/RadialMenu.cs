using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{
    protected bool isBeingHoveredOver = false;
    public RectTransform menuTransform;
    public float minDistanceFromCircle = 10.0f;
    public List<RadialMenuItem> items;

    // Start is called before the first frame update
    void Start()
    {

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
            if (minDistanceFromCircle < distanceFromMouseToCenter && distanceFromMouseToCenter <= radius)
            {
                Vector2 coord = Input.mousePosition - menuTransform.position;
                float angle = Mathf.Atan2(coord.y, coord.x);    //Value between -PI and PI
                Debug.Log("within menu at angle" + angle);

                float itemAngleSize = (2.0f * Mathf.PI) / items.Count;
            }
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

    public void OnPointerDown(BaseEventData data)
    {

    }
}
