using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Color _color;
    private List<Triangle> _triangles;
    [SerializeField] private InnerHandle _innerHandlePrefab;
    private List<InnerHandle> _innerHandles;
    public void onInitialize(Color color)
    {
        _color = color;
        _triangles = new List<Triangle>();
        _innerHandles = new List<InnerHandle>();
    }

    public void addTriangle(Triangle triangle)
    {
        triangle.transform.SetParent(transform);
        triangle.changeColor(_color);
        _triangles.Add(triangle);
    }

    public bool isNeighbourOfInnerTriangles(Triangle triangle)
    {
        for (int i = 0; i < _triangles.Count; i++)
        {
            if (_triangles[i].isNeighbourOfTriangle(triangle))
            {
                return true;
            }
        }

        return false;
    }

    public void resetPivot()
    {
        Vector3 averagePosition = Vector3.zero;
        for (int i = 0; i < _triangles.Count; i++)
        {
            averagePosition += _triangles[i].transform.position;
            _triangles[i].transform.SetParent(null);
        }

        averagePosition /= (float)_triangles.Count;
        transform.position = averagePosition;

        for (int i = 0; i < _triangles.Count; i++)
        {
            _triangles[i].transform.SetParent(transform);
        }

    }

    public void setInnerHandles()
    {

        for (int i = 0; i < _triangles.Count; i++)
        {
            Transform triangle = _triangles[i].transform;
            for (int j = 0; j < 3; j++)
            {
                InnerHandle innerHandle = Instantiate(_innerHandlePrefab, transform);
                innerHandle.transform.position = triangle.TransformPoint(_triangles[i].vertices[j]);
                _innerHandles.Add(innerHandle);
            }
        }
    }

 
    public void desnapAllInnerHandles()
    {
        for (int i = 0; i < _innerHandles.Count; i++)
        {
            _innerHandles[i].desnap();
        }
    }

    public List<InnerHandle> innerHandles => _innerHandles;

}
