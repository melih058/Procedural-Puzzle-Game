using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class GridManager : BaseSingleton<GridManager>
{
    private LineRenderer _lineRenderer;

    [Header("LevelDataSO")]
    [SerializeField] private LevelDataSO _levelDataSO;
    private int _blockCount;
    private Color[] _colors;
    private int _gridSize;

    [Header("Node Settings")]
    [SerializeField] private float _nodeOffset;
    private Node[] _nodes;
    [SerializeField] private Node _nodePrefab;
    [SerializeField] private Transform _nodeParent;

    [Header("Triangle Settings")]
    [SerializeField] private Triangle _trianglePrefab;
    [SerializeField] private List<Triangle> _triangles;
    [SerializeField] private Transform _triangleParent;

    [Header("Block Settings")]
    [SerializeField] private Block _blockPrefab;
    [SerializeField] private Transform _blockParent;
    private List<Block> _blocks;

    void Start()
    {
        onInitialize();
    }

    private void onInitialize()
    {
        loadLevelDataSO();
        initializeGrid();
        initializeTriangles();
        initializeBlocks();
    }



    private void loadLevelDataSO()
    {
        _gridSize = _levelDataSO.gridSize+1;
        _colors = _levelDataSO.colors;
        _blockCount = _levelDataSO.blockCount;
    }

    private void initializeGrid()
    {
        _nodes = new Node[_gridSize * _gridSize];
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 5;
        createGrid();
    }
    private void createGrid()
    {
        float xOffset = (_gridSize * _nodeOffset) *0.5f - _nodeOffset*0.5f;
        float yOffset = 7 - _gridSize;
        for (int x = 0; x < _gridSize; x++)
        {
            for (int y = 0; y < _gridSize; y++)
            {
                Node node = Instantiate(_nodePrefab, _nodeParent);
                Vector3 pos = new Vector3(_nodeOffset * x - xOffset, _nodeOffset * y + yOffset, 0f);
                node.transform.position = pos;
                _nodes[x * _gridSize + y] = node;
            }
        }

        setLineRenderer();
    }

    private void setLineRenderer()
    {
        Vector3[] linePositions = new Vector3[5];
        linePositions[0] = _nodes[0].transform.position;
        linePositions[1] = _nodes[_gridSize - 1].transform.position;
        linePositions[2] = _nodes[(_gridSize * _gridSize) - 1].transform.position;
        linePositions[3] = _nodes[_gridSize * (_gridSize - 1)].transform.position;
        linePositions[4] = _nodes[0].transform.position - Vector3.right * 0.09f;

        for (int i = 0; i < linePositions.Length; i++)
        {
            linePositions[i].z = -0.08f;
        }
        _lineRenderer.SetPositions(linePositions);
    }

    private void initializeTriangles()
    {
        _triangles = new List<Triangle>();

        int rootIndex = 0;
        for (int i = 0; i < (_gridSize - 1) * (_gridSize - 1); i++)
        {
            if (rootIndex % _gridSize == (_gridSize - 1))
            {
                rootIndex++;
                i--;
                continue;
            }
            createTwoTriangle(rootIndex);
            rootIndex++;
        }

    }


    private void createTwoTriangle(int rootIndex)
    {
        Triangle triangle1 = Instantiate(_trianglePrefab, _triangleParent);
        Triangle triangle2 = Instantiate(_trianglePrefab, _triangleParent);

        Node[] triangle1_nodes = new Node[3];
        triangle1_nodes[0] = _nodes[rootIndex];
        triangle1_nodes[1] = _nodes[rootIndex + 1];
        triangle1_nodes[2] = _nodes[rootIndex + _gridSize];


        Node[] triangle2_nodes = new Node[3];
        triangle2_nodes[0] = _nodes[rootIndex + 1];
        triangle2_nodes[1] = _nodes[rootIndex + 1 + _gridSize];
        triangle2_nodes[2] = _nodes[rootIndex + 1 + _gridSize - 1];

        for (int i = 0; i < 3; i++)
        {
            triangle1_nodes[i].targetSnapCount++;
            triangle2_nodes[i].targetSnapCount++;
        }

        triangle1.onInitialize(triangle1_nodes);
        triangle2.onInitialize(triangle2_nodes);

        _triangles.Add(triangle1);
        _triangles.Add(triangle2);
    }

    private void initializeBlocks()
    {
        _blocks = new List<Block>();

        int triangleCount = (_gridSize - 1) * (_gridSize - 1) * 2;

        int selectedTriangleIndex = 0;

        Triangle initTriangle = _triangles[selectedTriangleIndex];
        createNewBlockForAddTriangle(ref triangleCount, ref selectedTriangleIndex, initTriangle);

        while (triangleCount > 0)
        {
            Triangle selectedTriangle = _triangles[selectedTriangleIndex];
            int blockCountDifference = _blockCount - _blocks.Count;
            if (blockCountDifference > 0)
            {
                if (triangleCount > blockCountDifference)
                {
                    int randomDecision = UnityEngine.Random.Range(0, 100);
                    if (randomDecision > 50)
                    {
                        createNewBlockForAddTriangle(ref triangleCount, ref selectedTriangleIndex, selectedTriangle);
                    }
                    else
                    {
                        addTriangeToRandomBlock(ref triangleCount, ref selectedTriangleIndex, selectedTriangle);
                    }
                }
                else
                {
                    createNewBlockForAddTriangle(ref triangleCount, ref selectedTriangleIndex, selectedTriangle);
                }
            }
            else
            {
                addTriangeToRandomBlock(ref triangleCount, ref selectedTriangleIndex, selectedTriangle);
            }
        }

        resetPivotOfBlocks();
        setInnerHandlesOfBlocks();
        shuffleBlocks();
    }



    private void createNewBlockForAddTriangle(ref int triangleCount, ref int selectedTriangleIndex, Triangle initTriangle)
    {
        Block targetBlock = Instantiate(_blockPrefab, _blockParent);
        targetBlock.onInitialize(_colors[_blocks.Count]);
        targetBlock.addTriangle(initTriangle);
        _blocks.Add(targetBlock);
        triangleCount--;
        selectedTriangleIndex++;
    }

    private void addTriangeToRandomBlock(ref int triangleCount, ref int selectedTriangleIndex, Triangle selectedTriangle)
    {
        List<Block> neighbours = findNeighboursOfTriangle(selectedTriangle);
        int random = UnityEngine.Random.Range(0, neighbours.Count);
        neighbours[random].addTriangle(selectedTriangle);
        triangleCount--;
        selectedTriangleIndex++;
    }

    private List<Block> findNeighboursOfTriangle(Triangle triangle)
    {
        List<Block> neighboursBlock = new List<Block>();

        for (int i = 0; i < _blocks.Count; i++)
        {
            if (_blocks[i].isNeighbourOfInnerTriangles(triangle))
            {
                neighboursBlock.Add(_blocks[i]);
            }
        }

        return neighboursBlock;
    }
    private void setInnerHandlesOfBlocks()
    {
        for (int i = 0; i < _blocks.Count; i++)
        {
            _blocks[i].setInnerHandles();
        }
    }
    private void resetPivotOfBlocks()
    {
        for (int i = 0; i < _blocks.Count; i++)
        {
            _blocks[i].resetPivot();
        }
    }

    private void shuffleBlocks()
    {
        for (int i = 0; i < _blocks.Count; i++)
        {
            Vector3 bottomPosition = Vector3.zero;
            bottomPosition.y = _blocks[i].transform.position.y - _gridSize * _nodeOffset;
            StartCoroutine(moveToTarget(_blocks[i].transform, bottomPosition));
        }
    }

    private IEnumerator moveToTarget(Transform targetTransform, Vector3 targetPos)
    {
        targetTransform.position += Vector3.up * 7;
        float duration = 0f;
        Vector3 initPos = targetTransform.position;
        targetPos.x = initPos.x * -1;
        while (duration <= 1f)
        {
            targetTransform.position = Vector3.Lerp(initPos, targetPos, duration);
            duration += Time.deltaTime;
            yield return null;
        }
    }

    public Vector3 tryGetSnapPoint(Block block)
    {
        Vector3 snapPosition = Vector3.zero;
        List<InnerHandle> blockPoints = block.innerHandles;
        int checkedPointCount = 0;
        for (int i = 0; i < blockPoints.Count; i++)
        {
            Node node = findClosestNode(blockPoints[i].transform.position);
            if (node != null)
            {
                snapPosition += node.transform.position;
                blockPoints[i].snap(node);
                checkedPointCount++;
            }
            else
            {
                break;
            }

        }

        if (checkedPointCount != blockPoints.Count)
        {
            snapPosition = -Vector3.forward;
            block.desnapAllInnerHandles();
            Debug.Log("all points didn't match");
        }
        else
        {
            snapPosition /= (float)blockPoints.Count;
            checkGameStatus();
        }


        return snapPosition;
    }

    private Node findClosestNode(Vector3 pos)
    {
        float minDistance = float.MaxValue;
        int minIndex = -1;

        for (int i = 0; i < _nodes.Length; i++)
        {
            if (_nodes[i].isSnapped)
                continue;

            float distance = (pos - _nodes[i].transform.position).magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                minIndex = i;
            }
        }

        if (minDistance > 0.5f)
            return null;

        return _nodes[minIndex];
    }

    private void checkGameStatus()
    {
        for (int i = 0; i < _nodes.Length; i++)
        {
            if (!_nodes[i].isSnapped)
            {
                return;
            }
        }
        Debug.Log("GAME STATUS : COMPLETED");
        GameManager.instance.onLevelSuccess();
    }


}
