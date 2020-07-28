using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    [SerializeField] List<GameObject> objects = new List<GameObject>();
    public Canvas canvas;
    public float tileSize = .20f; //Percent
    public float paddingLeft;
    public float paddingRight;
    public float paddingTop;
    public float paddingBottom;
    // Start is called before the first frame update
    void Start()
    {
        float targetWidth = canvas.GetComponent<RectTransform>().rect.width * tileSize;

        foreach (GameObject gameObject in objects) {
            RectTransform transform = (RectTransform)gameObject.transform;
            float width = transform.rect.width;
            float height = transform.rect.height;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
