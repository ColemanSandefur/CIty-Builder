using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = .1f;
    enum Direction {UP, RIGHT, DOWN, LEFT, ZOOM_IN, ZOOM_OUT};
    public Camera controlCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetKey(KeyCode.W)) {
            MoveCamera(0, .1f, 0);
        } 
        if (Input.GetKey(KeyCode.D)) {
            MoveCamera(0.1f, 0f, 0);
        }
        if (Input.GetKey(KeyCode.S)) {
            MoveCamera(0, -.1f, 0);
        }
        if (Input.GetKey(KeyCode.A)) {
            MoveCamera(-0.1f, 0f, 0);
        }
        if (scrollWheel > 0f) { //Scroll up
            MoveCamera(0, 0, .5f);
        }
        if (scrollWheel < 0f) { //Scroll down
            MoveCamera(0, 0, -.5f);
        }

        if (Input.GetKey(KeyCode.Mouse2)) {
            float xAxis = Input.GetAxis("Mouse X") * speed;
            float yAxis = Input.GetAxis("Mouse Y") * speed;
            Debug.Log($"xAxis: {xAxis}; yAxis: {yAxis}");
            MoveCamera(xAxis, yAxis, 0);
        }
    }

    void MoveCamera(float x, float y, float z) {
        Vector3 pos = controlCamera.transform.localPosition;
        controlCamera.transform.localPosition = pos + new Vector3(x, y, z);
    }
}
