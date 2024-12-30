using Game.Bus;
using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.Player
{

    public class TeleportImpl : Teleport
    {
        private const double MINIMUM_POSITION_X = 75.0;
        private const double MAXIMUM_POSITION_X = 950.0;
        private const double MINIMUM_POSITION_Y = 50.0;
        private const double MAXIMUM_POSITION_Y = 550.0;

        private bool _canTeleport = true;
        [Export]
        private float _cooldownTime = 5.0f;
        private Timer _cooldown;
        private const string COOLDOWN_NODE_PATH = "Timer";
        private AudioStreamPlayer _teleportSoundPlayer;
        private const string TELEPORT_SOUND_PLAYER_NODE_PATH = "AudioStreamPlayer";

        private AudioStream _teleportUseSound = GD.Load<AudioStream>("res://Player/Audio/player_teleport_asteroids.wav");
        private AudioStream _teleportCooldownEndSound = GD.Load<AudioStream>("res://Player/Audio/teleport_cooldown_end_asteroids.wav");

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
        }

        private void SetNodeReferences()
        {
            _cooldown = GetNode<Timer>(COOLDOWN_NODE_PATH);
            _teleportSoundPlayer = GetNode<AudioStreamPlayer>(TELEPORT_SOUND_PLAYER_NODE_PATH);
        }

        private void CheckNodeReferences()
        {
            if (!_cooldown.IsValid())
            {
                GD.PrintErr("ERROR: Cooldown is not valid!");
            }
            if (!_teleportSoundPlayer.IsValid())
            {
                GD.PrintErr("ERROR Teleport sound player is not valid!");
            }
        }

        public override void TeleportNode()
        {
            if (_canTeleport)
            {
                _canTeleport = false;
                GD.Randomize();
                float randomXPosition = (float)GD.RandRange(MINIMUM_POSITION_X, MAXIMUM_POSITION_X);
                float randomYPosition = (float)GD.RandRange(MINIMUM_POSITION_Y, MAXIMUM_POSITION_Y);
                NodeToTeleport.GlobalPosition = new Vector2(randomXPosition, randomYPosition);

                _teleportSoundPlayer.PitchScale = (float)GD.RandRange(0.95, 1.05);
                _teleportSoundPlayer.Stream = _teleportUseSound;
                _teleportSoundPlayer.Play();

                _cooldown.Start(_cooldownTime);
                TeleportEventBus.Instance.EmitSignal("TeleportOff");
            }
        }

        public void OnCooldownTimerTimeout()
        {
            _canTeleport = true;

            _teleportSoundPlayer.PitchScale = (float)GD.RandRange(0.95, 1.05);
            _teleportSoundPlayer.Stream = _teleportCooldownEndSound;
            _teleportSoundPlayer.Play();

            TeleportEventBus.Instance.EmitSignal("TeleportOn");
        }
    }
}