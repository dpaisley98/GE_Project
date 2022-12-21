using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    // The size of each chunk of terrain
    public int chunkSize = 256;

    // The maximum height of the terrain
    public float maxHeight = 10f;

    // The scale of the noise function
    public float noiseScale = 0.1f;

    public float heightIncrease;

    // The mesh filter and renderer components
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;



    void Start()
    {
        // Get the mesh filter and renderer components
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Generate the initial chunks of terrain
        GenerateChunk(Vector2Int.zero);
    }

void GenerateChunk(Vector2Int chunkCoordinates)
{
    // Create a new mesh for the chunk
    Mesh mesh = new Mesh();

    // Generate the vertices and triangles for the mesh
    Vector3[] vertices = new Vector3[chunkSize * chunkSize];
    int[] triangles = new int[(chunkSize - 1) * (chunkSize - 1) * 6];
    float xOffset = Random.Range(-10000f, 10000);
    float yOffset = Random.Range(-10000f, 10000);
    noiseScale = Random.Range(0.1f, 0.15f);
    for (int x = 0; x < chunkSize; x++)
    {
        for (int z = 0; z < chunkSize; z++)
        {
            // Calculate the height of the vertex
            float height = maxHeight * Mathf.PerlinNoise((chunkCoordinates.x + x + xOffset) * noiseScale, (chunkCoordinates.y + z + yOffset) * noiseScale);
            
            // Add a mountain-like feature at the boundary of the chunk
            if (x == 0 || x == chunkSize - 1 || z == 0 || z == chunkSize - 1)
            {
                // Calculate the distance from the center of the chunk
                float distanceFromCenter = Mathf.Sqrt(Mathf.Pow(x - chunkSize / 2, 2) + Mathf.Pow(z - chunkSize / 2, 2));

                // Add a value that increases with the distance from the center
                height += maxHeight * Mathf.Clamp((distanceFromCenter - chunkSize / 4) / (chunkSize / 4), 0, 1) * heightIncrease;
            }
            
            vertices[x + z * chunkSize] = new Vector3(x, height, z) + GetChunkOffset(chunkCoordinates);

            // Calculate the triangle indices
            if (x < chunkSize - 1 && z < chunkSize - 1)
            {
                int triangleIndex = (x + z * (chunkSize - 1)) * 6;
                triangles[triangleIndex + 0] = x + z * chunkSize;
                triangles[triangleIndex + 1] = x + (z + 1) * chunkSize;
                triangles[triangleIndex + 2] = x + 1 + z * chunkSize;

                triangles[triangleIndex + 3] = x + 1 + z * chunkSize;
                triangles[triangleIndex + 4] = x + (z + 1) * chunkSize;
                triangles[triangleIndex + 5] = x + 1 + (z + 1) * chunkSize;
            }
        }
    }

    // Set the mesh data
    mesh.vertices = vertices;
    mesh.triangles = triangles;
    mesh.RecalculateNormals();

    MeshCollider collider = GetComponent<MeshCollider>();
    collider.sharedMesh = mesh;

    // Set the mesh filter's mesh and add the chunk to the dictionary
    meshFilter.mesh = mesh;

    Color[] colors = new Color[vertices.Length];
    Color bottom = Random.ColorHSV();
    Color top = Random.ColorHSV();
    for (int i = 0; i < vertices.Length; i++)
    {
        // Set the color based on the height of the vertex
        float t = Mathf.InverseLerp(0, maxHeight, vertices[i].y);
        colors[i] = Color.Lerp(bottom, top, t);
    }

    // Assign the colors to the mesh
    mesh.colors = colors;

    // Set the material color to a random color
    //meshRenderer.material.color = Random.ColorHSV();
}

    Vector3 GetChunkOffset(Vector2Int chunkCoordinates)
    {
        return new Vector3(chunkCoordinates.x * chunkSize, 0, chunkCoordinates.y * chunkSize);
    }
}

