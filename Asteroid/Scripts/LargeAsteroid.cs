using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Asteroid
{

    public class LargeAsteroid : Asteroid
    {

        private const string PLAYER_BULLET_NODE_GROUP = "PlayerBullet";
        private const string PLAYER_NODE_GROUP = "Player";

        private PackedScene _mediumAsteroid = GD.Load<PackedScene>("res://Asteroid/Scenes/MediumAsteroid.tscn");

        public override void _Ready()
        {
            base._Ready();
            DriftRotation = (float)GD.RandRange(0, 2.0 * Math.PI);
        }

        private void SplitAsteroid()
        {
            for (int i = 0; i < 2; i++)
            {
                Asteroid asteroid = _mediumAsteroid.Instance<Asteroid>();
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