using UnityEngine;

public enum DIRECTION
{
    LEFT, LEFTUP, LEFTDOWN,
    RIGHT, RIGHTUP, RIGHTDOWN,
    UP, DOWN, NONE
}

public static class Direction8
{
    public static Vector2 Tovector2(DIRECTION direction)
    {
        return direction switch
        {
            DIRECTION.UP => Vector2.up,
            DIRECTION.DOWN => Vector2.down,
            DIRECTION.LEFT => Vector2.left,
            DIRECTION.RIGHT => Vector2.right,
            DIRECTION.LEFTUP => new Vector2(-1f, 1f).normalized,
            DIRECTION.LEFTDOWN => new Vector2(-1f, -1f).normalized,
            DIRECTION.RIGHTUP => new Vector2(1f, 1f).normalized,
            DIRECTION.RIGHTDOWN => new Vector2(1f, -1f).normalized,
            DIRECTION.NONE => Vector2.zero,
            _ => Vector2.zero
        };
    }
}