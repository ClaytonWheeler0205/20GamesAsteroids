using Godot;
using System;

namespace Game.UFO
{

    public abstract class UFO : Area2D
    {
        public enum UFODirection
        {
            Left,
            Right
        }

        private UFODirection _ufoMovementDirection = UFODirection.Right;
        public UFODirection UFOMovementDirection
        {
            get { return _ufoMovementDirection; }
            set { _ufoMovementDirection = value;}
        }
    }
}