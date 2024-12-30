using Game.Bus;
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
            SetNodeConnections();
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

        private void SetNodeConnections()
        {
            PlayerEventBus.Instance.Connect("PlayerDestroyed", this, nameof(OnPlayerDestroyed));
            PlayerEventBus.Instance.Connect("PlayerRegainControl", this, nameof(OnPlayerRegainControl));
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
            if (@event.IsActionPressed("fire") && IsControllerActive)
            {
                _shipToControl.Fire();
            }
            if (@event.IsActionPressed("thrust") && IsControllerActive)
            {
                _shipToControl.IsMoving = true;
            }
            if (@event.IsActionReleased("thrust"))
            {
                _shipToControl.IsMoving = false;
            }
            if (@event.IsActionPressed("teleport") && IsControllerActive)
            {
                _shipToControl.Teleport();
            }
        }

        public void OnPlayerDestroyed()
        {
            IsControllerActive = false;
        }

        public void OnPlayerRegainControl()
        {
            IsControllerActive = true;
        }
    }
}