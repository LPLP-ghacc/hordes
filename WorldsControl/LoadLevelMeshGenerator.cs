using System.Collections;
using UnityEngine;

public class LoadLevelMeshGenerator : MonoBehaviour
{
    public int numVertices = 100;
    public int numTriangles = 50;

    private float frames;

    private void Start()
    {
        StartCoroutine(SetCurrentMesh());
    }

    private void Update()
    {


        frames += Time.deltaTime;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, frames * 16, 0));
    }

    private IEnumerator SetCurrentMesh()
    {
        yield return null;

        GetComponent<MeshFilter>().mesh = GenerateRandomMesh(numVertices, numTriangles);
    }

    public Mesh GenerateRandomMesh(int numVertices, int numTriangles)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = GenerateRandomVertices(numVertices);
        mesh.vertices = vertices;

        int[] triangles = GenerateRandomTriangles(numTriangles, numVertices);
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        return mesh;
    }

    private Vector3[] GenerateRandomVertices(int numVertices)
    {
        Vector3[] vertices = new Vector3[numVertices];

        for (int i = 0; i < numVertices; i++)
        {
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);
            float z = Random.Range(-1f, 1f);

            vertices[i] = new Vector3(x, y, z);
        }

        return vertices;
    }

    private int[] GenerateRandomTriangles(int numTriangles, int numVertices)
    {
        int[] triangles = new int[numTriangles * 3];

        for (int i = 0; i < numTriangles; i++)
        {
            int vertexIndex1 = Random.Range(0, numVertices);
            int vertexIndex2 = Random.Range(0, numVertices);
            int vertexIndex3 = Random.Range(0, numVertices);

            triangles[i * 3] = vertexIndex1;
            triangles[i * 3 + 1] = vertexIndex2;
            triangles[i * 3 + 2] = vertexIndex3;
        }

        return triangles;
    }
}
