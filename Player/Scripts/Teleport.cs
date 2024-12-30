using Godot;
using System;

namespace Game.Player
{

    public abstract class Teleport : Node
    {
        private Node2D _nodeToTeleport;
        public Node2D NodeToTeleport
        {
            get { return _nodeToTeleport;}
            set
            {
                if (value != null)
                {
                    _nodeToTeleport = value;
                }
            }
        }

        public abstract void TeleportNode();
    }
}