using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class Node
    {
        public bool walkable;
        public Vector2 worldPosition;
        public Vector2Int gridPosition;
        public int gCost, hCost;
        public Node parent;

        public Node(bool _walkable, Vector2 _worldPosition, Vector2Int _gridPosition)
        {
            walkable = _walkable;
            worldPosition = _worldPosition;
            gridPosition = _gridPosition;
        }

        public int FCost => gCost + hCost;
    }
}
