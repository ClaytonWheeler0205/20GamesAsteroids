using Game.Bus;
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

        private Node2D _muzzle;
        private const string MUZZLE_NODE_PATH = "Muzzle";

        private PackedScene _bullet = GD.Load<PackedScene>("res://Bullet/Scenes/PlayerBullet.tscn");

        private AudioStream _bulletSound = GD.Load<AudioStream>("res://Bullet/Audio/player_bullet_fire_2.wav");
        private PackedScene _bulletSoundEffect = GD.Load<PackedScene>("res://FX/Scenes/OneShotAudio.tscn");

        private bool _canFire = true;
        private Timer _cooldown;
        private const string COOLDOWN_NODE_PATH = "CooldownTimer";

        private AudioStreamPlayer _thrustSound;
        private const string THRUST_SOUND_NODE_PATH = "ThrustAudioPlayer";

        private CollisionPolygon2D _shipCollision;
        private const string SHIP_COLLISION_NODE_PATH = "CollisionPolygon2D";

        private AudioStreamPlayer _explosionSound;
        private const string EXPLOSION_SOUND_NODE_PATH = "ExplosionAudioPlayer";

        private PackedScene _playerExplosion = GD.Load<PackedScene>("res://FX/Scenes/PlayerExplosion.tscn");

        private Vector2 _startingPosition;


        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            _screenWrapper.NodeToTrack = this;
            _startingPosition = GlobalPosition;
        }

        private void SetNodeReferences()
        {
            _thrust = GetNode<AnimationPlayer>(THRUST_NODE_PATH);
            _screenWrapper = GetNode<ScreenWrapper>(SCREEN_WRAPPER_NODE_PATH);
            _muzzle = GetNode<Node2D>(MUZZLE_NODE_PATH);
            _cooldown = GetNode<Timer>(COOLDOWN_NODE_PATH);
            _thrustSound = GetNode<AudioStreamPlayer>(THRUST_SOUND_NODE_PATH);
            _shipCollision = GetNode<CollisionPolygon2D>(SHIP_COLLISION_NODE_PATH);
            _explosionSound = GetNode<AudioStreamPlayer>(EXPLOSION_SOUND_NODE_PATH);
        }

        public void CheckNodeReferences()
        {
            if (!_thrust.IsValid())
            {
                GD.PrintErr("ERROR: Thrust animation player is not valid!");
            }
            if (!_screenWrapper.IsValid())
            {
                GD.PrintErr("ERROR: Screen wrapper is not valid!");
            }
            if (!_muzzle.IsValid())
            {
                GD.PrintErr("ERROR: Muzzle is not valid!");
            }
            if (!_cooldown.IsValid())
            {
                GD.PrintErr("Error Cooldown timer is not valid!");
            }
            if (!_thrustSound.IsValid())
            {
                GD.PrintErr("ERROR: Thrust sound is not valid!");
            }
            if (!_shipCollision.IsValid())
            {
                GD.PrintErr("ERROR: Ship collision is not valid!");
            }
            if (!_explosionSound.IsValid())
            {
                GD.PrintErr("ERROR: Explosion sound is not valid!");
            }
        }

        public override void _Process(float delta)
        {
            if (isMoving)
            {
                _thrust.Play(THRUST_ANIMATION_NAME);
                if(!_thrustSound.Playing)
                {
                    _thrustSound.PitchScale = (float)GD.RandRange(0.95, 1.05);
                    _thrustSound.Play();
                }
            }
            else
            {
                _thrust.Play(THRUST_IDLE_ANIMATION_NAME);
                if (_thrustSound.Playing)
                {
                    _thrustSound.Stop();
                }
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

        public override void Fire()
        {
            if (_canFire)
            {
                Node2D bullet = _bullet.Instance<Node2D>();
                bullet.GlobalPosition = _muzzle.GlobalPosition;
                bullet.Rotation = Rotation;
                AudioStreamPlayer bulletSoundEffect = _bulletSoundEffect.Instance<AudioStreamPlayer>();
                bulletSoundEffect.Stream = _bulletSound;
                GetTree().Root.AddChild(bulletSoundEffect);
                GetTree().Root.AddChild(bullet);
                _canFire = false;
                _cooldown.Start();
            }
        }

        public override void Die()
        {
            Visible = false;
            _shipCollision.SetDeferred("disabled", true);
            _shipVelocity = Vector2.Zero;
            IsMoving = false;
            rotationDirection = 0.0f;

            _explosionSound.Play();
            Node2D explosion = _playerExplosion.Instance<Node2D>();
            explosion.GlobalPosition = GlobalPosition;
            GetTree().Root.AddChild(explosion);

            PlayerEventBus.Instance.EmitSignal("PlayerDestroyed");
            LivesEventBus.Instance.EmitSignal("LoseLife");
        }

        public override void Respawn()
        {
            GlobalPosition = _startingPosition;
            Rotation = 0.0f;
            Visible = true;
            _shipCollision.SetDeferred("disabled", false);
            PlayerEventBus.Instance.EmitSignal("PlayerRegainControl");
        }

        public void OnCooldownTimerTimeout()
        {
            _canFire = true;
        }
    }
}