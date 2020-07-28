using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridTile : MonoBehaviour
{
    public Vector3Int location;
    public Sprite sprite;

    public SpriteRenderer spriteRenderer;
    
    public virtual void SetSprite(Sprite sprite) {
        this.sprite = sprite;
        spriteRenderer.sprite = sprite;
    }

    public virtual void SetLocation(Vector3Int location) {
        this.location = location;
    }

    public virtual void Refresh(Vector3Int location, Grid grid) {
        
    }

    void Start() {

    }

    void Update() {

    }
}
