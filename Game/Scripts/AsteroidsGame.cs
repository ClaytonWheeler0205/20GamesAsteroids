using Game.Player;
using Godot;
using System;
using Util.ExtensionMethods;

namespace Game
{

    public class AsteroidsGame : Node
    {
        private Ship _ship;
        private const string SHIP_NODE_PATH = "PlayerController/PlayerShip";
        private AsteroidsManager _asteroidsManager;
        private const string ASTEROIDS_MANAGER_NODE_PATH = "AsteroidsManager";

        private int _level = 1;

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            _asteroidsManager.Ship = _ship;
            _asteroidsManager.SpawnAsteroids(_level);
        }

        private void SetNodeReferences()
        {
            _ship = GetNode<Ship>(SHIP_NODE_PATH);
            _asteroidsManager = GetNode<AsteroidsManager>(ASTEROIDS_MANAGER_NODE_PATH);
        }

        private void CheckNodeReferences()
        {
            if (!_ship.IsValid())
            {
                GD.PrintErr("ERROR: Ship is not valid!");
            }
            if (!_asteroidsManager.IsValid())
            {
                GD.PrintErr("ERROR: Asteroids manager is not valid!");
            }
        }

        public async void OnAsteroidsCleared()
        {
            _level++;
            await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
            _asteroidsManager.SpawnAsteroids(_level);
        }
    }
}