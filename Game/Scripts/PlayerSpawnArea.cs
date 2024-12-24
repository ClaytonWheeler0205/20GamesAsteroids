using Godot;
using System;

public class PlayerSpawnArea : Area2D
{
    
    private int _asteroidsInArea = 0;
    private const string ASTEROID_NODE_GROUP = "Asteroid";

    public void OnAreaEntered(Area2D area)
    {
        if (area.IsInGroup(ASTEROID_NODE_GROUP))
        {
            _asteroidsInArea++;
        }
    }

    public void OnAreaExited(Area2D area)
    {
        if (area.IsInGroup(ASTEROID_NODE_GROUP))
        {
            _asteroidsInArea--;
        }
    }

    public bool AreaIsEmpty()
    {
        return _asteroidsInArea == 0;
    }
}
