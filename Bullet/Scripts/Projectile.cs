using Godot;
using System;

namespace Game.Bullet
{

    public abstract class Projectile : Area2D
    {
        [Export]
        private float _projectileSpeed = 500.0f;
        private Vector2 _projectileDirection = Vector2.Up;

        public override void _PhysicsProcess(float delta)
        {
            GlobalPosition += _projectileDirection.Rotated(Rotation) * _projectileSpeed * delta;
        }
    }
}