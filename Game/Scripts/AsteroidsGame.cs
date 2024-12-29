using Game.Bus;
using Game.Player;
using Godot;
using System;
using System.Runtime.CompilerServices;
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
        private Control _waitingUI;
        private const string WAITING_UI_NODE_PATH = "GUI/WaitingUI";
        private Control _gameOverUI;
        private const string GAME_OVER_UI_NODE_PATH = "GUI/GameOverUI";

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
            _waitingUI = GetNode<Control>(WAITING_UI_NODE_PATH);
            _gameOverUI = GetNode<Control>(GAME_OVER_UI_NODE_PATH);
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
            if (!_waitingUI.IsValid())
            {
                GD.PrintErr("ERROR: Waiting UI is not valid!");
            }
            if (!_gameOverUI.IsValid())
            {
                GD.PrintErr("ERROR: Game over UI is not valid!");
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
                _waitingUI.Visible = true;
                await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
            }
            _waitingUI.Visible = false;
            _ship.Respawn();
        }

        public async void OnGameOver()
        {
            _gameOverUI.Visible = true;
            await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
            ClearAsteroids();
            GetTree().ChangeScene("res://Game/Scenes/MainMenu.tscn");
        }

        private void ClearAsteroids()
        {
            Node root = GetTree().Root;
            for (int i = 0; i < root.GetChildCount(); i++)
            {
                Node childNode = root.GetChild(i);
                if (childNode.IsInGroup("Asteroid"))
                {
                    childNode.SafeQueueFree();
                }
            }
        }
    }
}