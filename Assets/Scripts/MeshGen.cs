using System.Collections;
using UnityEngine;

public class MeshGen : MonoBehaviour
{
    private Mesh mesh;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    
    public bool IsGenerated { get; private set; }

    public int xSize = 20;
    public int zSize = 20;
    
    public float scale = 0.3f;
    public float height = 2f;
    
    public int seed;
    public bool randomizeSeed = true;

    private float seedOffsetX;
    private float seedOffsetZ;
    
    Vector3[] vertices;
    int[] triangles;

    void Start()
    {
        if (randomizeSeed)
        {
            seed = Random.Range(0, 100000);
        }
        
        System.Random random = new System.Random(seed);

        seedOffsetX = random.Next(-10000, 10000);
        seedOffsetZ = random.Next(-10000, 10000);

        mesh = new Mesh();

        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();

        GetComponent<MeshFilter>().mesh = mesh;

        CreateMesh();
        UpdateMesh();

        IsGenerated = true;
    }

    private void Update()
    {
        //CreateMesh();
        //UpdateMesh();
    }

    void CreateMesh()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise((x + seedOffsetX) * scale, (z + seedOffsetZ) * scale) * height;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }
        
        triangles = new int[xSize * zSize * 6];
        
        int vert = 0;
        int tris = 0;
        
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        mesh.RecalculateNormals();

        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = mesh;

        //meshRenderer.material.mainTexture
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

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

    // private void OnDrawGizmos()
    // {
    //     if  (vertices == null)
    //         return;
    //     
    //     for (int i = 0; i < vertices.Length; i++)
    //     {
    //         Gizmos.DrawSphere(vertices[i], 0.1f);
    //     }
    // }
}
