using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CollectibleSpawner : NetworkBehaviour
{
    [Header("Prefabs")]
    public GameObject[] collectiblePrefabs;

    [Header("Spawns")]
    public Transform[] spawnPoints;

    private List<GameObject> spawnedObjects =
        new List<GameObject>();

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        SpawnCollectibles();
    }

    public void SpawnCollectibles()
    {
        if (!IsServer) return;

        // limpiar anteriores
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                obj.GetComponent<NetworkObject>()
                    .Despawn(true);
            }
        }

        spawnedObjects.Clear();

        // spawn random
        foreach (Transform point in spawnPoints)
        {
            int randomIndex =
                Random.Range(
                    0,
                    collectiblePrefabs.Length
                );

            GameObject collectible =
                Instantiate(
                    collectiblePrefabs[randomIndex],
                    point.position,
                    Quaternion.identity
                );

            collectible
                .GetComponent<NetworkObject>()
                .Spawn();

            spawnedObjects.Add(collectible);
        }
    }
}
