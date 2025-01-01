using Godot;
using System;
using Util.ExtensionMethods;
using Game.Player;

namespace Game.Bullet
{

    public class UFOBullet : Projectile
    {
        private AnimationPlayer _bulletSpin;
        private const string BULLET_SPIN_PATH = "AnimationPlayer";
        private const string BULLET_SPIN_ANIMATION_NAME = "BulletSpin";

        private ScreenWrapper _screenWrapper;
        private const string SCREEN_WRAPPER_NODE_PATH = "VisibilityNotifier2D";

        private const string PLAYER_NODE_GROUP = "Player";

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            _bulletSpin.Play(BULLET_SPIN_ANIMATION_NAME);
            _screenWrapper.NodeToTrack = this;
            Rotation = (float)GD.RandRange(0, 2*Math.PI);
        }

        private void SetNodeReferences()
        {
            _bulletSpin = GetNode<AnimationPlayer>(BULLET_SPIN_PATH);
            _screenWrapper = GetNode<ScreenWrapper>(SCREEN_WRAPPER_NODE_PATH);
        }

        private void CheckNodeReferences()
        {
            if (!_bulletSpin.IsValid())
            {
                GD.PrintErr("ERROR: Bullet spin is not valid!");
            }
            if (!_screenWrapper.IsValid())
            {
                GD.PrintErr("ERROR: Screen wrapper is not valid!");
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
        
        public void OnTimerTimeout()
        {
            this.SafeQueueFree();
        }
    }
}