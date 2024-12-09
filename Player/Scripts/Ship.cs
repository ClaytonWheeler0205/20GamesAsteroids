using Godot;
using System;

namespace Game.Player
{

    public abstract class Ship : KinematicBody2D
    {
        protected float rotationDirection = 0.0f;
        public float RotationDirection
        {
            set
            {
                rotationDirection = Mathf.Clamp(value, -1.0f, 1.0f);
            }
        }

        protected bool isMoving = false;
        public bool IsMoving
        {
            set { isMoving = value; }
        }
    }
}