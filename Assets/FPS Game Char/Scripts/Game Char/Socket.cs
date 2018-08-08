using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socket : MonoBehaviour {

    //Useful variables to have as read only.
    public virtual bool HasItem { get { return _equippedItem; } }
    public virtual Item EquippedItem { get { return _equippedItem; } }

    protected Item _equippedItem;
    protected Collider _itemCol;
    protected Rigidbody _itemRB;

    protected virtual void Update()
    {
        //This makes sure the object's transform/rotation is in line with the hand. There is likely a better way to do  this. Maybe revisit.
        if (HasItem)
        {
            _equippedItem.transform.position = transform.position;
            _equippedItem.transform.rotation = transform.rotation;
        }
    }

    public virtual bool Equip(Item item)
    {
        //If something is already equipped or non-item is attempting to be equipped, nothing needs to happen.
        if (HasItem || !item)
        {
            return false;
        }

        //Equip the item
        _equippedItem = item;

        //Check for and disable collision collider.
        Collider[] colliders = item.GetComponents<Collider>();
        foreach(Collider col in colliders)
        {
            if(!col.isTrigger)
            {
                //Record the item's collision collider so it can be re-enabled later.
                _itemCol = col;
            }
        }
        if(_itemCol)
        {
            _itemCol.enabled = false;
        }

        //Check for and disable rigidbody physics
        //Record the item's rigidbody so it can be re-enabled later.
        _itemRB = item.GetComponent<Rigidbody>();
        if(_itemRB)
        {
            _itemRB.detectCollisions = false;
            _itemRB.isKinematic = true;
        }

        //Return true because an item has successfully been equipped.
        return true;
    }

    public virtual bool Unequip()
    {
        //If there's nothing to unequip, there's no work to do.
        if(!HasItem)
        {
            return false;
        }

        //Tell item it's not being held and unequip it.
        _equippedItem.SetNotBeingHeld();
        _equippedItem = null;

        //Re-enable collision collider if it exists
        if (_itemCol)
        {
            _itemCol.enabled = true;
        }
        _itemCol = null;

        //Re-enable rigidbody if it exists
        if (_itemRB)
        {
            _itemRB.detectCollisions = true;
            _itemRB.isKinematic = false;
        }

        return true;
    }

    //Accessible function to use an item, with info on who's using it.
    public virtual bool UseItem(Actor user)
    {
        if(!HasItem)
        {
            return false;
        }

        return _equippedItem.Use(user);
    }
}
