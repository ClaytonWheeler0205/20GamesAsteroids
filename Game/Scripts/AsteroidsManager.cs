using Godot;
using System;

namespace Game
{

    public abstract class AsteroidsManager : Node
    {
        private Node2D _ship;
        public Node2D Ship
        {
            get { return _ship; }
            set
            {
                if (value != null)
                {
                    _ship = value;
                }
            }
        }

        public abstract void SpawnAsteroids(int level);
    }
}