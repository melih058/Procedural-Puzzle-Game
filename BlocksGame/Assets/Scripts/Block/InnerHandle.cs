using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InnerHandle : MonoBehaviour
{
    private bool _isSnapped;
    private Node _snappedNode;

    public void snap(Node snappedNode)
    {
        _isSnapped = true;
        _snappedNode = snappedNode;
        _snappedNode.snap();
    }
    public void desnap()
    {
        _isSnapped = false;
        if (_snappedNode != null)
        {
            _snappedNode.desnap();
            _snappedNode = null;
        }
    }
    private void OnDrawGizmos()
    {
        Color debugColor = Color.red;
        if (_isSnapped)
            debugColor = Color.green;

        Gizmos.color = debugColor;

        Gizmos.DrawSphere(transform.position, 0.2f);
    }

    public bool isSnapped => _isSnapped;
}
