using Godot;
using System;

public partial class GameManager : Node
{

    public enum GameStates
    {
        Init,
        Main_menu,
        Gameplay,
        Pause
    }
    public GameStates CurrentGameState { get; private set; } = GameStates.Init;

    [Signal]
    public delegate void GameStateChangeEventHandler(GameStates newState);

    public void ChangeGameState(GameStates newState)
    {
        EmitSignal(SignalName.GameStateChange, Variant.From(newState));
        /*
        //out
        switch (_currentGameState)
        {
            case GameState.Init:
                break;
            case GameState.Main_menu:
                //
                break;
            case GameState.Gameplay:
                //
                break;
            case GameState.Pause:
                Engine.TimeScale = 1.0f;
                _currentGameState = state;
                break;
        }
        */
        //in
        switch (newState)
        {
            case GameStates.Init:
                Engine.TimeScale = 0.0f;
                CurrentGameState = newState;
                break;
            case GameStates.Main_menu:
                Engine.TimeScale = 0.0f;
                CurrentGameState = newState;
                break;
            case GameStates.Gameplay:
                Engine.TimeScale = 1.0f;
                CurrentGameState = newState;
                break;
            case GameStates.Pause:
                Engine.TimeScale = 0.0f;
                CurrentGameState = newState;
                break;
        }
    }

    public override void _Ready()
    {
        ChangeGameState(GameStates.Main_menu);
    }



}
