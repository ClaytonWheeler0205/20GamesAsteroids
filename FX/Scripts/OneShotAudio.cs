using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.FX
{

    public class OneShotAudio : AudioStreamPlayer
    {
        public void OnAudioFinished()
        {
            this.SafeQueueFree();
        }
    }
}