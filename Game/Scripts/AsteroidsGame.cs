using Game.Bus;
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
        private PlayerSpawnArea _spawnArea;
        private const string PLAYER_SPAWN_AREA_NODE_PATH = "PlayerSpawnArea";

        private int _level = 1;

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            SetNodeConnections();
            _asteroidsManager.Ship = _ship;
            _asteroidsManager.SpawnAsteroids(_level);
        }

        private void SetNodeReferences()
        {
            _ship = GetNode<Ship>(SHIP_NODE_PATH);
            _asteroidsManager = GetNode<AsteroidsManager>(ASTEROIDS_MANAGER_NODE_PATH);
            _spawnArea = GetNode<PlayerSpawnArea>(PLAYER_SPAWN_AREA_NODE_PATH);
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
            if (!_spawnArea.IsValid())
            {
                GD.PrintErr("ERROR: Spawn area is not valid!");
            }
        }

        private void SetNodeConnections()
        {
            PlayerEventBus.Instance.Connect("PlayerRespawn", this, nameof(OnPlayerRespawn));
        }

        public async void OnAsteroidsCleared()
        {
            _level++;
            await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
            _asteroidsManager.SpawnAsteroids(_level);
        }

        public async void OnPlayerRespawn()
        {
            await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
            while (!_spawnArea.AreaIsEmpty())
            {
                await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
            }
            _ship.Respawn();
        }
    }
}