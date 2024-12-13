using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.FX
{

    public class OneShotAudio : AudioStreamPlayer
    {

        public override void _Ready()
        {
            PitchScale = (float)GD.RandRange(0.95, 1.05);
        }

        public void OnAudioFinished()
        {
            this.SafeQueueFree();
        }
    }
}