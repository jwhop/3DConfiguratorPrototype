using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 prevMousePos, prevPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            prevMousePos = Input.mousePosition;
            prevPosition = transform.position;
        }
        else if (Input.GetMouseButton(1))
        {
            Vector3 mouseDelta = Input.mousePosition - prevMousePos;
            mouseDelta /= (1000f * 1/Camera.main.orthographicSize);
            transform.position = new Vector3(prevPosition.x - mouseDelta.x, prevPosition.y, prevPosition.z - mouseDelta.y);
        }
        
    }
}
