using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Bus
{

    public class LivesEventBus : Node
    {

        public static LivesEventBus Instance { get; private set; }

        [Signal]
        public delegate void LoseLife();
        [Signal]
        public delegate void GainLife();

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