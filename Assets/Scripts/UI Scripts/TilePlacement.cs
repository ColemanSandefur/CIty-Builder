using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePlacement : MonoBehaviour
{
    public Tilemap ground;
    public Tilemap overlay;
    public Tilemap tilemap_road;

    public RoadTileManager roadTileManager;

    public TileBase overlayTile;

    public GameObject road;

    Vector3Int start;
    Vector3Int stop;
    Pathfinding pathfinding = new Pathfinding();

    Vector3Int prevPos;
    // Start is called before the first frame update
    void Start()
    {
        
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
            if (GetObjectsInCell(tilemap_road, tilemap_road.transform, overlayPos).Count == 0 && tilemap_road.GetTile(overlayPos) == null) {
                // GameObject i = Instantiate(road);
                // i.transform.position = tilemap_road.CellToWorld(overlayPos);
                // i.transform.position = new Vector3(i.transform.position.x, i.transform.position.y + .25f, i.transform.position.z);
                // i.transform.SetParent(tilemap_road.transform);
                roadTileManager.CreateRoad(road, tilemap_road, overlayPos);
            }
        }

        if (Input.GetKey(KeyCode.Mouse1)) {
            if (roadTileManager.roads.ContainsKey(overlayPos)) {
                roadTileManager.RemoveRoad(tilemap_road, overlayPos);
            } else if (GetObjectsInCell(tilemap_road, tilemap_road.transform, overlayPos).Count != 0) {
                Destroy(GetObjectsInCell(tilemap_road, tilemap_road.transform, overlayPos)[0]);
            } else {
                tilemap_road.SetTile(overlayPos, null);
            }
        }

        if (Input.GetKeyDown(KeyCode.J)) {
            Debug.Log(start);
            if (roadTileManager.roads.ContainsKey(start) ) {
                roadTileManager.roads[start].spriteRenderer.color=Color.white;
            }
            start = overlayPos;
            roadTileManager.roads[overlayPos].spriteRenderer.color=Color.green;
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            if (roadTileManager.roads.ContainsKey(stop)) {
                roadTileManager.roads[stop].spriteRenderer.color=Color.white;
            }
            stop = overlayPos;
            roadTileManager.roads[overlayPos].spriteRenderer.color=Color.blue;
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            pathfinding.grid = roadTileManager.roads;
            pathfinding.FindPath(start, stop);
        }
        if (Input.GetKeyDown(KeyCode.I)) {
            foreach(RoadDisplay road in roadTileManager.roads.Values) {
                road.spriteRenderer.color = Color.white;
            }

            if (roadTileManager.roads.ContainsKey(start)) {
                roadTileManager.roads[start].spriteRenderer.color=Color.green;
            }
            if (roadTileManager.roads.ContainsKey(stop)) {
                roadTileManager.roads[stop].spriteRenderer.color=Color.blue;
            }
        }
    }

    static Vector3Int MouseToTilePos(Tilemap tilemap) {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = - Camera.main.transform.localPosition.z;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);
        return tilemap.WorldToCell(worldPoint);

        
    }

    public List<GameObject> GetObjectsInCell(Tilemap grid, Transform parent, Vector3Int position) {
        var results = new List<GameObject>();
        var childCount = parent.childCount;
        for (var i = 0; i < childCount; i++)
        {
            var child = parent.GetChild(i);
            if (position == grid.WorldToCell(child.position))
            {
                results.Add(child.gameObject);
            }
        }
        return results;
    }
}
