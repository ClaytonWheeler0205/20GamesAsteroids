using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Asteroid
{

    public class SmallAsteroid : Asteroid
    {
        private const double MAX_ANGLE_CHANGE = Math.PI / 4;

        private const string PLAYER_BULLET_NODE_GROUP = "PlayerBullet";

        public override void _Ready()
        {
            base._Ready();
            DriftRotation += (float)GD.RandRange(-MAX_ANGLE_CHANGE, MAX_ANGLE_CHANGE);
        }

        public void OnArea2DEntered(Area2D area)
        {
            if (area.IsInGroup(PLAYER_BULLET_NODE_GROUP))
            {
                Explode();
                this.SafeQueueFree();
                area.SafeQueueFree();
            }
        }
    }
}