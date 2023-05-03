using UnityEngine;

public static class Pointer
{
    public static Vector2 GetPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}