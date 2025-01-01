using Godot;
using System;

public class PlayerSpawnArea : Area2D
{
    
    private int _objectsInArea = 0;
    private const string ASTEROID_NODE_GROUP = "Asteroid";
    private const string UFO_NODE_GROUP = "UFO";
    private const string UFO_BULLET_NODE_GROUP = "UFOBullet";

    public void OnAreaEntered(Area2D area)
    {
        if (area.IsInGroup(ASTEROID_NODE_GROUP) || area.IsInGroup(UFO_NODE_GROUP) || area.IsInGroup(UFO_BULLET_NODE_GROUP))
        {
            _objectsInArea++;
        }
    }

    public void OnAreaExited(Area2D area)
    {
        if (area.IsInGroup(ASTEROID_NODE_GROUP) || area.IsInGroup(UFO_NODE_GROUP) || area.IsInGroup(UFO_BULLET_NODE_GROUP))
        {
            _objectsInArea--;
        }
    }

    public bool AreaIsEmpty()
    {
        return _objectsInArea == 0;
    }
}
