using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadMeshManager : MonoBehaviour
{
    public RoadTileManager roadTileManager;
    public List<RoadMesh> roadMeshes = new List<RoadMesh>();

    private Vector3Int[] offsets = new Vector3Int[] {new Vector3Int(0, 1, 0), new Vector3Int(1, 0, 0), new Vector3Int(0, -1, 0), new Vector3Int(-1, 0, 0)};
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public RoadMesh AddToRoadMesh(Vector3Int location, Dictionary<Vector3Int, RoadDisplay> roadMap) {
        foreach (RoadMesh roadMesh in roadMeshes) {
            foreach(Vector3Int offset in offsets) {
                if (roadMesh.roads.ContainsKey(location + offset)) {
                    if (roadMesh.CalculateRoadMesh(location, roadMap)) {
                        // Debug.Log($"There are {roadMesh.roads.Keys.Count} roads in the current mesh");
                        // Debug.Log($"There are {roadMeshes.Count} meshes.");
                        return roadMesh;
                    }
                }
            }
        }

        RoadMesh newRoadmesh = new RoadMesh();
        newRoadmesh.CalculateRoadMesh(location, roadMap);
        newRoadmesh.roadMeshManager = this;
        roadMeshes.Add(newRoadmesh);
        return newRoadmesh;
    }
}
