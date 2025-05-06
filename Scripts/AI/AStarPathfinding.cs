using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AStarPathfinder : MonoBehaviour
{
    public AStarGrid grid;
    public Transform target; 
    public LayerMask obstacleLayer;

    void Start()
    {
        grid = FindObjectOfType<AStarGrid>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public List<Vector2> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Node startNode = grid.GetNodeFromWorldPosition(startPos);
        Node targetNode = grid.GetNodeFromWorldPosition(targetPos);

        if (!startNode.walkable || !targetNode.walkable)
            return new List<Vector2>(); 

        List<Node> openSet = new List<Node> { startNode };
        HashSet<Node> closedSet = new HashSet<Node>();

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        Dictionary<Node, int> gScore = new Dictionary<Node, int>();
        Dictionary<Node, int> fScore = new Dictionary<Node, int>();

        gScore[startNode] = 0;
        fScore[startNode] = Heuristic(startNode, targetNode);

        while (openSet.Count > 0)
        {
            Node currentNode = GetLowestFCostNode(openSet, fScore);

            if (currentNode == targetNode)
                return ReconstructPath(cameFrom, currentNode);

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            foreach (Node neighbor in grid.GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                    continue;

                int tentativeGScore = gScore[currentNode] + 1;

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = currentNode;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, targetNode);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }
        return new List<Vector2>(); 
    }

    private int Heuristic(Node a, Node b)
    {
        return Mathf.Abs(a.gridPosition.x - b.gridPosition.x) + Mathf.Abs(a.gridPosition.y - b.gridPosition.y);
    }

    private Node GetLowestFCostNode(List<Node> openSet, Dictionary<Node, int> fScore)
    {
        Node lowest = openSet[0];
        foreach (Node node in openSet)
        {
            if (fScore.ContainsKey(node) && fScore[node] < fScore[lowest])
                lowest = node;
        }
        return lowest;
    }

    private List<Vector2> ReconstructPath(Dictionary<Node, Node> cameFrom, Node currentNode)
    {
        List<Vector2> path = new List<Vector2>();
        while (cameFrom.ContainsKey(currentNode))
        {
            path.Add(currentNode.worldPosition);
            currentNode = cameFrom[currentNode];
        }
        path.Reverse();
        return path;
    }
}
