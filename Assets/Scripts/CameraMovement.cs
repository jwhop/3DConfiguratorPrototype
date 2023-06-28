using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance { get; private set; }
    private Vector3 prevMousePos, prevPosition;
    public bool is2D;

    protected Transform _XForm_Camera;
    protected Transform _XForm_Parent;

    protected Vector3 _LocalRotation;
    protected float _CameraDistance = 5f;

    public float MouseSensitivity = 2f;
    public float ScrollSensitvity = 2f;
    public float OrbitDampening = 10f;
    public float ScrollDampening = 6f;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        is2D = true;
        _XForm_Camera = transform;
        _XForm_Parent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (is2D)
        {
            
            //Rotation of the Camera based on Mouse Coordinates
            if (Input.GetMouseButton(0) && (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0))
            {
                _LocalRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
                _LocalRotation.y -= Input.GetAxis("Mouse Y") * MouseSensitivity;

                //Clamp the y Rotation to horizon and not flipping over at the top
                if (_LocalRotation.y < 0f)
                    _LocalRotation.y = 0f;
                else if (_LocalRotation.y > 90f)
                    _LocalRotation.y = 90f;
            }
            //Zooming Input from our Mouse Scroll Wheel
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitvity;

                ScrollAmount *= (this._CameraDistance * 0.3f);

                this._CameraDistance += ScrollAmount * -1f;

                this._CameraDistance = Mathf.Clamp(this._CameraDistance, 2.5f, 30f);
            }

            
            //Actual Camera Rig Transformations
            Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
            this._XForm_Parent.rotation = Quaternion.Lerp(this._XForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);

            if (this._XForm_Camera.localPosition.z != this._CameraDistance * -1f)
            {
                this._XForm_Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this._XForm_Camera.localPosition.z, this._CameraDistance * -1f, Time.deltaTime * ScrollDampening));
            }
            
        }
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
