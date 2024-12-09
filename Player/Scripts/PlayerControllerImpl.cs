using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Player
{

    public class PlayerControllerImpl : PlayerController
    {

        private Ship _shipToControl;
        private const string SHIP_NODE_PATH = "PlayerShip";

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
        }

        private void SetNodeReferences()
        {
            _shipToControl = GetNode<Ship>(SHIP_NODE_PATH);
        }

        private void CheckNodeReferences()
        {
            if (!_shipToControl.IsValid())
            {
                GD.PrintErr("ERROR: Ship node is not valid!");
            }
        }

        public override void _Process(float delta)
        {
            if (IsControllerActive)
            {
                _shipToControl.RotationDirection = Input.GetAxis("rotate_left", "rotate_right");
            }
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed("thrust"))
            {
                _shipToControl.IsMoving = true;
            }
            if (@event.IsActionReleased("thrust"))
            {
                _shipToControl.IsMoving = false;
            }
        }
    }
}