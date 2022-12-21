using UnityEngine;

public static class MeshCombiner
{
    public static Mesh Combine(GameObject[] objectsToCombine)
    {
        // Create a new mesh
        Mesh combinedMesh = new Mesh();

        // Combine the meshes of the objects into the new mesh
        CombineInstance[] combinedMeshes = new CombineInstance[objectsToCombine.Length];
        for (int i = 0; i < objectsToCombine.Length; i++)
        {
            combinedMeshes[i].mesh = objectsToCombine[i].GetComponent<MeshFilter>().mesh;
            combinedMeshes[i].transform = objectsToCombine[i].transform.localToWorldMatrix;
        }
        combinedMesh.CombineMeshes(combinedMeshes, true, true);

        // Return the combined mesh
        return combinedMesh;
    }
}
