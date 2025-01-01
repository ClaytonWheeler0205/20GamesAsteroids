using Game.UFO;
using Godot;
using System;
using Util;
using Util.ExtensionMethods;

namespace Game
{

    public class UFOManager : Node
    {

        private PackedScene _ufo = GD.Load<PackedScene>("res://UFO/Scenes/UFO.tscn");
        private const float SPAWN_X_LEFT = -34.0f;
        private const float SPAWN_X_RIGHT = 1060.0f;
        private const double MINIMUM_SPAWN_Y = 23.0;
        private const double MAXIMUM_SPAWN_Y = 585.0;

        private Timer _spawnTimer;
        private const string SPAWN_TIMER_NODE_PATH = "SpawnTimer";
        private const double MINIMUM_SPAWN_TIME = 10.0;
        private const double MAXIMUM_SPAWN_TIME = 20.0;

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            GD.Randomize();
            float randomTime = (float)GD.RandRange(MINIMUM_SPAWN_TIME, MAXIMUM_SPAWN_TIME);
            _spawnTimer.Start(randomTime);
        }

        private void SetNodeReferences()
        {
            _spawnTimer = GetNode<Timer>(SPAWN_TIMER_NODE_PATH);
        }

        private void CheckNodeReferences()
        {
            if (!_spawnTimer.IsValid())
            {
                GD.PrintErr("ERROR: UFO manager spawn timer is not valid!");
            }
        }

        public void OnSpawnTimerTimeout()
        {
            UFO.UFO ufo = _ufo.Instance<UFO.UFO>();
            int randomSpawnDirection = GDRandom.RandiRange(1, 2);
            float xPosition = 0;
            switch (randomSpawnDirection)
            {
                case 1:
                    ufo.UFOMovementDirection = UFO.UFO.UFODirection.Right;
                    xPosition = SPAWN_X_LEFT;
                    break;
                case 2:
                    ufo.UFOMovementDirection = UFO.UFO.UFODirection.Left;
                    xPosition = SPAWN_X_RIGHT;
                    break;
            }
            float yPosition = (float)GD.RandRange(MINIMUM_SPAWN_Y, MAXIMUM_SPAWN_Y);
            ufo.GlobalPosition = new Vector2(xPosition, yPosition);
            AddChild(ufo);

            GD.Randomize();
            float randomTime = (float)GD.RandRange(MINIMUM_SPAWN_TIME, MAXIMUM_SPAWN_TIME);
            _spawnTimer.Start(randomTime);
        }
    }
}