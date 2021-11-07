using Godot;

public class FlameSegment : Node2D
{
    public enum Kind { Center, Up, Down, Left, Right, UpEnd, DownEnd, LeftEnd, RightEnd }

    public static PackedScene Scene = GD.Load<PackedScene>("res://src/gameplay/bomb/FlameSegment.tscn");

    public Bomb Bomb { get; private set; }

    private Kind kind;

    private AnimatedSprite animatedSprite;

    public override void _Ready()
    {
        SetAnimatedSprite();
        animatedSprite.Play();
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

    public static FlameSegment Create(Bomb bomb, Kind kind)
    {
        var flame = Scene.Instance<FlameSegment>();
        flame.Bomb = bomb;
        flame.kind = kind;
        return flame;
    }

    private void OnAnimationFinished()
    {
        QueueFree();
    }
}
