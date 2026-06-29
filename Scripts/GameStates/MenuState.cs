using Godot;
using System;

public class MenuState : GameState
{
    public MenuState(GameManager gameManager) : base(gameManager) { }

    public override void Enter()
    {
        GD.Print("Мы вошли в состояние меню");
    }
    public override void Update()
    {

    }
    public override void PhysicsUpdate(double delta)
    {

    }
    public override void Exit()
    {

    }
}
