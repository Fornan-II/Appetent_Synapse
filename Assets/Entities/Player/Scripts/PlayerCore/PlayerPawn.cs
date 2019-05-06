using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class PlayerPawn : Pawn
{
    public MoveScript MyMoveScript;
    public LookScript MyLookScript;
    public Camera MyCamera;
    public Inventory MyInventory;
    public Interacter MyInteracter;
    public EnergyManager MyEnergyManager;
    public int OnKillEnergyIncrease = 2;

    public PlayerController Controller
    {
        get;
        protected set;
    }

    public virtual void OnStartControlled(PlayerController controller)
    {
        Controller = controller;
    }

    public virtual void OnStopControlled(PlayerController controller)
    {
        Controller = null;
    }

    #region Input
    public virtual void PassLockScreen(bool value)
    {
        if (MyLookScript)
        {
            MyLookScript.lockState = value;
        }
    }

    public virtual void PassMoveInput(Vector2 value)
    {
        if(!MyMoveScript)
        {
            Debug.LogWarning(name + " is trying to be passed movement input when it has no MoveScript component assigned!");
            return;
        }

        MyMoveScript.MoveHorizontal(value.x);
        MyMoveScript.MoveVertical(value.y);
    }

    public virtual void PassLookInput(Vector2 value)
    {
        if(!MyLookScript)
        {
            Debug.LogWarning(name + " is trying to be passed look input when it has no LookScript component assigned!");
            return;
        }

        MyLookScript.MouseInput = value;
    }

    public virtual void PassPrimaryActionInput(float value)
    {
        if (!MyInventory)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no Inventory component assigned!");
            return;
        }

        MyInventory.UseEquippedPrimary(value >= 0.5f, this);
    }

    public virtual void PassSecondaryActionInput(float value)
    {
        if (!MyInventory)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no Inventory component assigned!");
            return;
        }
        MyInventory.UseEquippedSecondary(value >= 0.5f, this);
    }

    public virtual void PassDPadInput(Vector2 value)
    {
        if(!MyInventory)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no Inventory component assigned!");
            return;
        }
        
        MyInventory.ScrollThroughItems(value.x);
    }
    
    public virtual void PassInteractInput(bool value)
    {
        if(!MyInteracter)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no Interacter component assigned!");
            return;
        }

        if(value)
        {
            Debug.Log("interact");
            MyInteracter.TryToInteract(this);
        }
    }

    public virtual void PassJumpInput(bool value)
    {
        if (!MyMoveScript)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no MoveScript component assigned!");
            return;
        }

        MyMoveScript.Jump(value);
    }

    public virtual void PassSprintInput(bool value)
    {
        if (!MyMoveScript)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no MoveScript component assigned!");
            return;
        }

        MyMoveScript.Sprint(value);
    }

    public virtual void PassCrouchInput(bool value)
    {
        if (!MyMoveScript)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no MoveScript component assigned!");
            return;
        }
        
        MyMoveScript.Crouch(value);
    }
    #endregion

    public override void OnKill(DamageReciever victim)
    {
        MyEnergyManager.AddEnergy(OnKillEnergyIncrease);
    }

    public virtual void OnDeath(Pawn killer)
    {
        MyMoveScript.enabled = false;
        MyLookScript.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb)
        {
            rb.freezeRotation = false;
            rb.useGravity = true;
        }

        Collider col = GetComponent<Collider>();
        if(col)
        {
            col.material = null;
        }
        Destroy(gameObject, 7.0f);
        CheckpointManager.Instance.RespawnPlayer(Controller, 5.0f);
    }
}
