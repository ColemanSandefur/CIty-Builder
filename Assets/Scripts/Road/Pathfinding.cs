using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    public Dictionary<Vector3Int, RoadDisplay> grid = new Dictionary<Vector3Int, RoadDisplay>();
    private List<RoadDisplay> openList;
    private List<RoadDisplay> closedList;
    private bool debug = true;

    public List<RoadDisplay> FindPath(Vector3Int startPos, Vector3Int stopPos) {
        RoadDisplay startNode = grid[startPos];
        RoadDisplay endNode = grid[stopPos];

        openList = new List<RoadDisplay> {startNode};
        closedList = new List<RoadDisplay>();

        foreach (RoadDisplay road in grid.Values) {
            road.gCost = int.MaxValue;
            road.CalculateFCost();
            road.cameFromNode = null;
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode.location, endNode.location);
        startNode.CalculateFCost();

        while (openList.Count > 0) {
            RoadDisplay currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode) {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);
            
            foreach (RoadDisplay neighborNode in GetNeighborList(currentNode)) {
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

    private List<RoadDisplay> CalculatePath(RoadDisplay endNode) {
        List<RoadDisplay> path = new List<RoadDisplay>();
        path.Add(endNode);
        RoadDisplay currentNode = endNode;
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

    private List<RoadDisplay> GetNeighborList(RoadDisplay currentNode) {
        List<RoadDisplay> neighborList = new List<RoadDisplay>();

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
            foreach (RoadDisplay road in neighborList) {
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

    private RoadDisplay GetLowestFCostNode(List<RoadDisplay> roadNodeList) {
        RoadDisplay lowestFCostNode = roadNodeList[0];

        foreach(RoadDisplay roadDisplay in roadNodeList) {
            if (roadDisplay.fCost < lowestFCostNode.fCost) {
                lowestFCostNode = roadDisplay;
            }
        }

        return lowestFCostNode;
    }
}
