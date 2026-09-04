using System.Collections;
using UnityEngine;

public class RandomObjectGen : MonoBehaviour
{
    [Header("Object Settings")]
    public GameObject objectPrefab;
    public int numberToSpawn = 3;

    [Header("Spawn Area")]
    public float xSize = 20f;
    public float zSize = 20f;

    [Header("Terrain")]
    public MeshGen terrain;
    public LayerMask terrainLayer;
    public float raycastHeight = 100f;

    [Header("Safety")]
    public int maxAttempts = 100;

    IEnumerator Start()
    {
        // Wait until the terrain has finished generating
        while (!terrain.IsGenerated)
        {
            yield return null;
        }
        
        SpawnObjects();
    }

    void SpawnObjects()
    {
        int objectsSpawned = 0;
        int attempts = 0;

        while (objectsSpawned < numberToSpawn && attempts < maxAttempts)
        {
            attempts++;

            float randomX = Random.Range(0f, xSize);
            float randomZ = Random.Range(0f, zSize);

            Vector3 rayStart = transform.position + new Vector3(
                randomX,
                raycastHeight,
                randomZ
            );

            RaycastHit hit;

            if (Physics.Raycast(
                rayStart,
                Vector3.down,
                out hit,
                raycastHeight * 2f,
                terrainLayer))
            {
                Instantiate(
                    objectPrefab,
                    hit.point,
                    Quaternion.identity
                );

                objectsSpawned++;
            }
        }

        Debug.Log(
            "Spawned " + objectsSpawned +
            " / " + numberToSpawn + " objects."
        );
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 center = transform.position + new Vector3(
            xSize / 2f,
            0f,
            zSize / 2f
        );

        Vector3 size = new Vector3(
            xSize,
            0.1f,
            zSize
        );

        Gizmos.DrawWireCube(center, size);
    }
    
    
}