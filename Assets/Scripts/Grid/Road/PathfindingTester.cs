using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingTester
{
    Pathfinding pathfinding = new Pathfinding();
    public RoadGrid roadGrid;
    public GridTile start;
    public GridTile stop;

    public void SetStart(GridTile tile) {
        ResetColors(Color.white);
        start = tile;
        ColorPoints();
    }

    public void SetStop(GridTile tile) {
        ResetColors(Color.white);
        stop = tile;
        ColorPoints();
    }

    public void FindPath() {
        ResetColors(Color.white);
        pathfinding.grid = roadGrid.GetRoadTiles();
        pathfinding.FindPath(start.location, stop.location);
        ColorPoints();
    }

    public void ResetColors(Color color) {
        foreach (GridTile tile in roadGrid.GetAllTiles().Values) {
            tile.spriteRenderer.color = color;
        }
    }

    public void ColorPoints() {
        if (start != null) {
            start.spriteRenderer.color = Color.green;
        }

        if (stop != null) {
            stop.spriteRenderer.color = Color.red;
        }
    }
}
