using Game.Bus;
using Godot;
using System;
using Util.ExtensionMethods;

public class LivesManager : Node
{
    private Label _livesLabelReference;
    private const string LIVES_LABEL_NODE_PATH = "LivesLabel";

    private int _currentLives = 3;

    [Signal]
    public delegate void GameOver();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetNodeReferences();
        CheckNodeReferences();
        SetNodeConnections();
    }

    private void SetNodeReferences()
    {
        _livesLabelReference = GetNode<Label>(LIVES_LABEL_NODE_PATH);
    }

    private void CheckNodeReferences()
    {
        if (!_livesLabelReference.IsValid())
        {
            GD.PrintErr("ERROR: Lives label is not valid!");
        }
    }

    private void SetNodeConnections()
    {
        LivesEventBus.Instance.Connect("LoseLife", this, nameof(OnLoseLife));
        LivesEventBus.Instance.Connect("GainLife", this, nameof(OnGainLife));
    }

    private void UpdateLives()
    {
        _livesLabelReference.Text = $"LIVES: {_currentLives}";
    }

    public void OnLoseLife()
    {
        _currentLives--;
        UpdateLives();
        if (_currentLives == 0)
        {
            EmitSignal(nameof(GameOver));
        }
        else
        {
            PlayerEventBus.Instance.EmitSignal("PlayerRespawn");
        }
    }

    public void OnGainLife()
    {
        _currentLives++;
        UpdateLives();
    }
}
