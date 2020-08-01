using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid : MonoBehaviour
{
    public Dictionary<Vector3Int, GridTile> tiles = new Dictionary<Vector3Int, GridTile>();
    protected List<Grid> subGrids = new List<Grid>();
    public System.Type type;
    public GridTile prefab;
    protected Tilemap tilemap;
    public int layer;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        LoadTiles();
        LoadTiles(tilemap);
    }

    public void ShadeColor(Color color) {
        foreach (GridTile tile in tiles.Values) {
            tile.spriteRenderer.color = color;
        }
    }

    protected virtual void Init() {
        type = prefab.GetType();
        foreach (Grid grid in GetComponentsInChildren<Grid>()) {

            if (grid.Equals(this)) {
                continue;
            }
            grid.Init();
            subGrids.Add(grid);
        }
        tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Grid GetSubGrid(System.Type type) {
        foreach (Grid grid in subGrids) {
            if (grid.GetType() == type) {
                return grid;
            }
        }
        return null;
    }

    public Grid GetSubGrid(GridTile tileType) {
        foreach (Grid grid in subGrids) {
            if (grid.GetType() == tileType.GetType()) {
                return grid;
            }
        }
        return null;
    }

    /*
     * Modify tiles
     */

    public bool AddTile(Vector3Int location, GridTile tile) {
        if (tiles.ContainsKey(location) && tile == null) {
            return false;
        }

        SetTile(location, tile);
        return true;
    }

    public virtual void SetTile(Vector3Int location, GridTile tile) {
        if (tile == null) {
            return;
        }
        if (tilemap == null) {
            tilemap = GetComponent<Tilemap>();
        }
        tile.transform.position = tilemap.CellToWorld(location) + new Vector3(0, .25f, 0);
        tile.transform.SetParent(tilemap.transform);
        tile.spriteRenderer.sortingOrder = layer;
        tile.location = location;
        tiles[location] = tile;

        foreach (Grid newGrid in subGrids) {
            if (tile.GetType() == newGrid.type) {
                newGrid.SetTile(location, tile);
            }
        }
        
        // UpdateNeighbors(location);
    }

    public virtual bool RemoveTile(Vector3Int location, bool destroy) {
        if (TileEmpty(location)) {
            return false;
        }

        GridTile tmp = tiles[location];

        tiles.Remove(location);
        foreach (Grid grid in subGrids) {
            grid.RemoveTile(location, false);
        }
        // UpdateNeighbors(location);

        if (destroy) {
            Destroy(tmp.gameObject);
        }
        return true;
    }

    public virtual void ClearAllTiles() {
        foreach (GridTile tile in tiles.Values) {
            Destroy(tile.gameObject);
        }

        tiles.Clear();
    }

    public virtual GridTile GetTile(Vector3Int location) {
        return tiles[location];
    }

    public virtual bool TileEmpty(Vector3Int location) {
        return !tiles.ContainsKey(location);
    }

    public virtual Dictionary<Vector3Int, GridTile> GetAllTiles() {
        return tiles;
    }

    private void LoadTiles(Tilemap tilemap) {
        //Loop through each tile in tilemap and assign it to the 'tiles' Dictionary
        //Then remove it from the tilemap

        foreach (var coord in tilemap.cellBounds.allPositionsWithin) {
            if (!tilemap.HasTile(coord)) {
                continue;
            }

            Sprite sprite = tilemap.GetSprite(coord);
            GridTile tile = Instantiate<GridTile>(prefab);
            tile.SetLocation(coord);
            tile.SetSprite(sprite);
            // tile.name = sprite.name;
            SetTile(coord, tile);
        }

        tilemap.ClearAllTiles();
    }

    public void LoadTiles() {
        Init();
        foreach (GridTile tile in GetComponentsInChildren<GridTile>()) {
            AddTile(tile.location, tile);
        }
    }

    public void ClearAllTiles(bool destroy) {
        foreach (GridTile tile in tiles.Values) {
            Destroy(tile);
        }
        tiles.Clear();
    }

    /*
     * update tiles
     */

    protected virtual void UpdateNeighbors(Vector3Int location) {
        
        bool hasUpdated = false;
        foreach (Grid grid in subGrids) {
            if (!grid.TileEmpty(location)) {
                grid.UpdateNeighbors(location);
                hasUpdated = true;
            }
        }
        if (hasUpdated) {
            return;
        }
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
}
