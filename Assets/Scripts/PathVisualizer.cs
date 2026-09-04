using System.Collections.Generic;
using UnityEngine;

public class PathVisualizer : MonoBehaviour
{
    public float pathWidth = 2f;
    public float pathHeight = 0.05f;
    
    [Header("Terrain Projection")]
    public LayerMask terrainLayer;
    public float raycastHeight = 100f;
    public float raycastDistance = 200f;
    public float surfaceOffset = 0.05f;

    public Material pathMaterial;

    public void GeneratePathMesh(GeneratedPath path)
    {
        GameObject pathObject = new GameObject("Generated Path");

        MeshFilter meshFilter = pathObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = pathObject.AddComponent<MeshRenderer>();

        meshRenderer.material = pathMaterial;

        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        List<Vector3> waypoints = path.waypoints;

        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            Vector3 current = waypoints[i];
            Vector3 next = waypoints[i + 1];

            Vector3 direction = (next - current).normalized;

            Vector3 perpendicular = Vector3.Cross(
                direction,
                Vector3.up
            ).normalized;

            Vector3 left = current + perpendicular * pathWidth / 2f;
            Vector3 right = current - perpendicular * pathWidth / 2f;

            Vector3 nextLeft = next + perpendicular * pathWidth / 2f;
            Vector3 nextRight = next - perpendicular * pathWidth / 2f;
            
            // Projection
            left = ProjectToTerrain(left);
            right = ProjectToTerrain(right);

            nextLeft = ProjectToTerrain(nextLeft);
            nextRight = ProjectToTerrain(nextRight);

            int startIndex = vertices.Count;

            vertices.Add(left);
            vertices.Add(right);
            vertices.Add(nextLeft);
            vertices.Add(nextRight);

            triangles.Add(startIndex + 0);
            triangles.Add(startIndex + 2);
            triangles.Add(startIndex + 1);

            triangles.Add(startIndex + 2);
            triangles.Add(startIndex + 3);
            triangles.Add(startIndex + 1);
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }
    
    Vector3 ProjectToTerrain(Vector3 position)
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
            return hit.point + Vector3.up * surfaceOffset;
        }

        return position;
    }
}