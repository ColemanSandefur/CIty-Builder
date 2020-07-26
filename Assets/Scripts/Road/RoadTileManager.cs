using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoadTileManager : MonoBehaviour
{
    public Dictionary<Vector3Int, RoadDisplay> roads = new Dictionary<Vector3Int, RoadDisplay>();
    public RoadMeshManager roadMeshManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRoad(GameObject road_prefab, Tilemap tilemap, Vector3Int location) {
        GameObject road = Instantiate(road_prefab);

        Vector3 newPos = tilemap.CellToWorld(location);
        newPos += new Vector3(0, .25f, 0);
        road.transform.position = newPos;
        road.transform.SetParent(tilemap.transform);
        RoadDisplay roadDisplay = road.GetComponent<RoadDisplay>();
        roadDisplay.roadTileManager = this;
        roads.Add(location, roadDisplay);
        roadDisplay.location = location;

        AlertNeighbors(tilemap, location);
        roadMeshManager.AddToRoadMesh(location, roads);
    }

    public void RemoveRoad(Tilemap tilemap, Vector3Int location) {
        if (roads.ContainsKey(location)) {
            Destroy(roads[location]);
            roads.Remove(location);
            AlertNeighbors(tilemap, location);
        }
    }

    void AlertNeighbors(Tilemap tilemap, Vector3Int location) {
        for (int yd = -1; yd <= 1; yd++) {
            for (int xd = -1; xd <= 1; xd++)
            {
                Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
                if (roads.ContainsKey(position)) {
                    roads[position].Refresh(tilemap, position);
                }
            }
        }
    }

    public RoadDisplay GetNeighbor(Tilemap grid, Vector3Int position) {
        Transform parent = grid.transform;
        var results = new List<GameObject>();
        var childCount = parent.childCount;
        for (var i = 0; i < childCount; i++)
        {
            var child = parent.GetChild(i);
            if (position == grid.WorldToCell(child.position) && child.GetComponent<RoadDisplay>() != null) {
                return child.GetComponent<RoadDisplay>();
            }
        }
        return null;
    }
}
