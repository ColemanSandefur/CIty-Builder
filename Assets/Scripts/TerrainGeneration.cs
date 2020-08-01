using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration: MonoBehaviour
{
    public Grid ground;
    public Grid buildings;
    public GridTile tree;
    public GridTile grass;
    
    public float threshold;
    public float scale;
    public int width;
    public int height;
    public float xOff;
    public float yOff;

    public void CalcNoise() {
        buildings.ClearAllTiles();
        float xScale = (float) width * scale;
        float yScale = (float) height * scale;
        for (int w = 0; w < width; w++) {
            for (int h = 0; h < height; h++) {
                float xCoord = w / (float)width * scale + xOff;
                float yCoord = h / (float)height * scale + yOff;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                if (sample > threshold) {
                    buildings.AddTile(new Vector3Int(w, h, 0), Instantiate(tree));
                }
            }
        }
    }

    public void CreateGround() {
        ground.ClearAllTiles();
        for (int w = 0; w < width; w++) {
            for (int h = 0; h < height; h++) {
                float xCoord = w;
                float yCoord = h;
                GridTile tile = Instantiate(grass);
                ground.AddTile(new Vector3Int((int) xCoord, (int) yCoord, 0), tile);
            }
        }
    }
    public void RandomizeOffsets() {
        xOff = Random.Range(0f, 1000f);
        yOff = Random.Range(0f, 1000f);
    }

    void Awake() {
        RandomizeOffsets();
        CreateGround();
        CalcNoise();
    }
}
