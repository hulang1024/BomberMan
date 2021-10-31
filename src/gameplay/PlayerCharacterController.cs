using Godot;
using static Godot.GD;
using System;

public class PlayerCharacterController : Node2D
{
    public MapPlay MapPlay;

    private Character character;

    private Bomb lastPlaceBomb;

    public override void _Ready()
    {
        character = GetNode<Character>("../PlayerCharacter");
    }

    public override void _Process(float delta)
    {
        if (character.IsDead) return;

        character.Vector.x = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
        character.Vector.y = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (character.IsDead) return;

        if (@event.IsActionPressed("place_bomb"))
        {
            character.OnPlaceBomb();
            lastPlaceBomb = MapPlay.PlaceBomb(character.GlobalPosition + Vector2.Down * 5);
        }
        else if (@event.IsActionPressed("explode_bomb"))
        {
            lastPlaceBomb.Explode();
        }
    }
}