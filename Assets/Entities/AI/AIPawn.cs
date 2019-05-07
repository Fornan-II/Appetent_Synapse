using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

public class AIPawn : Pawn
{
    public const string PROPERTY_AGGRO = "IsAggrod";
    public const string PROPERTY_AGGROSTRENGTH = "AggroStrength";

    protected AIController _controller;

    public AIMoveScript moveScript;

    public float AggroTime = 60.0f;
    [SerializeField]protected float _remainingAggroTime = 0.0f;

    protected virtual void Update()
    {
        if (_controller)
        {
            bool removeAggro = false;

            if (_remainingAggroTime > 0.0f)
            {
                _remainingAggroTime -= Time.deltaTime;

                if (_remainingAggroTime <= 0.0f)
                {
                    removeAggro = true;
                }
            }
            if (!_controller.localBlackboard.GetProperty<Pawn>("target") && _controller.localBlackboard.GetProperty<bool>(PROPERTY_AGGRO))
            {
                removeAggro = true;
            }

            if (removeAggro)
            {
                _controller.localBlackboard.SetProperty(PROPERTY_AGGRO, false);
                _controller.localBlackboard.RemoveProperty("target");
                _controller.InterruptBehavior();
            }
        }
    }

    public virtual void Init(AIController controller)
    {
        _controller = controller;
        _controller.localBlackboard.SetProperty(PROPERTY_AGGRO, false);
    }

    public virtual void OnTakeDamage(Pawn instigator)
    {
        GiveAggro(instigator, 2);
    }

    public virtual void GiveAggro(Pawn instigator, int aggroStrength)
    {
        if (!(instigator || _controller))
        {
            //This is here for cases like environmental hazards, so the AI doesn't get confused and attack a wall
            return;
        }

        _remainingAggroTime = AggroTime;

        bool currentAggro = _controller.localBlackboard.GetProperty<bool>(PROPERTY_AGGRO);
        int currentStrength = _controller.localBlackboard.GetProperty<int>(PROPERTY_AGGROSTRENGTH);
        if (!currentAggro || aggroStrength > currentStrength)
        {
            _controller.localBlackboard.SetProperty(PROPERTY_AGGRO, true);
            _controller.localBlackboard.SetProperty(PROPERTY_AGGROSTRENGTH, aggroStrength);
            _controller.InterruptBehavior();
            _controller.localBlackboard.SetProperty("target", instigator);
        }
    }

    public virtual void DeathBehavior(Pawn killer)
    {
        if(killer)
        {
            Debug.Log("Ye killed me, " + killer.name);
        }

        _controller.ProcessTree = false;

        if (moveScript)
        {
            moveScript.enabled = false;
        }
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb)
        {
            rb.freezeRotation = false;
        }
        DamageReciever dr = GetComponent<DamageReciever>();
        if(dr)
        {
            dr.enabled = false;
        }
        LookAtTarget ls = GetComponent<LookAtTarget>();
        if(ls)
        {
            ls.enabled = false;
        }

        Destroy(gameObject, 3.0f);
    }
}
