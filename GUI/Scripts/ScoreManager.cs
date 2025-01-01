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
        private Label _highScoreTextReference;
        private const string HIGH_SCORE_TEXT_NODE_PATH = "HighScoreLabel";

        private ConfigFile _config = new ConfigFile();

        private int _points;
        private int _highScore = 0;

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            SetNodeConnections();
            LoadHighScore();
        }

        private void SetNodeReferences()
        {
            _scoreTextReference = GetNode<Label>(SCORE_TEXT_NODE_PATH);
            _highScoreTextReference = GetNode<Label>(HIGH_SCORE_TEXT_NODE_PATH);
        }

        private void CheckNodeReferences()
        {
            if (!_scoreTextReference.IsValid())
            {
                GD.PrintErr("ERROR: Score label is not valid!");
            }
            if (!_highScoreTextReference.IsValid())
            {
                GD.PrintErr("ERROR: Score manager high score label is not valid!");
            }
        }

        private void SetNodeConnections()
        {
            ScoreEventBus.Instance.Connect("AwardPoints", this, nameof(OnAwardPoints));
        }

        private void LoadHighScore()
        {
            Error err = _config.Load("user://asteroids_highscore.cfg");

            if (err != Error.Ok)
            {
                _highScore = 0;
                return;
            }

            _highScore = (int)_config.GetValue("AsteroidsPlayerScore", "asteroids_high_score");
            UpdateHighScore();
        }

        public void OnAwardPoints(int pointsToGive)
        {
            _points += pointsToGive;
            UpdateScore();

            if (_points > _highScore)
            {
                _highScore = _points;
                UpdateHighScore();
            }
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

        private void UpdateHighScore()
        {
            if (_highScoreTextReference.IsValid())
            {
                if (_highScore < 10)
                {
                    _highScoreTextReference.Text = $"High: 000{_highScore}";
                }
                else if (_highScore < 100)
                {
                    _highScoreTextReference.Text = $"High: 00{_highScore}";
                }
                else if (_highScore < 1000)
                {
                    _highScoreTextReference.Text = $"High: 0{_highScore}";
                }
                else
                {
                    _highScoreTextReference.Text = $"High: {_highScore}";
                }
            }
            SaveHighScore();
        }

        private void SaveHighScore()
        {
            _config.SetValue("AsteroidsPlayerScore", "asteroids_high_score", _highScore);

            _config.Save("user://asteroids_highscore.cfg");
        }
    }
}