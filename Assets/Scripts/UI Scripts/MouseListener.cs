using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool lmb;
    private bool rmb;
    public TilePlacement tilePlacement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lmb) {
            tilePlacement.AddTile();
        }   

        if (rmb) {
            tilePlacement.RemoveTile();
        } 
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            lmb = true;
        }

        if (eventData.button == PointerEventData.InputButton.Right) {
            rmb = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            lmb = false;
        }

        if (eventData.button == PointerEventData.InputButton.Right) {
            rmb = false;
        }
    }
}
