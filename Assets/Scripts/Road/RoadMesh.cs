using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoadMesh
{
    public Dictionary<Vector3Int, RoadDisplay> roads = new Dictionary<Vector3Int, RoadDisplay>();
    public Dictionary<Vector3Int, RoadDisplay> junctions = new Dictionary<Vector3Int, RoadDisplay>();
    public RoadMeshManager roadMeshManager;

    public bool CalculateRoadMesh(Vector3Int location, Dictionary<Vector3Int, RoadDisplay> roadMap) {
        if (roads.ContainsKey(location)) {
            return true;
        } else if (!roadMap.ContainsKey(location)) {
            return false;
        }

        RoadDisplay roadDisplay = roadMap[location];
        roads.Add(location, roadDisplay);
        
        if (roadDisplay.roadMesh != null && roadDisplay.roadMesh != this) {
            roadMeshManager.roadMeshes.Remove(roadDisplay.roadMesh);
        }
        roadDisplay.roadMesh = this;
        
        CalculateRoadMesh(location + new Vector3Int(0, 1, 0), roadMap);
        CalculateRoadMesh(location + new Vector3Int(1, 0, 0), roadMap);
        CalculateRoadMesh(location + new Vector3Int(0, -1, 0), roadMap);
        CalculateRoadMesh(location + new Vector3Int(-1, 0, 0), roadMap);

        // int neighbors = 0;
        // neighbors += (CalculateRoadMesh(location + new Vector3Int(0, 1, 0), roadMap)) ? 1 : 0;
        // neighbors += (CalculateRoadMesh(location + new Vector3Int(1, 0, 0), roadMap)) ? 1 : 0;
        // neighbors += (CalculateRoadMesh(location + new Vector3Int(0, -1, 0), roadMap)) ? 1 : 0;
        // neighbors += (CalculateRoadMesh(location + new Vector3Int(-1, 0, 0), roadMap)) ? 1 : 0;
        // if (neighbors > 2) {
        //     junctions.Add(location, roadMap[location].GetComponent<RoadDisplay>());
        // }

        return true;
    }
}
