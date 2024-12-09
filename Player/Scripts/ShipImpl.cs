using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Player
{

    public class ShipImpl : Ship
    {
        [Export]
        private float _shipAcceleration = 20.0f;
        [Export]
        private float _maxSpeed = 150.0f;
        [Export]
        private float _roationSpeed = 150.0f;

        private Vector2 _shipDirection = Vector2.Up;
        private Vector2 _shipVelocity = Vector2.Zero;

        private AnimationPlayer _thrust;
        private const string THRUST_NODE_PATH = "AnimationPlayer";
        private const string THRUST_ANIMATION_NAME = "ShipThrust";

        public override void _Ready()
        {
            SetNodeReferences();
        }

        private void SetNodeReferences()
        {
            _thrust = GetNode<AnimationPlayer>(THRUST_NODE_PATH);
        }

        public void CheckNodeReferences()
        {
            if (_thrust.IsValid())
            {
                GD.PrintErr("ERROR: Thrust animation player is not valid!");
            }
        }

        public override void _Process(float delta)
        {
            if (isMoving && !_thrust.IsPlaying())
            {
                _thrust.Play(THRUST_ANIMATION_NAME);
            }
            else if (!isMoving && _thrust.IsPlaying())
            {
                _thrust.Stop();
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            Rotate(Mathf.Deg2Rad(rotationDirection * _roationSpeed * delta));

            if (isMoving)
            {
                _shipVelocity += _shipDirection.Rotated(Rotation) * _shipAcceleration;
                _shipVelocity = _shipVelocity.LimitLength(_maxSpeed);
            }
            else
            {
                _shipVelocity = _shipVelocity.MoveToward(Vector2.Zero, 1);
            }

            MoveAndSlide(_shipVelocity);
        }
    }
}