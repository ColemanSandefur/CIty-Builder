using System;
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
    public Grid overlayGrid;

    public TileBase overlayTile;

    public GridTile gridTile;

    Vector3Int start;
    Vector3Int stop;
    PathfindingTester pathfindingTester = new PathfindingTester();
    Rotation rotation;
    Vector3Int prevPos;

    bool overlayPlaced = false;
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
        //If mouse is in new tile
        if (!prevPos.Equals(overlayPos)) {

            //Remove overlay tile
            if (overlayPlaced) {
                grid.RemoveTile(prevPos, true);
                overlayPlaced = false;
            }

            //Place overlay tile
            if (grid.TileEmpty(overlayPos)) {
                GridTile tile = Instantiate(gridTile);
                if (!grid.AddTile(overlayPos, tile)) { //Remove tile if it failes
                    Destroy(tile);
                } else {
                    overlayPlaced = true;
                    if (tile is DirectionalTile) {
                        ((DirectionalTile) tile).SetRotation(rotation);
                    }
                }
            }
            
            prevPos = overlayPos;
        }

        // if (Input.GetKey(KeyCode.Mouse0)) {
        //     AddTile();
        // }

        // if (Input.GetKey(KeyCode.Mouse1)) {
        //     RemoveTile();
        // }

        if (Input.GetKeyDown(KeyCode.P)) {
            grid.LoadTiles();
        }

        if (Input.GetKeyDown(KeyCode.N)) {
            roadGrid.ShadeColor(Color.blue);
        }

        if (Input.GetKeyDown(KeyCode.M)) {
            roadGrid.ShadeColor(Color.white);
        }

        if (Input.GetKeyDown(KeyCode.J) && !overlayPlaced) {
            Debug.Log(roadGrid.TileEmpty(overlayPos));
            pathfindingTester.SetStart(roadGrid.GetTile(overlayPos));
        }

        if (Input.GetKeyDown(KeyCode.K) && !overlayPlaced) {
            pathfindingTester.SetStop(roadGrid.GetTile(overlayPos));
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            pathfindingTester.FindPath();
        }

        if (Input.GetKeyDown(KeyCode.I)) {
            pathfindingTester.ResetColors(Color.white);
            pathfindingTester.ColorPoints();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                ChangeRotation(-1);
            } else {
                ChangeRotation(1);
            }
            
        }

        
    }

    public void AddTile() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = - Camera.main.transform.localPosition.z;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);
        
        Vector3Int overlayPos = MouseToTilePos(overlay);
        if ((prevPos.Equals(overlayPos) && overlayPlaced) || grid.TileEmpty(overlayPos)) {
            if (overlayPlaced) {
                grid.RemoveTile(prevPos, true);
            }
            overlayPlaced = false;
            GridTile tile = Instantiate(gridTile);
            if (!grid.AddTile(overlayPos, tile)) {
                Destroy(tile);
            }
            if (gridTile is DirectionalTile) {
                ((DirectionalTile) tile).SetRotation(rotation);
            }
        }
    }

    public void RemoveTile() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = - Camera.main.transform.localPosition.z;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);
        
        Vector3Int overlayPos = MouseToTilePos(overlay);
        if (!grid.TileEmpty(overlayPos)) {
            grid.RemoveTile(overlayPos, true);
        }
    }

    public void ChangeRotation(int times) {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = - Camera.main.transform.localPosition.z;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);
        
        Vector3Int overlayPos = MouseToTilePos(overlay);

        int x = (int)rotation;
        x += times;
        if (x >= Enum.GetValues(typeof(Rotation)).GetLength(0)) {
            x -= Enum.GetValues(typeof(Rotation)).GetLength(0);
        }

        if (x < 0) {
            x += Enum.GetValues(typeof(Rotation)).GetLength(0);
        }
        rotation = (Rotation)x;

        if (grid.GetTile(overlayPos) is DirectionalTile) {
            DirectionalTile tile = (DirectionalTile) grid.GetTile(overlayPos);
            tile.SetRotation(rotation);
        }

        Debug.Log(rotation);
    }

    public void SetTile(GridTile tile) {
        this.gridTile = tile;
    }

    static Vector3Int MouseToTilePos(Tilemap tilemap) {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = - Camera.main.transform.localPosition.z;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);
        return tilemap.WorldToCell(worldPoint);
    }
}
