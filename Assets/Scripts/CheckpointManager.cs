using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    protected Checkpoint _lastVisitedCheckPoint;

    public virtual void RegisterCheckPoint(Checkpoint checkpoint)
    {
        _lastVisitedCheckPoint = checkpoint;
    }

    public virtual void RespawnPlayer(Pawn player)
    {
        if (_lastVisitedCheckPoint)
        {
            _lastVisitedCheckPoint.SpawnPlayer(player);
        }
    }
}
