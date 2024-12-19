using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Asteroid
{

    public class MediumAsteroid : Asteroid
    {

        private const double MAX_ANGLE_CHANGE = Math.PI / 4;

        private const string PLAYER_BULLET_NODE_GROUP = "PlayerBullet";
        private const string PLAYER_NODE_GROUP = "Player";

        private PackedScene _smallAsteroid = GD.Load<PackedScene>("res://Asteroid/Scenes/SmallAsteroid.tscn");

        public override void _Ready()
        {
            base._Ready();
            DriftRotation += (float)GD.RandRange(-MAX_ANGLE_CHANGE, MAX_ANGLE_CHANGE);
        }

        private void SplitAsteroid()
        {
            for (int i = 0; i < 2; i++)
            {
                Asteroid asteroid = _smallAsteroid.Instance<Asteroid>();
                asteroid.DriftRotation = DriftRotation;
                asteroid.GlobalPosition = GlobalPosition;
                GetTree().Root.CallDeferred("add_child", asteroid);
            }
        }

        public void OnArea2DEntered(Area2D area)
        {
            if (area.IsInGroup(PLAYER_BULLET_NODE_GROUP))
            {
                SplitAsteroid();
                Explode();
                area.SafeQueueFree();
            }
        }

        public void OnBodyEntered(Node body)
        {
            if (body.IsInGroup(PLAYER_NODE_GROUP))
            {
                SplitAsteroid();
                Explode();
            }
        }
    }
}