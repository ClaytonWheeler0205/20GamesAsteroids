using Game.Bus;
using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Asteroid
{

    public abstract class Asteroid : Area2D
    {
        [Export]
        private int _pointValue;

        [Export]
        private float _driftSpeed = 200.0f;
        private Vector2 _driftDirection = Vector2.Up;
        private float _driftRotation = 0.0f;
        public float DriftRotation
        {
            get { return _driftRotation; }
            set
            {
                if (value < 0.0f)
                {
                    _driftRotation = 0.0f;
                }
                else if (value > 2 * Math.PI)
                {
                    _driftRotation = (float)(2.0 * Math.PI);
                }
                else
                {
                    _driftRotation = value;
                }
            }
        }

        private ScreenWrapper _screenWrapper;
        private const string SCREEN_WRAPPER_NODE_PATH = "VisibilityNotifier2D";

        private PackedScene _asteroidExplosion = GD.Load<PackedScene>("res://FX/Scenes/AsteroidExplosion.tscn");
        private PackedScene _asteroidExplosionSound = GD.Load<PackedScene>("res://FX/Scenes/OneShotAudio.tscn");
        private AudioStream _asteroidExplosionSoundEffect = GD.Load<AudioStream>("res://Asteroid/Audio/asteroid_explosion.wav");

        public override void _Ready()
        {
            Rotation = (float)GD.RandRange(0.0, 2.0 * Math.PI);
            SetNodeReferences();
            CheckNodeReferences();
            _screenWrapper.NodeToTrack = this;
        }

        private void SetNodeReferences()
        {
            _screenWrapper = GetNode<ScreenWrapper>(SCREEN_WRAPPER_NODE_PATH);
        }

        private void CheckNodeReferences()
        {
            if (!_screenWrapper.IsValid())
            {
                GD.PrintErr("ERROR: Screen wrapper is not valid!");
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            GlobalPosition += _driftDirection.Rotated(_driftRotation) * _driftSpeed * delta;
        }


        protected void Explode()
        {
            CPUParticles2D explosion = _asteroidExplosion.Instance<CPUParticles2D>();
            AudioStreamPlayer sound = _asteroidExplosionSound.Instance<AudioStreamPlayer>();
            explosion.GlobalPosition = GlobalPosition;
            sound.Stream = _asteroidExplosionSoundEffect;
            GetTree().Root.AddChild(explosion);
            GetTree().Root.AddChild(sound);

            ScoreEventBus.Instance.EmitSignal("AwardPoints", _pointValue);
            AsteroidEventBus.Instance.EmitSignal("AsteroidDestroyed");

            this.SafeQueueFree();
        }
    }
}