using System;
using System.Collections.Generic;
using UnityEngine;

internal class PlayerInputs : MonoBehaviour
{
    public static PlayerInputs Instance { get; private set; }

    public bool forward;
    public bool back;
    public bool left;
    public bool right;
    public bool jump;
    public bool shoot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        SetInputs();
    }

    private void SetInputs()
    {
        forward = Input.GetKey(KeyCode.W);
        back = Input.GetKey(KeyCode.S);
        left = Input.GetKey(KeyCode.A);
        right = Input.GetKey(KeyCode.D);
        jump = Input.GetKey(KeyCode.Space);
        shoot = Input.GetKey(KeyCode.Mouse0);
    }
}