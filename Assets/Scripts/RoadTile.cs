using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu]
public class RoadTile : Tile 
{
    public Sprite[] m_Sprites;
    public Sprite m_Preview;
    public Sprite RoadType;
    // public GameObject m_Prefab;
    public Sprite curSprite;

    public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go) {
        base.StartUp(location, tilemap, go);
        if (go != null) {
            // TestScript s = go.GetComponent<TestScript>();
            // s.location = location;
            // s.tilemap = tilemap;
            // s.sprite = GetCurrentSprite(location, tilemap);
        } else {
            // Debug.Log("gameObject is null");
        }
        return true;
    }

    // This refreshes itself and other RoadTiles that are orthogonally and diagonally adjacent
    public override void RefreshTile(Vector3Int location, ITilemap tilemap)
    {
        
        for (int yd = -1; yd <= 1; yd++) {
            for (int xd = -1; xd <= 1; xd++)
            {
                Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
                if (HasRoadTile(tilemap, position)) {
                    tilemap.RefreshTile(position);
                }
            }
        }
    }
    // This determines which sprite is used based on the RoadTiles that are adjacent to it and rotates it to fit the other tiles.
    // As the rotation is determined by the RoadTile, the TileFlags.OverrideTransform is set for the tile.
    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        // tileData.gameObject = gameObject;

        curSprite = GetCurrentSprite(location, tilemap);

        this.sprite = curSprite;
        this.color = new Color(255, 255, 255, 0);
        tileData.sprite = this.sprite;
        tileData.color = this.color;
    }
    // This determines if the Tile at the position is the same RoadTile.
    private bool HasRoadTile(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }

    private Sprite GetCurrentSprite(Vector3Int location, ITilemap tilemap) {
        int mask = HasRoadTile(tilemap, location + new Vector3Int(0, 1, 0)) ? 1 : 0; // Top;
        mask += HasRoadTile(tilemap, location + new Vector3Int(1, 0, 0)) ? 2 : 0; // Right
        mask += HasRoadTile(tilemap, location + new Vector3Int(0, -1, 0)) ? 4 : 0; // Bottom
        mask += HasRoadTile(tilemap, location + new Vector3Int(-1, 0, 0)) ? 8 : 0; // Left
        int index = GetIndex((byte)mask);
        if (index >= 0 && index < m_Sprites.Length) {
            return m_Sprites[index];
        }
        else{
            return m_Sprites[0];
        }
    }

    // The following determines which sprite to use based on the number of adjacent RoadTiles
    private int GetIndex(byte mask)
    {
        switch (mask)
        {
            case 0: return 11; //none
            case 3: return 4;//(1, 1)
            case 6: return 5;//(1, -1)
            case 9: return 3;//(-1, 1)
            case 12: return 2; //(-1, -1)
            case 1: return 1;// (0, 1)
            case 2: return 0;// (1, 0)
            case 4: return 1;// (0, -1)
            case 5: return 1;// (0, -1) (0, 1)
            case 10: return 0;// (-1, 0) (1,0)
            case 8: return 0; //(-1,0)
            case 7: return 9;//(0, 1) (1, 0) (0, -1)
            case 11: return 8;//(0,1) (1,0) (-1,0)
            case 13: return 7;// (0,1) (0,-1) (-1,0)
            case 14: return 6; //(1,0) (0,-1) (-1,0)
            case 15: return 10; //(0,1) (1,0) (0,-1) (-1,0)
        }
        return -1;
    }
// The following determines which rotation to use based on the positions of adjacent RoadTiles
#if UNITY_EDITOR
// The following is a helper that adds a menu item to create a RoadTile Asset
    [MenuItem("Assets/Create/RoadTile")]
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Road Tile", "New Road Tile", "Asset", "Save Road Tile", "Assets");
        if (path == "")
            return;
    AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<RoadTile>(), path);
    }
#endif
}