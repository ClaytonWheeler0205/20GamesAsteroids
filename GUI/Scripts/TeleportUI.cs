using Game.Bus;
using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.GUI
{

    public class TeleportUI : Control
    {
        private Label _teleportTextReference;
        private const string TELEPORT_TEXT_REFERENCE_NODE_PATH = "TeleportLabel";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            SetNodeConnections();
        }

        private void SetNodeReferences()
        {
            _teleportTextReference = GetNode<Label>(TELEPORT_TEXT_REFERENCE_NODE_PATH);
        }

        private void CheckNodeReferences()
        {
            if (!_teleportTextReference.IsValid())
            {
                GD.PrintErr("ERROR: Teleport text is not valid!");
            }
        }

        private void SetNodeConnections()
        {
            TeleportEventBus.Instance.Connect("TeleportOn", this, nameof(OnTeleportOn));
            TeleportEventBus.Instance.Connect("TeleportOff", this, nameof(OnTeleportOff));
        }

        public void OnTeleportOn()
        {
            _teleportTextReference.Text = "Teleport: On";
        }

        public void OnTeleportOff()
        {
            _teleportTextReference.Text = "Teleport: Off";
        }
    }
}