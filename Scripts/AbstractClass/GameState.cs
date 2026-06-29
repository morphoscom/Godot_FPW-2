using Godot;
using System;

public abstract class GameState
{
    protected GameManager _gameManager;

    public GameState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public virtual void Enter()
    {

    }
    //Возможно понадобиться дельта
    public virtual void Update()
    {

    }
    public virtual void PhysicsUpdate(double delta)
    {

    }
    public virtual void Exit()
    {

    }
}
