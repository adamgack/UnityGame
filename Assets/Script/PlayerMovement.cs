using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _controller;
    private Vector3 _velocity;

    public float Gravity = -9.81f;
    public Vector3 Drag;
    public bool isGrounded = true;

    public float maxSpeed = 1f;
    public float acceleration = 2f;
    public float JumpHeight = 100f;

    public float drag = 0f;
    public float minDrag = 1f;
    public float maxDrag = 0.8f;

    public float minFov = 60;
    public float maxFov = 90;

    Camera playerCamera;

    public GameObject groundDetector;
    public GameObject arms;
    public GameObject leftArm;
    public GameObject rightArm;

    public float armRotation = 0f;
    public float armVelocity = 0f;
    public float armAcceleration = 3f;

    public float lookSensitivity = 10f;
    private bool jumping;
    private float _previousVerticalPosition = 0f;


    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * lookSensitivity, 0), Space.World);
        Move();
    }

    private void ProcessInputs()
    {
    }

    private void FixedUpdate()
    {
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    void Move()
    {
        if (isGrounded && _velocity.y < 0)
        {
            _velocity.y = 0f;
        }

        _previousVerticalPosition = transform.position.y;

        float distance = acceleration * Time.deltaTime;
        
        Vector3 movementDirection = GetMovementDirection();

        if (PlayerInputs.Instance.jump && isGrounded)
        {
            _velocity.y += Mathf.Sqrt(JumpHeight * -2f * Gravity);
            isGrounded = false;
        }

        var move = movementDirection.normalized * distance;

        move = Vector3.ClampMagnitude(move, maxSpeed * Time.deltaTime);

        _controller.Move(move);
        
        if (transform.position.y > _previousVerticalPosition)
        {
            var deltaVerticalPosition = transform.position.y - _previousVerticalPosition;
            _velocity += new Vector3(0, deltaVerticalPosition, 0);
        }

        playerCamera.fieldOfView = Mathf.Lerp(minFov, maxFov, (_velocity.magnitude / maxSpeed) / Time.deltaTime);

        armVelocity += _velocity.magnitude * armAcceleration;

        leftArm.transform.Rotate(new Vector3(armVelocity, 0, 0));

        rightArm.transform.Rotate(new Vector3(armVelocity, 0, 0));

        armVelocity *= 0.90f;

        if (!isGrounded)
        {
            _velocity.y += Gravity * Time.deltaTime;
        }

        _velocity.x /= 1 + Drag.x * Time.deltaTime;
        _velocity.y /= 1 + Drag.y * Time.deltaTime;
        _velocity.z /= 1 + Drag.z * Time.deltaTime;

        _controller.Move(_velocity * Time.deltaTime);

        Debug.Log(_velocity.y);
    }

    private Vector3 GetMovementDirection()
    {
        Vector3 movementDirection = Vector3.zero;
        if (PlayerInputs.Instance.forward)
        {
            movementDirection += gameObject.transform.forward;
        }
        if (PlayerInputs.Instance.left)
        {
            movementDirection -= gameObject.transform.right;
        }
        if (PlayerInputs.Instance.back)
        {
            movementDirection -= gameObject.transform.forward;
        }
        if (PlayerInputs.Instance.right)
        {
            movementDirection += gameObject.transform.right;
        }

        return movementDirection;
    }
}
