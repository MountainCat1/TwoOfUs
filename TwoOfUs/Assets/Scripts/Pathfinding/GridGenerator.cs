﻿using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public bool displayGridGizmos;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public bool debugUpdateGrid;
    
    Node[,] _grid;
    float _nodeDiameter;
    int _gridSizeX, _gridSizeY;
    
    void Awake()
    {
        _nodeDiameter = nodeRadius * 2;
        _gridSizeX = Mathf.RoundToInt(gridWorldSize.x / _nodeDiameter);
        _gridSizeY = Mathf.RoundToInt(gridWorldSize.y / _nodeDiameter);
        CreateGrid();
    }
    
    void Update()
    {
        if(nodeRadius < 0.05f)
        {
            nodeRadius = 0.05f;
        }
        
        if (debugUpdateGrid)
        {
            _nodeDiameter = nodeRadius * 2;
            _gridSizeX = Mathf.RoundToInt(gridWorldSize.x / _nodeDiameter);
            _gridSizeY = Mathf.RoundToInt(gridWorldSize.y / _nodeDiameter);
            CreateGrid();
        }
    }

    void CreateGrid()
    {
        _grid = new Node[_gridSizeX, _gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + nodeRadius) + Vector3.up * (y * _nodeDiameter + nodeRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius - 0.05f, unwalkableMask)); // we subtract 0.05f to avoid overlapping with the walls
                _grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);
        return _grid[x, y];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                // Check if the node is within the grid
                if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
                {
                    // If the neighbour is diagonal
                    if (Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1)
                    {
                        // Check if the horizontal and vertical neighbours are walkable
                        Node horizontalNeighbour = _grid[node.gridX + x, node.gridY];
                        Node verticalNeighbour = _grid[node.gridX, node.gridY + y];

                        if (horizontalNeighbour.walkable && verticalNeighbour.walkable)
                        {
                            neighbours.Add(_grid[checkX, checkY]);
                        }
                    }
                    else
                    {
                        neighbours.Add(_grid[checkX, checkY]);
                    }
                }
            }
        }

        return neighbours;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

        if (_grid != null && displayGridGizmos)
        {
            foreach (Node n in _grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (_nodeDiameter - .1f));
            }
        }
    }
}
