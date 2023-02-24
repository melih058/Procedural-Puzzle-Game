using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{
    private Node[] _nodes;
    private MeshFilter _meshFilter;
    private Renderer _renderer;
    private Vector3[] _vertices;

    public void onInitialize(Node[] nodes)
    {
        _nodes = nodes;
        _meshFilter = GetComponent<MeshFilter>();
        _renderer = GetComponent<Renderer>();

        setPivot(_nodes);

        _vertices = new Vector3[3];
        _vertices[0] = transform.InverseTransformPoint(_nodes[0].transform.position);
        _vertices[1] = transform.InverseTransformPoint(_nodes[1].transform.position);
        _vertices[2] = transform.InverseTransformPoint(_nodes[2].transform.position);

        int[] triangles = new int[3];
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        Mesh mesh = new Mesh();
        mesh.vertices = _vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        _meshFilter.mesh = mesh;

        gameObject.AddComponent<MeshCollider>();

    }

    private void setPivot(Node[] nodes)
    {
        Vector3 averagePosition = Vector3.zero;
        for (int i = 0; i < nodes.Length; i++)
        {
            averagePosition += nodes[i].transform.position;
        }

        averagePosition /= 3f;

        transform.position = averagePosition;
    }

    public void changeColor(Color color)
    {
        _renderer.material.SetColor("_Color", color);
    }

    public bool isNeighbourOfTriangle(Triangle triangle)
    {
        int neighbourNodeCount = 0;
        Node[] targetNodes = triangle.nodes;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (targetNodes[j] == _nodes[i])
                {
                    neighbourNodeCount++;
                }
            }
        }

        if (neighbourNodeCount > 1)
            return true;
        else
            return false;
    }

    public Node[] nodes => _nodes;
    public Vector3[] vertices => _vertices;

}
