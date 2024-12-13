using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Asteroid
{

    public class LargeAsteroid : Asteroid
    {

        private const string  PLAYER_BULLET_NODE_GROUP = "PlayerBullet";

        private PackedScene _mediumAsteroid = GD.Load<PackedScene>("res://Asteroid/Scenes/MediumAsteroid.tscn");

        public override void _Ready()
        {
            base._Ready();
            DriftRotation = (float)GD.RandRange(0, 2.0*Math.PI);
        }

        public void OnArea2DEntered(Area2D area)
        {
            if (area.IsInGroup(PLAYER_BULLET_NODE_GROUP))
            {
                for (int i = 0; i < 2; i++)
                {
                    Asteroid asteroid = _mediumAsteroid.Instance<Asteroid>();
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