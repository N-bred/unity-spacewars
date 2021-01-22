﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody _rigidBody;

    void Start()
    {
        Cursor.visible = false;
        _rigidBody = GetComponent<Rigidbody>();    
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        _rigidBody.MovePosition(new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 50)).x, _rigidBody.position.y, 0));
    }
}