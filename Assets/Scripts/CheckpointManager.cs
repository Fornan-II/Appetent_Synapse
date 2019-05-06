using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    public Checkpoint DefaultCheckpoint;
    public GameObject PlayerPrefab;
    protected Dictionary<PlayerController, Checkpoint> _lastVisitedCheckPoints = new Dictionary<PlayerController, Checkpoint>();

    protected virtual void Awake()
    {
        if (Instance)
        {
            Debug.LogWarning("Multiple CheckpointManagers found in scene.");
        }
        else
        {
            Instance = this;
        }
    }

    public virtual void RegisterCheckPoint(PlayerController player, Checkpoint checkpoint)
    {
        if(_lastVisitedCheckPoints.ContainsKey(player))
        {
            _lastVisitedCheckPoints[player] = checkpoint;
        }
        else
        {
            _lastVisitedCheckPoints.Add(player, checkpoint);
        }
    }

    public virtual void RespawnPlayer(PlayerController player)
    {
        if(_lastVisitedCheckPoints.ContainsKey(player))
        {
            _lastVisitedCheckPoints[player].SpawnPlayer(player);
        }
        else
        {
            DefaultCheckpoint.SpawnPlayer(player);
        }
    }

    public virtual void RespawnPlayer(PlayerController player, float delay)
    {
        StartCoroutine(RespawnPlayerAfterDelay(player, delay));
    }

    protected virtual IEnumerator RespawnPlayerAfterDelay(PlayerController player, float delay)
    {
        yield return new WaitForSeconds(delay);
        RespawnPlayer(player);
    }
}
