using Game.Bus;
using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.GUI
{

    public class ScoreManager : Node
    {
        private Label _scoreTextReference;
        private const string SCORE_TEXT_NODE_PATH = "ScoreLabel";

        private int _points;

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            SetNodeConnections();
        }

        private void SetNodeReferences()
        {
            _scoreTextReference = GetNode<Label>(SCORE_TEXT_NODE_PATH);
        }

        private void CheckNodeReferences()
        {
            if (!_scoreTextReference.IsValid())
            {
                GD.PrintErr("ERROR: Score label is not valid!");
            }
        }

        private void SetNodeConnections()
        {
            ScoreEventBus.Instance.Connect("AwardPoints", this, nameof(OnAwardPoints));
        }

        public void OnAwardPoints(int pointsToGive)
        {
            _points += pointsToGive;
            UpdateScore();
        }

        private void UpdateScore()
        {
            if (_points < 100)
            {
                _scoreTextReference.Text = $"Score: 00{_points}";
            }
            else if (_points < 1000)
            {
                _scoreTextReference.Text = $"Score: 0{_points}";
            }
            else
            {
                _scoreTextReference.Text = $"Score: {_points}";
            }
        }
    }
}