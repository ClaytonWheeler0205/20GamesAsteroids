using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Bullet
{

    public class PlayerBullet : Projectile
    {
        private AnimationPlayer _bulletSpin;
        private const string BULLET_SPIN_PATH = "AnimationPlayer";
        private const string BULLET_SPIN_ANIMATION_NAME = "BulletSpin";

        private ScreenWrapper _screenWrapper;
        private const string SCREEN_WRAPPER_NODE_PATH = "VisibilityNotifier2D";

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            _bulletSpin.Play(BULLET_SPIN_ANIMATION_NAME);
            _screenWrapper.NodeToTrack = this;
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
        
        public void OnTimerTimeout()
        {
            this.SafeQueueFree();
        }
    }
}