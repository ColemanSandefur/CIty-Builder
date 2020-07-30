using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingGrid : Grid
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Init() {
        type = prefab.GetType();
        tilemap = transform.parent.GetComponent<Tilemap>();
    }

    public override void SetTile(Vector3Int location, GridTile tile) {
        BuildingTile buildingTile = (BuildingTile) tile;
        tiles[location] = buildingTile;
        
        buildingTile.transform.SetParent(this.transform);
        if (buildingTile.name.IndexOf("(Clone)") > -1) {
            buildingTile.name = buildingTile.name.Substring(0, buildingTile.name.IndexOf("(Clone)")) + location;
        }
    }

    public override bool RemoveTile(Vector3Int location, bool destroy) {
        tiles.Remove(location);
        return true;
    }

    public Dictionary<Vector3Int, RoadTile> GetRoadTiles() {
        Dictionary<Vector3Int, RoadTile> newTiles = new Dictionary<Vector3Int, RoadTile>();
        foreach(KeyValuePair<Vector3Int, GridTile> keyValuePair in tiles) {
            newTiles.Add(keyValuePair.Key, (RoadTile)keyValuePair.Value);
        }
        return newTiles;
    }
}
