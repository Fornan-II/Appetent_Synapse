using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public virtual void SpawnPlayer(PlayerController player)
    {
        GameObject spawnedPlayer = Instantiate(CheckpointManager.Instance.PlayerPrefab, transform.position, transform.rotation);

        PlayerPawn playerPawn = spawnedPlayer.GetComponent<PlayerPawn>();
        if(playerPawn)
        {
            player.ControlledPawn = playerPawn;
        }
        else
        {
            Debug.LogWarning("Spawned player is missing PlayerPawn component");
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        PlayerPawn pawn = other.GetComponent<PlayerPawn>();
        if (pawn)
        {
            if(pawn.Controller)
            {
                CheckpointManager.Instance.RegisterCheckPoint(pawn.Controller, this);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position - Vector3.right * 0.25f, Vector3.right * 0.5f, Color.green);
        Debug.DrawRay(transform.position - Vector3.forward * 0.25f, Vector3.forward * 0.5f, Color.green);
    }
#endif
}
