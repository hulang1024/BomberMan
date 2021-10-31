using Godot;
using System;

public class BrokenBrick : Node2D
{
    public static PackedScene Scene = GD.Load<PackedScene>("res://src/brick/BrokenBrick.tscn");

    private AnimatedSprite animatedSprite;

    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        animatedSprite.Connect("animation_finished", this, "OnAnimationFinished");
        animatedSprite.Play("broken");
    }

    private void OnAnimationFinished()
    {
        QueueFree();
    }
}
