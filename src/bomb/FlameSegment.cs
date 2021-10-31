using Godot;
using System;

public class FlameSegment : Node2D
{
    public enum Kind { Center, Up, Down, Left, Right, UpEnd, DownEnd, LeftEnd, RightEnd }

    public static PackedScene Scene = GD.Load<PackedScene>("res://src/bomb/FlameSegment.tscn");

    public Action<Character> OnTouchCharacter;

    private Kind kind;

    private AnimatedSprite animatedSprite;

    public override void _Ready()
    {
        SetAnimatedSprite();
        animatedSprite.Play();

        var hitbox = GetNode<Hitbox>("Hitbox");
        hitbox.Connect("hurtbox_entered", this, "OnHurtboxEntered");
    }

    private void SetAnimatedSprite()
    {
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        animatedSprite.Connect("animation_finished", this, "OnAnimationFinished");

        string anim = null;
        switch (kind)
        {
            case Kind.Up:
                anim = "v";
                break;
            case Kind.Down:
                anim = "v";
                animatedSprite.FlipV = true;
                break;
            case Kind.Left:
                anim = "h";
                animatedSprite.FlipH = true;
                break;
            case Kind.Right:
                anim = "h";
                break;
            case Kind.UpEnd:
            case Kind.DownEnd:
            case Kind.LeftEnd:
            case Kind.RightEnd:
            case Kind.Center:
                anim = kind.ToString();
                break;
        }
        animatedSprite.Animation = anim;
    }

    public static FlameSegment Create(Kind kind)
    {
        var flame = Scene.Instance<FlameSegment>();
        flame.kind = kind;
        return flame;
    }

    private void OnAnimationFinished()
    {
        QueueFree();
    }

    private void OnHurtboxEntered(Hurtbox hurtbox)
    {
        if (hurtbox.Owner is Character)
        {
            OnTouchCharacter?.Invoke((Character)hurtbox.Owner);
        }
    }
}
