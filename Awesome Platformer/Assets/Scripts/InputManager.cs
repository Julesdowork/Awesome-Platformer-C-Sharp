using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public event Action OnJump;
    public event Action OnShiftPressed;
    public event Action OnShiftReleased;

    public float horizontal;
    public float lastMoveHorizontal;
    // public bool isSprinting { get; private set; }
    public bool shiftPressed { get; private set; }

    [SerializeField] bool useKeyboardInput = true;

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (useKeyboardInput)
            horizontal = Input.GetAxisRaw(TagManager.Horizontal);

        if (Input.GetButtonDown(TagManager.Sprint) && useKeyboardInput)
        {
            shiftPressed = true;
            OnShiftPressed.Invoke();
        }
        if (Input.GetButtonUp(TagManager.Sprint))
        {
            shiftPressed = false;
            OnShiftReleased.Invoke();
        }

        if (Input.GetButtonDown(TagManager.Jump) && useKeyboardInput)
            OnJump.Invoke();
    }

    public void SetHorizontalMovement(float amount)
    {
        horizontal = amount;
    }

    public void TriggerJump()
    {
        OnJump.Invoke();
    }
}
