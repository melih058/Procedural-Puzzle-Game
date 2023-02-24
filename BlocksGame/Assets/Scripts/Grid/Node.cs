using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private bool _isSnapped;
    private int _targetSnapCount;
    private int _currentSnapCount;
    private GameObject _childMeshObject;

    private void Start()
    {
        onInitialize();
    }

    private void onInitialize()
    {
        _childMeshObject = transform.GetChild(0).gameObject;
    }
    public void setChildMesh(bool state)
    {
        _childMeshObject.SetActive(state);
    }

    public void snap()
    {
        _currentSnapCount++;
        if (_currentSnapCount == _targetSnapCount)
        {
            _isSnapped = true;
        }
    }
    public void desnap()
    {
        _currentSnapCount--;
        if (_currentSnapCount < _targetSnapCount)
        {
            _isSnapped = false;
        }
    }
    private void OnDrawGizmos()
    {
        Color debugColor = Color.red;
        if (_isSnapped)
            debugColor = Color.green;

        Gizmos.color = debugColor;

        Gizmos.DrawWireSphere(transform.position, 0.22f);
    }
    public bool isSnapped => _isSnapped;
    public int targetSnapCount { get { return _targetSnapCount; } set { _targetSnapCount = value; } }

}
