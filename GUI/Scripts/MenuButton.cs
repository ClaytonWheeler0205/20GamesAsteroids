using Godot;
using System;
using Util.ExtensionMethods;

public class MenuButton : TextureButton
{
    private AudioStreamPlayer _menuButtonSFX;
    private AudioStream _menuMoveSound = GD.Load<AudioStream>("res://GUI/Audio/menu_move.wav");
    private AudioStream _menuToggleSound = GD.Load<AudioStream>("res://GUI/Audio/menu_toggle.wav");

    public override void _Ready()
    {
        SetNodeReferences();
        CheckNodeReferences();
        SetNodeConnections();
    }

    private void SetNodeReferences()
    {
        _menuButtonSFX = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
    }

    private void CheckNodeReferences()
    {
        if (!_menuButtonSFX.IsValid())
        {
            GD.PrintErr("ERROR: Audio stream player is not valid!");
        }
    }

    private void SetNodeConnections()
    {
        Connect("pressed", this, nameof(OnButtonPressed));
        Connect("focus_exited", this, nameof(OnFocusExit));
    }

    public void OnFocusExit()
    {
        _menuButtonSFX.Stream = _menuMoveSound;
        _menuButtonSFX.Play();
    }

    public void OnButtonPressed()
    {
        _menuButtonSFX.Stream = _menuToggleSound;
        _menuButtonSFX.Play();
    }
}
