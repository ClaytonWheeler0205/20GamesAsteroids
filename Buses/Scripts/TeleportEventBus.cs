using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Bus
{

    public class TeleportEventBus : Node
    {
        public static TeleportEventBus Instance { get; private set; }

        [Signal]
        public delegate void TeleportOn();
        [Signal]
        public delegate void TeleportOff();

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