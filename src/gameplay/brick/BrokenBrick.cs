using Godot;
using System;

public enum BrickType
{
    Brick_Gray,
    Brick_Orange
}

public class BrokenBrick : Node2D
{
    public static PackedScene Scene = GD.Load<PackedScene>("res://src/gameplay/brick/BrokenBrick.tscn");

    public BrickType BrickType;

    private AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        var sprite = GetNode<Sprite>("Sprite");
        sprite.Texture = GD.Load<Texture>($"res://assets/brick/{BrickType.ToString().ToLower()}.png");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.Connect("animation_finished", this, "OnAnimationFinished");
        animationPlayer.Play("broken");
    }

    private void OnAnimationFinished(string anim)
    {
        QueueFree();
    }
}
