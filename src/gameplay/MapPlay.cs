using Godot;
using System;

public class MapPlay: Node2D
{
    public Map playMap;

    public override void _Ready()
    {
        playMap = GetNode<Map>("Map");
        var controller = GetNode<PlayerCharacterController>("PlayerCharacterController");
        controller.MapPlay = this;
    }

    public Bomb PlaceBomb(Character owner)
    {
        var bomb = Bomb.Scene.Instance<Bomb>();
        bomb.Power = owner.Props.BombPower;
        bomb.Map = playMap;
        owner.Props.RunningBombs++;
        bomb.OnDestory = () =>
        {
            owner.Props.RunningBombs--;
        };
        var worldPosition = owner.GlobalPosition + Vector2.Down * 5;
        playMap.ReplaceTile(worldPosition, bomb, true);
        return bomb;
    }
}
