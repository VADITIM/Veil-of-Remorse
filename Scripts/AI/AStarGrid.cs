using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Pathfinding;

public class AStarGrid : MonoBehaviour
{
    public Tilemap collisionTilemap;
    public LayerMask obstacleLayer;
    public float nodeSize = 1f;
    
    private Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    void Start()
    {
        CreateGridFromTilemap();
    }

    void CreateGridFromTilemap()
    {
        BoundsInt bounds = collisionTilemap.cellBounds;
        
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector2Int gridPos = new Vector2Int(x, y);
                Vector3Int tilePos = new Vector3Int(x, y, 0); 
                
                Vector2 worldPos = collisionTilemap.CellToWorld(tilePos) + Vector3.one * (nodeSize / 2);
                bool walkable = (collisionTilemap.GetTile(tilePos) == null) &&
                                !Physics2D.OverlapCircle(worldPos, nodeSize / 2, obstacleLayer);

                grid[gridPos] = new Node(walkable, worldPos, gridPos);
            }
        }
    }

    public Node GetNodeFromWorldPosition(Vector2 worldPosition)
    {
        Vector3Int cellPos = collisionTilemap.WorldToCell(worldPosition);
        Vector2Int gridPos = new Vector2Int(cellPos.x, cellPos.y);

        if (grid.ContainsKey(gridPos))
            return grid[gridPos];

        return null; // No valid node found
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        Vector2Int[] directions = {
            new Vector2Int(0, 1),  // Up
            new Vector2Int(0, -1), // Down
            new Vector2Int(1, 0),  // Right
            new Vector2Int(-1, 0)  // Left
        };

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborPos = node.gridPosition + direction;
            if (grid.ContainsKey(neighborPos) && grid[neighborPos].walkable)
                neighbors.Add(grid[neighborPos]);
        }

        return neighbors;
    }
}
