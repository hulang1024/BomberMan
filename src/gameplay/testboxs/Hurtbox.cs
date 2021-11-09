using Godot;

public class Hurtbox : TestBox
{
    [Signal]
    delegate void hitbox_entered(Hitbox hitbox);

    [Signal]
    delegate void hitbox_exited(Hitbox hitbox);

    public Hitbox hitbox;

    /// <summary>
    /// 是否被击中
    /// </summary>
    /// <value></value>
    public bool HasHit {
        get { return hitbox != null; }
    }

    public override void _Ready()
    {
        base._Ready();
        Connect("hitbox_entered", this, "OnHitboxEntered");
        Connect("hitbox_exited", this, "OnHitboxExited");
    }

    private void OnHitboxEntered(Hitbox hitbox)
    {
        this.hitbox = hitbox;
    }

    private void OnHitboxExited(Hitbox hitbox)
    {
        hitbox = null;
    }
}
