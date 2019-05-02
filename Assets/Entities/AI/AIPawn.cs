using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

public class AIPawn : Pawn
{
    public const string PROPERTY_AGGRO = "IsAggrod";

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

    public virtual void GiveAggro(Pawn instigator)
    {
        if (!(instigator || _controller))
        {
            //This is here for cases like environmental hazards, so the AI doesn't get confused and attack a wall
            return;
        }

        _remainingAggroTime = AggroTime;

        bool currentAggro = _controller.localBlackboard.GetProperty<bool>(PROPERTY_AGGRO);
        if (!currentAggro)
        {
            _controller.localBlackboard.SetProperty(PROPERTY_AGGRO, true);
            _controller.InterruptBehavior();
            _controller.localBlackboard.SetProperty("target", instigator);
        }
    }

    public virtual void DeathBehavior(Pawn killer)
    {
        Debug.Log("Ye killed me, " + killer.name);
        Destroy(gameObject);
    }
}
