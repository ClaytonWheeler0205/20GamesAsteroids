using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Bus
{

    public class PlayerEventBus : Node
    {

        public static PlayerEventBus Instance { get; private set; }

        [Signal]
        public delegate void PlayerDestroyed();
        [Signal]
        public delegate void PlayerRespawn();

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            if (Instance != null && Instance != this)
            {
                this.SafeQueueFree();
            }
            else
            {
                Instance = this;
            }
        }
    }
}