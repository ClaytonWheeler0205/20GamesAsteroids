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
        private const string THRUST_IDLE_ANIMATION_NAME = "ShipThrustIdle";

        private ScreenWrapper _screenWrapper;
        private const string SCREEN_WRAPPER_NODE_PATH = "VisibilityNotifier2D";


        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            _screenWrapper.NodeToTrack = this;
        }

        private void SetNodeReferences()
        {
            _thrust = GetNode<AnimationPlayer>(THRUST_NODE_PATH);
            _screenWrapper = GetNode<ScreenWrapper>(SCREEN_WRAPPER_NODE_PATH);
        }

        public void CheckNodeReferences()
        {
            if (!_thrust.IsValid())
            {
                GD.PrintErr("ERROR: Thrust animation player is not valid!");
            }
            if (!_screenWrapper.IsValid())
            {
                GD.PrintErr("ERROR: Screen wrapper is not valid");
            }
        }

        public override void _Process(float delta)
        {
            if (isMoving)
            {
                _thrust.Play(THRUST_ANIMATION_NAME);
            }
            else
            {
                _thrust.Play(THRUST_IDLE_ANIMATION_NAME);
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