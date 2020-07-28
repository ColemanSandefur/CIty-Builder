using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    public Dictionary<Vector3Int, RoadTile> grid = new Dictionary<Vector3Int, RoadTile>();
    private List<RoadTile> openList;
    private List<RoadTile> closedList;
    private bool debug = true;

    public List<RoadTile> FindPath(Vector3Int startPos, Vector3Int stopPos) {
        if (debug) {
            Debug.Log($"Start: {startPos}; Stop: {stopPos}");
        }
        
        RoadTile startNode = grid[startPos];
        RoadTile endNode = grid[stopPos];

        openList = new List<RoadTile> {startNode};
        closedList = new List<RoadTile>();

        foreach (RoadTile road in grid.Values) {
            road.gCost = int.MaxValue;
            road.CalculateFCost();
            road.cameFromNode = null;
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode.location, endNode.location);
        startNode.CalculateFCost();

        while (openList.Count > 0) {
            RoadTile currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode) {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);
            
            foreach (RoadTile neighborNode in GetNeighborList(currentNode)) {
                if (closedList.Contains(neighborNode)) {
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode.location, neighborNode.location);
                Debug.Log($"Tentative Cost: {tentativeGCost}, neighborNode gCost {neighborNode.gCost}, currentNode gCost: {currentNode.gCost}, distance cost: {CalculateDistanceCost(currentNode.location, neighborNode.location)}");
                if (tentativeGCost < neighborNode.gCost) { //If travelling to this node is faster/cheaper than the current path
                    neighborNode.cameFromNode = currentNode;
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.hCost = CalculateDistanceCost(neighborNode.location, endNode.location);
                    neighborNode.CalculateFCost();

                    if (!openList.Contains(neighborNode)) {
                        openList.Add(neighborNode);
                    }
                }
            }
        }

        //Out of nodes
        Debug.Log("Couldn't find path");
        return null;
    }

    private List<RoadTile> CalculatePath(RoadTile endNode) {
        List<RoadTile> path = new List<RoadTile>();
        path.Add(endNode);
        RoadTile currentNode = endNode;
        if (debug) {
            endNode.spriteRenderer.color = Color.red;
        }
        while (currentNode.cameFromNode != null) {
            currentNode = currentNode.cameFromNode;
            path.Add(currentNode);
            if (debug) {
                currentNode.spriteRenderer.color = Color.red;
            }
        }
        path.Reverse();
        return path;
    }

    private List<RoadTile> GetNeighborList(RoadTile currentNode) {
        List<RoadTile> neighborList = new List<RoadTile>();

        if (grid.ContainsKey(currentNode.location + new Vector3Int(0, 1, 0))) { //top
            neighborList.Add(grid[currentNode.location + new Vector3Int(0, 1, 0)]);
        }
        if (grid.ContainsKey(currentNode.location + new Vector3Int(1, 0, 0))) { //right
            neighborList.Add(grid[currentNode.location + new Vector3Int(1, 0, 0)]);
        }
        if (grid.ContainsKey(currentNode.location + new Vector3Int(0, -1, 0))) { //bottom
            neighborList.Add(grid[currentNode.location + new Vector3Int(0, -1, 0)]);
        }
        if (grid.ContainsKey(currentNode.location + new Vector3Int(-1, 0, 0))) { //left
            neighborList.Add(grid[currentNode.location + new Vector3Int(-1, 0, 0)]);
        }
        
        if (debug) {
            Debug.Log($"{currentNode.location} found {neighborList.Count} neighbors");
            foreach (RoadTile road in neighborList) {
                Debug.Log($"Found neighbor at {road.location}");
            }
        }
        return neighborList;
    }

    private int CalculateDistanceCost(Vector3Int a, Vector3Int b) {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);

        return MOVE_STRAIGHT_COST * (xDistance + yDistance);
    }

    private RoadTile GetLowestFCostNode(List<RoadTile> roadNodeList) {
        RoadTile lowestFCostNode = roadNodeList[0];

        foreach(RoadTile RoadTile in roadNodeList) {
            if (RoadTile.fCost < lowestFCostNode.fCost) {
                lowestFCostNode = RoadTile;
            }
        }

        return lowestFCostNode;
    }
}
