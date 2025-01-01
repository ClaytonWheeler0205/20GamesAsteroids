using Game.Bus;
using Game.Player;
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

        private PackedScene _bullet = GD.Load<PackedScene>("res://Bullet/Scenes/UFOBullet.tscn");
        private AudioStream _bulletSound = GD.Load<AudioStream>("res://Bullet/Audio/ufo_bullet_fire.wav");
        private PackedScene _oneShotAudio = GD.Load<PackedScene>("res://FX/Scenes/OneShotAudio.tscn");

        private const string ASTEROID_NODE_GROUP = "Asteroid";
        private const string PLAYER_BULLET_NODE_GROUP = "PlayerBullet";
        private const string PLAYER_NODE_GROUP = "Player";

        private AudioStream _explosionSound = GD.Load<AudioStream>("res://UFO/Audio/ufo_explosion.wav");
        private PackedScene _ufoExplosion = GD.Load<PackedScene>("res://FX/Scenes/UFOExplosion.tscn");

        private AudioStreamPlayer _movementSound;
        private const string MOVEMENT_SOUND_NODE_PATH = "MovementSoundPlayer";

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
            _movementSound.PitchScale = (float)GD.RandRange(0.95, 1.05);
        }

        private void SetNodeReferences()
        {
            _screenWrapper = GetNode<ScreenWrapper>(SCREEN_WRAPPER_NODE_PATH);
            _movementTimer = GetNode<Timer>(MOVEMENT_TIMER_NODE_PATH);
            _movementSound = GetNode<AudioStreamPlayer>(MOVEMENT_SOUND_NODE_PATH);
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
            if (!_movementSound.IsValid())
            {
                GD.PrintErr("ERROR: UFO movement sound player is not valid!");
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

        public void OnFireRateTimerTimeout()
        {
            Node2D bullet = _bullet.Instance<Node2D>();
            bullet.GlobalPosition = GlobalPosition;
            AudioStreamPlayer bulletSoundPlayer = _oneShotAudio.Instance<AudioStreamPlayer>();
            bulletSoundPlayer.Stream = _bulletSound;
            bulletSoundPlayer.PitchScale = (float)GD.RandRange(0.95, 1.05);
            GetTree().Root.AddChild(bullet);
            GetTree().Root.AddChild(bulletSoundPlayer);
        }

        public void OnScreenExited()
        {
            if (_canBeDestroyed)
            {
                this.SafeQueueFree();
            }
        }

        public void OnAreaEntered(Area2D area)
        {
            if (area.IsInGroup(ASTEROID_NODE_GROUP))
            {
                Explode();
            }
            if (area.IsInGroup(PLAYER_BULLET_NODE_GROUP))
            {
                ScoreEventBus.Instance.EmitSignal("AwardPoints", _pointValue);
                Explode();
            }
        }

        public void OnBodyEntered(Node2D body)
        {
            if (body.IsInGroup(PLAYER_NODE_GROUP))
            {
                if (body is Ship ship)
                {
                    ship.Die();
                }
            }
        }

        private void Explode()
        {
            Node2D explosion = _ufoExplosion.Instance<Node2D>();
            explosion.GlobalPosition = GlobalPosition;
            GetTree().Root.AddChild(explosion);

            AudioStreamPlayer explosionSound = _oneShotAudio.Instance<AudioStreamPlayer>();
            explosionSound.Stream = _explosionSound;
            explosionSound.PitchScale = (float)GD.RandRange(0.95, 1.05);
            GetTree().Root.AddChild(explosionSound);

            this.SafeQueueFree();
        }
    }
}