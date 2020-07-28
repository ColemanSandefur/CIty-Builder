using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePlacement : MonoBehaviour
{
    public Tilemap ground;
    public Tilemap overlay;
    public Tilemap tilemap_road;
    public Grid grid;
    public RoadGrid roadGrid;

    public TileBase overlayTile;

    public GridTile road;

    Vector3Int start;
    Vector3Int stop;
    PathfindingTester pathfindingTester = new PathfindingTester();

    Vector3Int prevPos;
    // Start is called before the first frame update
    void Start()
    {
        pathfindingTester.roadGrid = roadGrid;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = - Camera.main.transform.localPosition.z;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);
        
        Vector3Int overlayPos = MouseToTilePos(overlay);
        if (!prevPos.Equals(overlayPos)) {
            overlay.ClearAllTiles();
            overlay.SetTile(overlayPos, overlayTile);
        }

        if (Input.GetKey(KeyCode.Mouse0)) {
            if (grid.TileEmpty(overlayPos)) {
                grid.AddTile(overlayPos, Instantiate(road));
            }
        }

        if (Input.GetKey(KeyCode.Mouse1)) {
            if (!grid.TileEmpty(overlayPos)) {
                grid.RemoveTile(overlayPos, true);
            }
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            grid.LoadTiles();
        }

        if (Input.GetKeyDown(KeyCode.N)) {
            roadGrid.ShadeColor(Color.blue);
        }

        if (Input.GetKeyDown(KeyCode.M)) {
            roadGrid.ShadeColor(Color.white);
        }

        if (Input.GetKeyDown(KeyCode.J)) {
            Debug.Log(roadGrid.TileEmpty(overlayPos));
            pathfindingTester.SetStart(roadGrid.GetTile(overlayPos));
        }

        if (Input.GetKeyDown(KeyCode.K)) {
            pathfindingTester.SetStop(roadGrid.GetTile(overlayPos));
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            pathfindingTester.FindPath();
        }

        if (Input.GetKeyDown(KeyCode.I)) {
            pathfindingTester.ResetColors(Color.white);
            pathfindingTester.ColorPoints();
        }
    }

    static Vector3Int MouseToTilePos(Tilemap tilemap) {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = - Camera.main.transform.localPosition.z;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);
        return tilemap.WorldToCell(worldPoint);

        
    }
}
