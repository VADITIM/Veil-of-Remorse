using UnityEngine;
using System.Collections.Generic;

public class PathfindingGrid : MonoBehaviour
{
    public static PathfindingGrid Instance { get; private set; }

    [Header("Grid Settings")]
    public float nodeSpacing = 0f;
    public LayerMask wallLayer;
    public Vector2 gridWorldBottomLeft = new Vector2(-50f, -50f); // Covers 100x100 units with 200x200 nodes

    private Node[,] grid;
    private Vector2Int gridSize = new Vector2Int(200, 200);

    public class Node
    {
        public bool walkable;
        public Vector2 worldPosition;
        public int gridX, gridY;
        public int gCost, hCost;
        public Node parent;

        public int fCost => gCost + hCost;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        grid = new Node[gridSize.x, gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2 worldPoint = gridWorldBottomLeft + Vector2.right * (x * nodeSpacing) + Vector2.up * (y * nodeSpacing);
                bool walkable = !Physics2D.OverlapCircle(worldPoint, nodeSpacing * 0.4f, wallLayer);

                grid[x, y] = new Node
                {
                    walkable = walkable,
                    worldPosition = worldPoint,
                    gridX = x,
                    gridY = y
                };
            }
        }
    }

    public Node WorldPointToNode(Vector2 worldPoint)
    {
        Vector2 localPoint = worldPoint - gridWorldBottomLeft;
        int x = Mathf.Clamp(Mathf.RoundToInt(localPoint.x / nodeSpacing), 0, gridSize.x - 1);
        int y = Mathf.Clamp(Mathf.RoundToInt(localPoint.y / nodeSpacing), 0, gridSize.y - 1);
        return grid[x, y];
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbors;
    }

    public int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.gridX - b.gridX);
        int dstY = Mathf.Abs(a.gridY - b.gridY);
        return dstX > dstY ? 14 * dstY + 10 * (dstX - dstY) : 14 * dstX + 10 * (dstY - dstX);
    }

    // private void OnDrawGizmos()
    // {
    //     if (grid == null) return;

    //     Gizmos.color = Color.gray;
    //     foreach (Node node in grid)
    //     {
    //         Gizmos.color = node.walkable ? Color.green : Color.red;
    //         Gizmos.DrawWireCube(node.worldPosition, Vector3.one * nodeSpacing * 0.8f);
    //     }
    // }
}