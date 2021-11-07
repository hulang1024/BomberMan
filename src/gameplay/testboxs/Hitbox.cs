using Godot;
using System;

public class Hitbox : TestBox
{
    [Signal]
    delegate void hurtbox_entered(Hurtbox hurtbox);

    [Signal]
    delegate void hurtbox_exited(Hurtbox hurtbox);

    public Hurtbox hitBy;
    public bool HasHit {
        get { return hitBy != null; }
    }

    public override void _Ready()
    {
        base._Ready();
        Connect("area_entered", this, "OnAreaEntered");
        Connect("area_exited", this, "OnAreaExited");
    }

    public void OnAreaEntered(Area2D area)
    {
        if (!(area is Hurtbox))
        {
            return;
        }
        hitBy = area as Hurtbox;
        EmitSignal("hurtbox_entered", area);
        area.EmitSignal("hitbox_entered", this);
    }

    public void OnAreaExited(Area2D area)
    {
        if (!(area is Hurtbox))
        {
            return;
        }
        hitBy = null;
        EmitSignal("hurtbox_exited", area);
        area.EmitSignal("hitbox_exited", this);
    }
}
