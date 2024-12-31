using Godot;
using System;
using Util;
using Util.ExtensionMethods;

namespace Game.UFO
{

    public class UFOImpl : UFO
    {
        [Export]
        private int _pointValue = 200;

        [Export]
        private float _speed = 200.0f;

        private ScreenWrapper _screenWrapper;
        private const string SCREEN_WRAPPER_NODE_PATH = "VisibilityNotifier2D";
        private Timer _movementTimer;
        private const string MOVEMENT_TIMER_NODE_PATH = "MovementTimer";

        private float _xDirection = 0.0f;
        private float _yDirection = 0.0f;

        private bool _canBeDestroyed = false;

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            _screenWrapper.NodeToTrack = this;

            switch (UFOMovementDirection)
            {
                case UFODirection.Left:
                    _xDirection = -1;
                    break;
                case UFODirection.Right:
                    _xDirection = 1;
                    break;
            }

            float randomTime = (float)GD.RandRange(1.0, 2.0);
            _movementTimer.Start(randomTime);
        }

        private void SetNodeReferences()
        {
            _screenWrapper = GetNode<ScreenWrapper>(SCREEN_WRAPPER_NODE_PATH);
            _movementTimer = GetNode<Timer>(MOVEMENT_TIMER_NODE_PATH);
        }

        private void CheckNodeReferences()
        {
            if (!_screenWrapper.IsValid())
            {
                GD.PrintErr("ERROR: UFO screen wrapper is not valid!");
            }
            if (!_movementTimer.IsValid())
            {
                GD.PrintErr("ERROR: UFO movement time is not valid!");
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            Vector2 velocity = new Vector2(_xDirection, _yDirection);
            velocity = velocity.Normalized();

            GlobalPosition += velocity * _speed * delta;
        }

        public void OnMovementTimerTimeout()
        {
            _yDirection = GDRandom.RandiRange(0, 2) - 1;
            float randomTime = (float)GD.RandRange(1.0, 2.0);
            _movementTimer.Start(randomTime);
        }

        public void OnLifetimeTimerTimeout()
        {
            _canBeDestroyed = true;
        }

        public void OnScreenExited()
        {
            if (_canBeDestroyed)
            {
                this.SafeQueueFree();
            }
        }
    }
}