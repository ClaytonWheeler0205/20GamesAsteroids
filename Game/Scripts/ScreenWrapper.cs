using Godot;
using System;

namespace Game
{

    public abstract class ScreenWrapper : VisibilityNotifier2D
    {
        protected Node2D nodeToTrack;
        public Node2D NodeToTrack
        {
            set
            {
                if (value != null)
                {
                    nodeToTrack = value;
                }
            }
        }
    }
}