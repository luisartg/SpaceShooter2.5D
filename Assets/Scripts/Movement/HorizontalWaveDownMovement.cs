using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Movement
{
    class HorizontalWaveDownMovement : IMovement
    {
        private float _startTime;
        private float _arc;
        private float _cycle;

        public void SetProperties(float arcInDegrees, float cycleInSeconds)
        {
            _startTime = Time.time;
            _arc = arcInDegrees;
            _cycle = cycleInSeconds;
        }

        public Vector2 GetMovementVector()
        {
            var direction = new Vector2();
            float passedTimeInCyle = (Time.time - _startTime) % _cycle;
            float arcBegin = 270 - _arc / 2;
            float arcPart = (passedTimeInCyle * _arc * 2) / _cycle;
            if (arcPart >= _arc)
            {
                arcPart = 2 * _arc - arcPart; // we are going backwards
            }
            float theta = arcBegin + arcPart;
            direction.x = Mathf.Cos(GetRadians(theta));
            direction.y = Mathf.Sin(GetRadians(theta));
            return direction;
        }

        private float GetRadians(float degrees)
        {
            return degrees * Mathf.PI / 180;
        }
    }
}
