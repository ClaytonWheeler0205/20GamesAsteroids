using Godot;
using System;

namespace Game.Asteroid
{

    public class LargeAsteroid : Asteroid
    {
        public override void _Ready()
        {
            base._Ready();
            DriftRotation = (float)GD.RandRange(0, 2.0*Math.PI);
        }
    }
}