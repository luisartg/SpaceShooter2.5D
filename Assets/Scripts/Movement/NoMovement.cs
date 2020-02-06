using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Movement
{
    public class NoMovement : IMovement
    {
        public Vector2 GetMovementVector()
        {
            return new Vector2(0, 0);
        }
    }
}
