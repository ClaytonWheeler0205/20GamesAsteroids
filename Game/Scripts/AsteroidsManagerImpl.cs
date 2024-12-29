using Game.Bus;
using Godot;
using System;
using Util.ExtensionMethods;

namespace Game
{

    public class AsteroidsManagerImpl : AsteroidsManager
    {
        private const int BASE_NUMBER_OF_ASTEROIDS = 3;

        private const float MINIMUM_SHIP_RANGE = 175.0f;

        private const double MINIMUM_ASTEROID_X = 75.0;
        private const double MAXIMUM_ASTEROID_X = 950.0;
        private const double MINIMUM_ASTEROID_Y = 50.0;
        private const double MAXIMUM_ASTEROID_Y = 550.0;

        private PackedScene _asteroid = GD.Load<PackedScene>("res://Asteroid/Scenes/LargeAsteroid.tscn");

        private int _asteroidsCount = 0;
        private const int ASTEROIDS_PER_LARGE_ASTEROID = 7;

        [Signal]
        public delegate void AsteroidsCleared();

        public override void _Ready()
        {
            SetNodeConnections();
        }

        private void SetNodeConnections()
        {
            AsteroidEventBus.Instance.Connect("AsteroidDestroyed", this, "OnAsteroidDestroyed");
        }

        public override void SpawnAsteroids(int level)
        {
            GD.Randomize();
            int asteroidsToSpawn = level + BASE_NUMBER_OF_ASTEROIDS;
            for (int i = 0; i < asteroidsToSpawn; i++)
            {
                Node2D asteroid = _asteroid.Instance<Node2D>();

                float randomXPosition = (float)GD.RandRange(MINIMUM_ASTEROID_X, MAXIMUM_ASTEROID_X);
                while (Math.Abs(randomXPosition - Ship.GlobalPosition.x) < MINIMUM_SHIP_RANGE)
                {
                    randomXPosition = (float)GD.RandRange(MINIMUM_ASTEROID_X, MAXIMUM_ASTEROID_X);
                }

                float randomYPosition = (float)GD.RandRange(MINIMUM_ASTEROID_Y, MAXIMUM_ASTEROID_Y);
                while (Math.Abs(randomYPosition - Ship.GlobalPosition.y) < MINIMUM_SHIP_RANGE)
                {
                    randomYPosition = (float)GD.RandRange(MINIMUM_ASTEROID_Y, MAXIMUM_ASTEROID_Y);
                }

                asteroid.GlobalPosition = new Vector2(randomXPosition, randomYPosition);
                AddChild(asteroid);
            }
            _asteroidsCount = ASTEROIDS_PER_LARGE_ASTEROID * asteroidsToSpawn;
        }

        public void OnAsteroidDestroyed()
        {
            _asteroidsCount--;
            if (_asteroidsCount == 0)
            {
                EmitSignal("AsteroidsCleared");
            }
        }
    }
}