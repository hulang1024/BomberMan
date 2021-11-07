using Godot;
using System;

public class Balloon : KinematicBody2D
{
    enum State { Move, Dead }

    public Action OnDead;

    public Bomb DeadByBomb { get; private set; }

    private Dir dir = Dir.Right;
    private State state = State.Move;

    private AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.Connect("animation_finished", this, "OnAnimationFinished");

        var hurtbox = GetNode<Hurtbox>("Hurtbox");
        hurtbox.Connect("hitbox_entered", this, "OnHitboxEntered");
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

    private void OnHitboxEntered(Hitbox hitbox)
    {
        state = State.Dead;
        if (hitbox.Owner is FlameSegment)
        {
            DeadByBomb = ((FlameSegment)hitbox.Owner).Bomb;
        }
    }

    private void OnAnimationFinished(string anim)
    {
        if (anim == "dead")
        {
            OnDead?.Invoke();
            QueueFree();
        }
    }
}
