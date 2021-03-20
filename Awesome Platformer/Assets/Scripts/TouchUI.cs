using UnityEngine;

public class TouchUI : MonoBehaviour
{
    public void MoveLeft()
    {
        InputManager.instance.SetHorizontalMovement(-1f);
    }

    public void MoveRight()
    {
        InputManager.instance.SetHorizontalMovement(1f);
    }

    public void StopMoving()
    {
        InputManager.instance.SetHorizontalMovement(0);
    }

    public void Jump()
    {
        InputManager.instance.TriggerJump();
    }
}
