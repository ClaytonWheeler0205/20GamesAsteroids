using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Bus
{

    public class AsteroidEventBus : Node
    {
        public static AsteroidEventBus Instance {get; private set;}

        [Signal]
        public delegate void AsteroidDestroyed();

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