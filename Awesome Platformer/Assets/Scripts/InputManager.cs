using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public event Action OnJump;

    public float horizontal;
    public float lastMoveHorizontal;

    bool useKeyboardInput = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (useKeyboardInput)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
        }

        if (Input.GetButtonDown("Jump") && useKeyboardInput)
        {
            OnJump.Invoke();
        }
    }
}
