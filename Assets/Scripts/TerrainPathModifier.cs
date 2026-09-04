using UnityEngine;

public class TerrainPathModifier : MonoBehaviour
{
    [Header("Path Settings")]
    public float terrainRaiseWidth = 3f;
    public float pathRaiseAmount = 1f;

    public bool digPath = false;

    public void RaiseTerrainAlongPath(
        GeneratedPath path,
        Mesh terrainMesh)
    {
        Vector3[] vertices = terrainMesh.vertices;

        // Convert path waypoints from WORLD space
        // to TERRAIN LOCAL space
        Vector3[] localWaypoints =
            new Vector3[path.waypoints.Count];

        for (int i = 0; i < path.waypoints.Count; i++)
        {
            localWaypoints[i] =
                transform.InverseTransformPoint(
                    path.waypoints[i]
                );
        }

        // Check every terrain vertex
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];

            float closestDistance = float.MaxValue;

            // Check every path segment
            for (int j = 0; j < localWaypoints.Length - 1; j++)
            {
                float distance =
                    DistanceToLineSegmentXZ(
                        vertex,
                        localWaypoints[j],
                        localWaypoints[j + 1]
                    );

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                }
            }

            // Raise terrain if close enough to path
            if (closestDistance <= terrainRaiseWidth)
            {
                float strength =
                    1f -
                    (closestDistance / terrainRaiseWidth);

                float heightChange = pathRaiseAmount * strength;

                // If digPath is enabled, invert the height change
                if (digPath)
                {
                    heightChange *= -1f;
                }

                vertices[i].y += heightChange;
            }
        }

        terrainMesh.vertices = vertices;

        terrainMesh.RecalculateNormals();
        terrainMesh.RecalculateBounds();

        // Update collider
        MeshCollider meshCollider =
            GetComponent<MeshCollider>();

        if (meshCollider != null)
        {
            meshCollider.sharedMesh = null;
            meshCollider.sharedMesh = terrainMesh;
        }
    }

    float DistanceToLineSegmentXZ(
        Vector3 point,
        Vector3 start,
        Vector3 end)
    {
        Vector2 pointXZ =
            new Vector2(point.x, point.z);

        Vector2 startXZ =
            new Vector2(start.x, start.z);

        Vector2 endXZ =
            new Vector2(end.x, end.z);

        Vector2 direction =
            endXZ - startXZ;

        float lengthSquared =
            direction.sqrMagnitude;

        if (lengthSquared == 0f)
        {
            return Vector2.Distance(
                pointXZ,
                startXZ
            );
        }

        float t =
            Vector2.Dot(
                pointXZ - startXZ,
                direction
            ) / lengthSquared;

        t = Mathf.Clamp01(t);

        Vector2 closestPoint =
            startXZ + direction * t;

        return Vector2.Distance(
            pointXZ,
            closestPoint
        );
    }
}