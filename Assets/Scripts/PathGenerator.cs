using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    [Header("Path Settings")]
    public int waypointCount = 5;
    
    [Header("Path Shape")]
    public float maxSidewaysOffset = 5f;
    
    [Header("Terrain")]
    public LayerMask terrainLayer;
    public float raycastHeight = 100f;
    public float raycastDistance = 200f;
    
    public TerrainPathModifier terrainPathModifier;
    public MeshFilter terrainMeshFilter;

    public List<GeneratedPath> generatedPaths = new List<GeneratedPath>();
    public PathVisualizer pathVisualizer;
    
    [Header("Obstacle Avoidance")]
    public LayerMask obstacleLayer;
    public float obstacleAvoidanceRadius = 2f;
    public int maxWaypointAttempts = 20;
    

    public void GeneratePaths(
        List<Enemyspawner> spawners,
        MainTower tower)
    {
        generatedPaths.Clear();

        foreach (Enemyspawner spawner in spawners)
        {
            GeneratedPath path =
                GeneratePath(
                    spawner.transform,
                    tower.transform
                );

            generatedPaths.Add(path);
            
            //This is to see a drawn path
            pathVisualizer.GeneratePathMesh(path);
        }

        Debug.Log(
            "Generated " +
            generatedPaths.Count +
            " paths."
        );

        // Now modify terrain
        foreach (GeneratedPath path in generatedPaths)
        {
            terrainPathModifier.RaiseTerrainAlongPath(
                path,
                terrainMeshFilter.mesh
            );
        }
        
        
    }

    GeneratedPath GeneratePath(
        Transform spawner,
        Transform tower)
    {
        GeneratedPath path = new GeneratedPath();

        path.spawner = spawner;
        path.tower = tower;

        Vector3 start = GetTerrainPosition(spawner.position);
        Vector3 end = GetTerrainPosition(tower.position);

        // Starting point
        path.waypoints.Add(start);

        // Get the overall direction from spawner to tower
        Vector3 direction = (end - start).normalized;

        // Calculate a sideways direction
        Vector3 perpendicular = Vector3.Cross(
            direction,
            Vector3.up
        ).normalized;

        for (int i = 1; i <= waypointCount; i++)
        {
            float progress =
                (float)i / (waypointCount + 1);

            Vector3 basePosition = Vector3.Lerp(
                start,
                end,
                progress
            );

            float curveStrength =
                Mathf.Sin(progress * Mathf.PI);

            bool foundValidWaypoint = false;

            Vector3 waypoint = basePosition;

            for (int attempt = 0;
                 attempt < maxWaypointAttempts;
                 attempt++)
            {
                // Generate random sideways movement
                float randomOffset = Random.Range(
                    -maxSidewaysOffset,
                    maxSidewaysOffset
                );

                float sidewaysOffset =
                    randomOffset * curveStrength;

                waypoint =
                    basePosition +
                    perpendicular * sidewaysOffset;

                // Put the waypoint on the terrain
                waypoint =
                    GetTerrainPosition(waypoint);

                // Check for obstacles
                if (IsWaypointSafe(waypoint))
                {
                    foundValidWaypoint = true;
                    break;
                }
            }

            if (foundValidWaypoint)
            {
                path.waypoints.Add(waypoint);
            }
            else
            {
                Debug.LogWarning(
                    "Could not find a safe waypoint after " +
                    maxWaypointAttempts +
                    " attempts."
                );

                // Use the original position as a fallback
                waypoint =
                    GetTerrainPosition(basePosition);

                path.waypoints.Add(waypoint);
            }
        }

        // Ending point
        path.waypoints.Add(end);

        return path;
    }
    
    Vector3 GetTerrainPosition(Vector3 position)
    {
        Vector3 rayStart = new Vector3(
            position.x,
            raycastHeight,
            position.z
        );

        if (Physics.Raycast(
                rayStart,
                Vector3.down,
                out RaycastHit hit,
                raycastDistance,
                terrainLayer))
        {
            return hit.point;
        }

        // If no terrain was found, return the original position
        return position;
    }
    
    bool IsWaypointSafe(Vector3 position)
    {
        return !Physics.CheckSphere(
            position,
            obstacleAvoidanceRadius,
            obstacleLayer
        );
    }
}