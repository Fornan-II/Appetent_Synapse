using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public virtual void SpawnPlayer(Pawn player)
    {
        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        CheckpointManager manager = other.GetComponent<CheckpointManager>();
        manager.RegisterCheckPoint(this);
    }
}
