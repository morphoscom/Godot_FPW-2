using Godot;
using System;
using static GameManager;

public partial class UiManager : Node
{
    private GameManager _gameManager;

    private Control _mainMenuNode;
    private Control _pauseNode;

    [Signal]
    public delegate void UIWindowChangeEventHandler(UIWindows newUIWindow);

    public enum UIWindows
    {
        None,
        MainMenu,
        Pause
        //...
    }

    public UIWindows CurrentUIWindow { get; private set; } = UIWindows.MainMenu;

    public void ChangeUIWindow(UIWindows newUIWindow)
    {
        EmitSignal(SignalName.UIWindowChange, Variant.From(newUIWindow));

        switch (newUIWindow)
        {
            case UIWindows.None:
                _mainMenuNode.Visible = false;
                _pauseNode.Visible = false;
                break;
            case UIWindows.MainMenu:
                _mainMenuNode.Visible = true;
                _pauseNode.Visible = false;
                break;
            case UIWindows.Pause:
                _mainMenuNode.Visible = false;
                _pauseNode.Visible = true;
                break;
        }

    }

    public void OnGameStateChange(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.Init:
                //
                break;
            case GameStates.Main_menu:
                ChangeUIWindow(UIWindows.MainMenu);
                Input.MouseMode = Input.MouseModeEnum.Confined;
                //
                CurrentUIWindow = UIWindows.MainMenu;
                break;
            case GameStates.Gameplay:
                ChangeUIWindow(UIWindows.None);
                Input.MouseMode = Input.MouseModeEnum.Captured;
                //
                CurrentUIWindow = UIWindows.None;
                break;
            case GameStates.Pause:
                ChangeUIWindow(UIWindows.Pause);
                Input.MouseMode = Input.MouseModeEnum.Confined;
                //
                CurrentUIWindow = UIWindows.Pause;
                break;
        }
    }

    //MainMenuNode behavior
    public void _OnPlayButtonPressed()
    {
        _gameManager.ChangeGameState(GameStates.Gameplay);
    }

    public void _OnSettingButtonPressed()
    {
        GD.Print("Setting");
    }

    public void _OnQuitButtonPressed()
    {
        GetTree().Quit();
    }

    //PauseNode behavior
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            if (CurrentUIWindow != UIWindows.MainMenu)
            {
                if (CurrentUIWindow == UIWindows.None)
                {
                    _gameManager.ChangeGameState(GameStates.Pause);
                }
                else if (CurrentUIWindow == UIWindows.Pause)
                {
                    _gameManager.ChangeGameState(GameStates.Gameplay);
                }
            }
        }
    }

    public void _OnQuitButtonPressed2()
    {
        _gameManager.ChangeGameState(GameStates.Main_menu);
    }

    //Launch UIManager
    public override void _Ready()
    {
        _gameManager = GetParent<GameManager>();
        if (_gameManager != null)
        {
            _gameManager.GameStateChange += OnGameStateChange;
        }

        _mainMenuNode = GetNode<Control>("MainMenu");
        _pauseNode = GetNode<Control>("Pause");

        ChangeUIWindow(UIWindows.MainMenu);
    }
}

