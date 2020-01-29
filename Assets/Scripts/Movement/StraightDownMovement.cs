using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Movement;

public class StraightDownMovement : IMovement
{
    public Vector2 GetMovementVector()
    {
        return Vector2.down;
    }
}
