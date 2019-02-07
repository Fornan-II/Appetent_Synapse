using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item", menuName = "InventoryItem")]
public class InventoryHoldableItem : ScriptableObject
{
    public GameObject EquippedPrefab;
}
