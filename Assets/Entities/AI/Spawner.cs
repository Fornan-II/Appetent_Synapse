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

    [SerializeField] protected float _delay;
    protected bool runSpawnTimer = false;
    protected Coroutine SpawnCoroutine;

    public virtual void SpawnEntity(GameObject objToSpawn)
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

    protected void LateUpdate()
    {
        if(runSpawnTimer)
        {
            if(_delay <= 0.0f && SpawnCoroutine == null)
            {
                SpawnCoroutine = StartCoroutine(TrySpawning());
                _delay = Random.Range(minSpawnTime, maxSpawnTime);
            }

            _delay -= Time.deltaTime;
        }

        runSpawnTimer = false;
    }
    
    protected virtual void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<PlayerPawn>())
        {
            runSpawnTimer = true;
        }
    }

    protected virtual IEnumerator TrySpawning()
    {
        GameObject objToSpawn = SpawnPool[Random.Range(0, SpawnPool.Length)];

        AIPawn pawn = objToSpawn.GetComponent<AIPawn>();
        int nearbyOfSameTypeFound = 0;
        bool invalidSpawnConditions = true;
        while(invalidSpawnConditions)
        {
            nearbyOfSameTypeFound = 0;
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
            //Debug.Log("Found " + nearbyOfSameTypeFound + " nearby " + pawn.GetType());

            if(nearbyOfSameTypeFound < maxNearbySpawn || maxNearbySpawn <= 0)
            {
                invalidSpawnConditions = false;
            }
            else
            {
                Debug.Log("Invalid spawn conditions");
                yield return null;
            }

            //Debug.Log("invalidSpawnConditions: " + invalidSpawnConditions);
        }

        int numberToSpawn = Random.Range(1, maxGroupSize);
        if(numberToSpawn + nearbyOfSameTypeFound > maxNearbySpawn && maxNearbySpawn > 0)
        {
            numberToSpawn = maxNearbySpawn - nearbyOfSameTypeFound;
        }
        //Debug.Log("Spawning " + numberToSpawn + " " + objToSpawn.name);
        for(int count = 0; count < numberToSpawn; count++)
        {
            SpawnEntity(objToSpawn);
        }

        SpawnCoroutine = null;
    }

    public virtual void Break()
    {
        Destroy(gameObject);
    }
}
