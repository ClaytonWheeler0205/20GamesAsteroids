using Godot;
using System;

namespace Game
{

    public class ScreenWrapperImpl : ScreenWrapper
    {
        public override void _Ready()
        {
            this.Connect("screen_exited", this, nameof(OnScreenExited));
        }

        public void OnScreenExited()
        {
            Vector2 screenSize = GetViewportRect().Size;
            if (nodeToTrack.GlobalPosition.x < 0)
            {
                nodeToTrack.GlobalPosition = new Vector2(screenSize.x, GlobalPosition.y);
            }
            else if (nodeToTrack.GlobalPosition.x > screenSize.x)
            {
                nodeToTrack.GlobalPosition = new Vector2(0, GlobalPosition.y);
            }
            if (nodeToTrack.GlobalPosition.y < 0)
            {
                nodeToTrack.GlobalPosition = new Vector2(GlobalPosition.x, screenSize.y);
            }
            else if (nodeToTrack.GlobalPosition.y > screenSize.y)
            {
                nodeToTrack.GlobalPosition = new Vector2(GlobalPosition.x, 0);
            }
        }
    }
}