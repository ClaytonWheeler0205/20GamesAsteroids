using Godot;
using System;

namespace Game.Player
{

    public abstract class PlayerController : Node2D
    {
        private bool _isControllerActive = true;
        public bool IsControllerActive
        {
            get { return _isControllerActive; }
            set { _isControllerActive = value; }
        }
    }
}