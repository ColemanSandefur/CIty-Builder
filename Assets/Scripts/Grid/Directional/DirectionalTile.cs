using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rotation {NORTH, EAST, SOUTH, WEST}
public class DirectionalTile : GridTile
{
    public Rotation rotation {get; protected set;}
    public Sprite[] m_Sprites;

    public void SetRotation(Rotation rotation) {
        this.rotation = rotation;
        spriteRenderer.sprite = m_Sprites[(int) rotation];
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
