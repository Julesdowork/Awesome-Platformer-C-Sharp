using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public event Action OnJump;

    public float horizontal;
    public float lastMoveHorizontal;

    bool useKeyboardInput = true;

    // Update is called once per frame
    void Update()
    {
        if (useKeyboardInput)
        {
            horizontal = Input.GetAxisRaw(TagManager.Horizontal);
        }

        if (Input.GetButtonDown(TagManager.Jump) && useKeyboardInput)
        {
            OnJump.Invoke();
        }
    }
}
