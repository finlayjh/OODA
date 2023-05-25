using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //private Touch initTouch = new Touch();
    private float rotX = 0f;
    private float rotY = 0f;
    private Vector3 origRot;
    private float turnSpeed;

    //player cam movement
    public float rotSpeed = 0.5f;

    //observe phase 360 tracking
    private float _currentAngle = 0;
    private Vector3 _startingForward;
    private Vector3 _startingUp;
    private bool _lookedFrontRight = false;
    private bool _lookedFrontLeft = false;
    private bool _lookedBackRight = false;
    private bool _lookedBackLeft = false;

    private void Start()
    {
        _startingForward = transform.forward;
        _startingUp =  transform.up;
        origRot = gameObject.transform.eulerAngles;
        rotX = origRot.x;
        rotY = origRot.y;
    }

    private void Update()
    {
        
        /*foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                initTouch.phase = initTouch;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                //swiping
                float deltaX = initTouch.position.x - touch.position.x;
                float deltaY = initTouch.position.y - touch.position.y;
                rotX -= deltaY * rotSpeed * Time.deltaTime * dir;
                rotY += deltaX * rotSpeed * Time.deltaTime * dir;
                Mathf.Clamp(rotX, -80f, 80f);
                cam.transform.eulerAngles = new Vector3(rotX, rotY, 0f);
            }

            else if (touch.phase == TouchPhase.Ended)
            {
                initTouch = new Touch();
            }
        }*/
        if (Input.GetMouseButton(0) && GameManager.instance.getCanInteract())
        {
            transform.eulerAngles += rotSpeed * new Vector3(x: -Input.GetAxis("Mouse Y"), y: Input.GetAxis("Mouse X"), z: 0);
            if (GameManager.instance.getGameState() == GameManager.GameState.Orient) //if ORIENT gameState
            {
                track360(Vector3.SignedAngle(_startingForward, transform.forward, _startingUp));
            }
        }
    }

    public void QuizLookAt(Transform t)
    {
        transform.LookAt(t);
        transform.eulerAngles += new Vector3(x: 0, y: -20, z: 0);
    }

    private void track360(float delta)
    {
        _currentAngle = delta;
        if(_currentAngle > 35 && _currentAngle < 55)
        {
            _lookedFrontRight = true;
        }
        else if (_currentAngle > 125 && _currentAngle < 135)
        {
            _lookedBackRight = true;
        }
        else if (_currentAngle < -35 && _currentAngle > -55)
        {
            _lookedBackLeft = true;
        }
        else if (_currentAngle < -125 && _currentAngle > -135)
        {
            _lookedFrontLeft = true;
        }
        Debug.Log(_currentAngle);
        if(_lookedFrontLeft && _lookedFrontRight && _lookedBackLeft && _lookedBackRight)
        {
            GameManager.instance.StageComplete();
        }
    }
}
