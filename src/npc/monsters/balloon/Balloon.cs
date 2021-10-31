using Godot;
using System;

public class Balloon : KinematicBody2D
{
    enum State { Move, Dead }

    private Dir dir = Dir.Right;
    private State state = State.Move;

    private AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.Connect("animation_finished", this, "OnAnimationFinished");

        var hurtbox = GetNode<Hurtbox>("Hurtbox");
        hurtbox.Connect("hitbox_entered", this, "OnHitEntered");
    }

    public override void _Process(float delta)
    {
        switch (state)
        {
            case State.Move:
                animationPlayer.Play($"move_{dir.ToString().ToLower()}");
                break;
            case State.Dead:
                animationPlayer.Play("dead");
                break;
        }
    }

    private void OnHitEntered(Area2D area)
    {
        state = State.Dead;
    }

    private void OnAnimationFinished(string anim)
    {
        if (anim == "dead")
        {
            QueueFree();
        }
    }
}
