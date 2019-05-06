using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] SpawnPool;
    public float minSpawnDistance;
    public float maxSpawnDistance;
    public int maxGroupSize;
    public float minSpawnTime;
    public float maxSpawnTime;
    public int maxNearbySpawn;

    protected Coroutine SpawnCoroutine;

    public virtual void Spawn(GameObject objToSpawn)
    {
        Vector2 spawnPlanarPosition = Random.insideUnitCircle * (maxSpawnDistance - minSpawnDistance);
        spawnPlanarPosition += spawnPlanarPosition.normalized * minSpawnDistance;
        Vector3 spawnPosition = new Vector3(spawnPlanarPosition.x, 1.0f, spawnPlanarPosition.y) + transform.position;

        Vector3 spawnRotation = Vector3.up * Random.Range(0, 360);

        //Debug.DrawLine(spawnPosition - Vector3.up * 1.0f, spawnPosition + Vector3.up * 1.0f);
        //UnityEditor.EditorApplication.isPaused = true;

        if(!Physics.CheckCapsule(spawnPosition - Vector3.up * 0.5f, spawnPosition + Vector3.up * 0.5f, 0.5f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            Instantiate(objToSpawn, spawnPosition, Quaternion.Euler(spawnRotation));
        }
        else
        {
            Debug.Log("Could not spawn - position volume occupied");
        }
    }

    protected virtual void FixedUpdate()
    {
        if(SpawnCoroutine == null)
        {
            SpawnCoroutine = StartCoroutine(SpawnAfterDelay());
        }
    }

    protected virtual IEnumerator SpawnAfterDelay()
    {
        GameObject objToSpawn = SpawnPool[Random.Range(0, SpawnPool.Length)];
        float delay = Random.Range(minSpawnTime, maxSpawnTime);

        Debug.Log("Spawning in " + delay + " seconds");
        yield return new WaitForSeconds(delay);

        AIPawn pawn = objToSpawn.GetComponent<AIPawn>();
        int nearbyOfSameTypeFound = 0;
        bool invalidSpawnConditions = true;
        while(invalidSpawnConditions)
        {
            Collider[] foundColliders = Physics.OverlapSphere(transform.position, maxSpawnDistance * 2);
            foreach(Collider col in foundColliders)
            {
                AIPawn foundPawn = col.GetComponent<AIPawn>();
                if(foundPawn)
                {
                    if(foundPawn.GetType() == pawn.GetType())
                    {
                        nearbyOfSameTypeFound++;
                    }
                }
            }

            if(nearbyOfSameTypeFound < maxNearbySpawn || maxNearbySpawn <= 0)
            {
                invalidSpawnConditions = false;
            }
            else
            {
                Debug.Log("Invalid spawn conditions");
                yield return null;
            }

            Debug.Log("invalidSpawnConditions: " + invalidSpawnConditions);
        }

        int numberToSpawn = Random.Range(1, maxGroupSize);
        if(numberToSpawn + nearbyOfSameTypeFound > maxNearbySpawn && maxNearbySpawn > 0)
        {
            numberToSpawn = maxNearbySpawn - nearbyOfSameTypeFound;
        }
        Debug.Log("Spawning " + numberToSpawn + " " + objToSpawn.name);
        for(int count = 0; count < numberToSpawn; count++)
        {
            Spawn(objToSpawn);
        }

        SpawnCoroutine = null;
    }

    public virtual void Break()
    {
        Destroy(gameObject);
    }
}
