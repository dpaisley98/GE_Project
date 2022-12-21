using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    // The size of each chunk of terrain
    public int chunkSize = 256;

    // The maximum height of the terrain
    public float maxHeight = 10f;

    // The distance from the edge of the generated area at which new chunks are generated
    public float chunkGenerationDistance = 10f;

    // The mesh filter and renderer components
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    // A dictionary to store the generated chunks of terrain
    Dictionary<Vector2Int, Mesh> chunks;

    // The position of the player
    Vector3 playerPosition;

    void Start()
    {
        // Get the mesh filter and renderer components
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Initialize the dictionary
        chunks = new Dictionary<Vector2Int, Mesh>();

        // Generate the initial chunks of terrain
        GenerateChunksAroundPoint(Vector2Int.zero);
    }

    void Update()
    {
        // Update the player's position
        playerPosition = Camera.main.transform.position;

        // Check if new chunks need to be generated
        Vector2Int currentChunk = GetChunkCoordinates(playerPosition);
        if (NeedsChunkGeneration(currentChunk))
        {
            GenerateChunksAroundPoint(currentChunk);
        }
    }

    bool NeedsChunkGeneration(Vector2Int chunkCoordinates)
    {
        // Check if the chunk at the given coordinates has already been generated
        if (chunks.ContainsKey(chunkCoordinates))
        {
            return false;
        }

        // Check if the distance from the player to the chunk is within the chunk generation distance
        Vector3 chunkCenter = GetChunkCenter(chunkCoordinates);
        float distance = Vector3.Distance(chunkCenter, playerPosition);
        if (distance > chunkGenerationDistance)
        {
            return false;
        }

        return true;
    }
    
    void GenerateChunksAroundPoint(Vector2Int chunkCoordinates)
    {
        // Generate the chunk at the given coordinates
        GenerateChunk(chunkCoordinates);

        // Generate the chunks to the left, right, top, and bottom of the given chunk
        GenerateChunk(chunkCoordinates + Vector2Int.left);
        GenerateChunk(chunkCoordinates + Vector2Int.right);
        GenerateChunk(chunkCoordinates + Vector2Int.up);
        GenerateChunk(chunkCoordinates + Vector2Int.down);
    }

    void GenerateChunk(Vector2Int chunkCoordinates)
    {
        // Check if the chunk has already been generated
        if (chunks.ContainsKey(chunkCoordinates))
        {
            return;
        }

        // Create a new mesh for the chunk
        Mesh mesh = new Mesh();

        // Generate the vertices and triangles for the mesh
        Vector3[] vertices = new Vector3[chunkSize * chunkSize];
        int[] triangles = new int[(chunkSize - 1) * (chunkSize - 1) * 6];
        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {                
            float height = maxHeight * Mathf.PerlinNoise((chunkCoordinates.x + x) * 0.1f, (chunkCoordinates.y + z) * 0.1f);
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

        // Recalculate the normals and bounds of the mesh
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        MeshCollider collider = GetComponent<MeshCollider>();
        collider.sharedMesh = mesh;

        // Add the mesh to the dictionary and set it as the mesh for the game object
        meshFilter.mesh = mesh;
        chunks.Add(chunkCoordinates, mesh);

    }

    Vector3 GetChunkOffset(Vector2Int chunkCoordinates)
    {
        return new Vector3(chunkCoordinates.x * chunkSize, 0, chunkCoordinates.y * chunkSize);
    }

    Vector3 GetChunkCenter(Vector2Int chunkCoordinates)
    {
        return GetChunkOffset(chunkCoordinates) + new Vector3(chunkSize / 2, 0, chunkSize / 2);
    }

    Vector2Int GetChunkCoordinates(Vector3 position)
    {
        return new Vector2Int(Mathf.FloorToInt(position.x / chunkSize), Mathf.FloorToInt(position.z / chunkSize));
    }
}



       
