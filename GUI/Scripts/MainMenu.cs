using Godot;
using System;
using Util.ExtensionMethods;

namespace Game.GUI
{

    public class MainMenu : Node
    {
        private BaseButton _playButton;
        private BaseButton _instructionsButton;
        private BaseButton _menuButton;
        private Control _instructionsMenu;
        private Control _titleMenu;

        public override void _Ready()
        {
            SetNodeReferences();
            CheckNodeReferences();
            _playButton.CallDeferred("grab_focus");
        }

        private void SetNodeReferences()
        {
            _playButton = GetNode<BaseButton>("%PlayButton");
            _instructionsButton = GetNode<BaseButton>("%InstructionsButton");
            _menuButton = GetNode<BaseButton>("%MenuButton");
            _instructionsMenu = GetNode<Control>("CanvasLayer/InstructionsMenu");
            _titleMenu = GetNode<Control>("CanvasLayer/TitleMenu");
        }

        private void CheckNodeReferences()
        {
            if (!_playButton.IsValid())
            {
                GD.PrintErr("ERROR: Play button is not valid!");
            }
            if (!_instructionsButton.IsValid())
            {
                GD.PrintErr("ERROR: Instructions button is not valid!");
            }
            if (!_menuButton.IsValid())
            {
                GD.PrintErr("ERROR: Menu button is not valid!");
            }
            if (!_instructionsMenu.IsValid())
            {
                GD.PrintErr("ERROR: Instructions menu is not valid!");
            }
            if (!_titleMenu.IsValid())
            {
                GD.PrintErr("ERROR: Title menu is not valid!");
            }
        }

        public async void OnPlayButtonPressed()
        {
            await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
            GetTree().ChangeScene("res://Game/Scenes/Main.tscn");
        }

        public void OnInstructionsButtonPressed()
        {
            _instructionsButton.CallDeferred("release_focus");
            _titleMenu.Visible = false;
            _instructionsMenu.Visible = true;
            _menuButton.CallDeferred("grab_focus");
        }

        public void OnMenuButtonPressed()
        {
            _menuButton.CallDeferred("release_focus");
            _instructionsMenu.Visible = false;
            _titleMenu.Visible = true;
            _playButton.CallDeferred("grab_focus");
        }
    }
}