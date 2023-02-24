using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private float _rayDistance;
    private int _blockLayerMask = 1 << 6;
    private Block _selectedBlock;
    private float _zOffset;
    private Vector3 _posOffset;
    void Start()
    {
        onInitialize();
    }

    void Update()
    {
        if (_selectedBlock == null && Input.GetMouseButtonDown(0))
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _rayDistance, _blockLayerMask))
            {
                if (hit.transform.GetComponent<Block>() is Block block)
                {
                    _selectedBlock = block;
                    _selectedBlock.desnapAllInnerHandles();
                    _zOffset = _mainCamera.WorldToScreenPoint(_selectedBlock.transform.position).z;
                    _posOffset = _selectedBlock.transform.position - getWorldPosition();
                }
            }
        }
        else if (_selectedBlock != null && Input.GetMouseButton(0))
        {
            _selectedBlock.transform.position = getWorldPosition() + _posOffset;
        }
        else if(_selectedBlock != null && Input.GetMouseButtonUp(0))
        {
            dropSelectedBlock();
        }
    }

 

    private void onInitialize()
    {
        _mainCamera = Camera.main;
    }
    private Vector3 getWorldPosition()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = _zOffset;

        return _mainCamera.ScreenToWorldPoint(pos);

    }
    private void dropSelectedBlock()
    {
        Vector3 snapPosition = GridManager.instance.tryGetSnapPoint(_selectedBlock);
        if(snapPosition != -Vector3.forward)
        {
            _selectedBlock.transform.position = snapPosition;
        }
        _selectedBlock = null;
    }
}
