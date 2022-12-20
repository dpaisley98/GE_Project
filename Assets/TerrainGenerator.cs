using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    // The size of the terrain
    public int width = 1024;
    public int height = 1024;

    // The maximum height of the terrain
    public float maxHeight = 10f;

    // The mesh filter and renderer components
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    void Start()
    {
        // Get the mesh filter and renderer components
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Generate the terrain
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        // Create a new mesh and set it as the filter's mesh
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        // Create arrays for the vertices and triangles
        Vector3[] vertices = new Vector3[width * height];
        int[] triangles = new int[(width - 1) * (height - 1) * 6];

        // Generate the vertices
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Use Perlin noise to generate the height of the vertex
                float vertexHeight = Mathf.PerlinNoise(x * 0.1f, y * 0.1f) * maxHeight;

                // Set the vertex position
                vertices[x + y * width] = new Vector3(x, vertexHeight, y);
            }
        }

        // Generate the triangles
        int triangleIndex = 0;
        for (int x = 0; x < width - 1; x++)
        {
            for (int y = 0; y < height - 1; y++)
            {
                // Get the indices of the four vertices of the current quad
                int topLeft = x + y * width;
                int topRight = (x + 1) + y * width;
                int bottomLeft = x + (y + 1) * width;
                int bottomRight = (x + 1) + (y + 1) * width;

                // Set the triangle indices
                triangles[triangleIndex] = topLeft;
                triangles[triangleIndex + 1] = bottomLeft;
                triangles[triangleIndex + 2] = topRight;
                triangles[triangleIndex + 3] = topRight;
                triangles[triangleIndex + 4] = bottomLeft;
                triangles[triangleIndex + 5] = bottomRight;

                // Increase the triangle index by 6
                triangleIndex += 6;
            }
        }

        // Set the mesh's vertices and triangles
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Generate a color for each vertex based on its height
        Color[] colors = new Color[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            // Set the color based on the height of the vertex
            float t = Mathf.InverseLerp(0, maxHeight, vertices[i].y);
            colors[i] = Color.Lerp(Color.green, Color.white, t);
        }

        // Assign the colors to the mesh
        mesh.colors = colors;
    }
}



