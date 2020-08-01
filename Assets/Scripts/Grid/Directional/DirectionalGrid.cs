using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DirectionalGrid : Grid
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
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
        DirectionalTile directionalTile = (DirectionalTile) tile;
        tiles[location] = directionalTile;
        directionalTile.transform.SetParent(this.transform);
        // if (directionalTile.name.IndexOf("(Clone)") > -1) {
        //     directionalTile.name = directionalTile.name.Substring(0, directionalTile.name.IndexOf("(Clone)")) + location;
        // }
        directionalTile.SetRotation(Rotation.NORTH);
        UpdateNeighbors(location);
    }

    public virtual void SetTile(Vector3Int location, GridTile tile, Rotation rotation) {
        SetTile(location, tile);
        DirectionalTile directionalTile = (DirectionalTile) tile;
        directionalTile.SetRotation(rotation);
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
