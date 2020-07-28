using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoadGrid : Grid
{
    
    void Start() {
        Init();
    }

    void Update() {

    }

    protected override void Init() {
        type = prefab.GetType();
        tilemap = transform.parent.GetComponent<Tilemap>();
    }

    public override void SetTile(Vector3Int location, GridTile tile) {
        RoadTile roadTile = (RoadTile) tile;
        tiles[location] = roadTile;
        roadTile.roadGrid = this;
        roadTile.transform.SetParent(this.transform);
        if (roadTile.name.IndexOf("(Clone)") > -1) {
            roadTile.name = roadTile.name.Substring(0, roadTile.name.IndexOf("(Clone)")) + location;
        }
        UpdateNeighbors(location);
    }

    public override bool RemoveTile(Vector3Int location, bool destroy) {
        tiles.Remove(location);
        UpdateNeighbors(location);
        return true;
    }

    protected override void UpdateNeighbors(Vector3Int location) {
        Vector3Int tmpLocation = location;
        if (!TileEmpty(tmpLocation)) {
            GetTile(tmpLocation).Refresh(tmpLocation, this);
        }
        tmpLocation = location + new Vector3Int(0, 1, 0);
        if (!TileEmpty(tmpLocation)) {
            GetTile(tmpLocation).Refresh(tmpLocation, this);
        }
        tmpLocation = location + new Vector3Int(1, 0, 0);
        if (!TileEmpty(tmpLocation)) {
            GetTile(tmpLocation).Refresh(tmpLocation, this);
        }
        tmpLocation = location + new Vector3Int(0, -1, 0);
        if (!TileEmpty(tmpLocation)) {
            GetTile(tmpLocation).Refresh(tmpLocation, this);
        }
        tmpLocation = location + new Vector3Int(-1, 0, 0);
        if (!TileEmpty(tmpLocation)) {
            GetTile(tmpLocation).Refresh(tmpLocation, this);
        }
    }

    public Dictionary<Vector3Int, RoadTile> GetRoadTiles() {
        Dictionary<Vector3Int, RoadTile> newTiles = new Dictionary<Vector3Int, RoadTile>();
        foreach(KeyValuePair<Vector3Int, GridTile> keyValuePair in tiles) {
            newTiles.Add(keyValuePair.Key, (RoadTile)keyValuePair.Value);
        }
        return newTiles;
    }
}
