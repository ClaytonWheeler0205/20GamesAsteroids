using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.FX
{

    public class OneShotVFX : CPUParticles2D
    {

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Emitting = true;
        }

        public override void _Process(float delta)
        {
            if (!Emitting)
            {
                this.SafeQueueFree();
            }
        }
    }
}