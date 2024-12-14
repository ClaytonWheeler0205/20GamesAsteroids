using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Asteroid
{

    public class MediumAsteroid : Asteroid
    {

        private const double MAX_ANGLE_CHANGE = Math.PI / 4;

        private const string PLAYER_BULLET_NODE_GROUP = "PlayerBullet";

        private PackedScene _smallAsteroid = GD.Load<PackedScene>("res://Asteroid/Scenes/SmallAsteroid.tscn");

        public override void _Ready()
        {
            base._Ready();
            DriftRotation += (float)GD.RandRange(-MAX_ANGLE_CHANGE, MAX_ANGLE_CHANGE);
        }

        public void OnArea2DEntered(Area2D area)
        {
            if (area.IsInGroup(PLAYER_BULLET_NODE_GROUP))
            {
                for (int i = 0; i < 2; i++)
                {
                    Asteroid asteroid = _smallAsteroid.Instance<Asteroid>();
                    asteroid.DriftRotation = DriftRotation;
                    asteroid.GlobalPosition = GlobalPosition;
                    GetTree().Root.CallDeferred("add_child", asteroid);
                }
                Explode();
                this.SafeQueueFree();
                area.SafeQueueFree();
            }
        }
    }
}