using Godot;
using System;

public partial class GameManager : Node
{
    public GameState CurrentState { get; private set; }
    
    public void ChangeState(GameState newState)
    {
        CurrentState?.Exit();

        CurrentState = newState;

        CurrentState.Enter();
    }

    public override void _Ready()
    {
        ChangeState(new MenuState(this));
    }

    public override void _Process(double delta)
    {
        CurrentState?.Update();
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentState?.PhysicsUpdate(delta);
    }
}
