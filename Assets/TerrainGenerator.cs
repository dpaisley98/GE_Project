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

    // The distance from the edge of the generated area at which new chunks are generated
    public float chunkGenerationDistance = 10f;

    // The mesh filter and renderer components
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    // A dictionary to store the generated chunks of terrain
    Dictionary<Vector2Int, Mesh> chunks;

    // The position of the player
    Vector3 playerPosition;

    public GameObject player;

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
        playerPosition = player.transform.position;

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
    Vector3 chunkMin = GetChunkOffset(chunkCoordinates);
    Vector3 chunkMax = chunkMin + Vector3.one * chunkSize;
    if (playerPosition.x > chunkMin.x - chunkGenerationDistance &&
        playerPosition.x < chunkMax.x + chunkGenerationDistance &&
        playerPosition.z > chunkMin.z - chunkGenerationDistance &&
        playerPosition.z < chunkMax.z + chunkGenerationDistance)
    {
        return true;
    }

    return false;
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
        float xOffset = Random.Range(-10000f, 10000);
        float yOffset = Random.Range(-10000f, 10000);
        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                float height = maxHeight * Mathf.PerlinNoise((chunkCoordinates.x + x + xOffset) * noiseScale, (chunkCoordinates.y + z + yOffset) * noiseScale);
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
        chunks.Add(chunkCoordinates, mesh);

        // Set the material color to a random color
        meshRenderer.material.color = Random.ColorHSV();
    }

    Vector3 GetChunkOffset(Vector2Int chunkCoordinates)
    {
        return new Vector3(chunkCoordinates.x * chunkSize, 0, chunkCoordinates.y * chunkSize);
    }

    Vector2Int GetChunkCoordinates(Vector3 position)
    {
        return new Vector2Int(Mathf.FloorToInt(position.x / chunkSize), Mathf.FloorToInt(position.z / chunkSize));
    }

    Vector3 GetChunkCenter(Vector2Int chunkCoordinates)
    {
        return new Vector3(chunkCoordinates.x * chunkSize + chunkSize / 2, 0, chunkCoordinates.y * chunkSize + chunkSize / 2);
    }
}

