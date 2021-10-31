using Godot;
using System;

public class MapPlay: Node2D
{
    public Map playMap;

    public override void _Ready()
    {
        playMap = GetNode<Map>("Map");
        var controller = GetNode<PlayerCharacterController>("../PlayerCharacterController");
        controller.MapPlay = this;
    }

    public Bomb PlaceBomb(Vector2 worldPosition)
    {
        var bomb = Bomb.Scene.Instance<Bomb>();
        bomb.Map = playMap;
        playMap.ReplaceTile(worldPosition, bomb, true);
        return bomb;
    }
}
