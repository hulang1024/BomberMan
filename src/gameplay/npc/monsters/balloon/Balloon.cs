using Godot;
using System;
using System.Collections.Generic;

public class Balloon : KinematicBody2D
{
    enum State { Move, Dead }

    public Action OnDead;

    private World world;
    public World World
    {
        get => world;
        set
        {
            if (world == null)
            {
                world = value;
                dir = GetCanGoRandomDir();
                ForwardRandomSteps();
            }
        }
    }

    public Bomb DeadByBomb { get; private set; }

    private Dir dir = Dir.Down;
    private float speed = 8;
    private Vector2 velocity = Vector2.Zero;
    private Vector2 destPosition = Vector2.Zero;
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
        if (state != State.Dead)
        {
            bool canGo = true;
            if ((GlobalPosition - destPosition).Length() <= 8.5)
            {
                GlobalPosition = destPosition;
                dir = GetCanGoRandomDir();
                canGo = CanGo(dir);
                if (canGo)
                    ForwardRandomSteps();
                // GD.Print("j");
            }
            if (canGo)
            {
                var collision = MoveAndCollide(velocity * delta);
                if (collision?.Collider != null)
                {
                    dir = GetCanGoRandomDir();
                    ForwardRandomSteps();
                    // GD.Print("c");
                }
            }
        }

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

    /// <summary>
    /// 获取周围可以走的随机方向。
    /// 如果没有可以走的方向(被四周墙围住的情况)则从四个方向中随机一个。
    /// 此方法还保证了网格对齐
    /// </summary>
    /// <returns></returns>
    private Dir GetCanGoRandomDir()
    {
        List<Dir> dirs = new List<Dir>();
        for (int d = 0; d < 4; d++)
        {
            if (CanGo((Dir)d))
                dirs.Add((Dir)d);
        }
        
        return dirs.Count > 0 ? DirHelper.Random(dirs) : DirHelper.Random();
    }

    /// <summary>
    /// 前进随机个步
    /// </summary>
    private void ForwardRandomSteps()
    {
        Forward((int)GD.RandRange(1, 5));
    }

    /// <summary>
    /// 前进
    /// 前置条件，前方应该可以走
    /// </summary>
    /// <param name="steps"></param>
    private void Forward(int steps = 1)
    {
        destPosition = World.Map.ToMapWorld(GlobalPosition)
            + DirHelper.ToVector(dir) * steps * Map.TileSize;
        velocity = DirHelper.ToVector(dir) * speed;
    }

    private MapTile GetFrontMapTile(Dir dir)
    {
        var frontPos = World.Map.WorldToMap(GlobalPosition) + DirHelper.ToVector(dir);
        return World.Map.GetTileId(frontPos);
    }

    private bool CanGo(Dir dir)
    {
        return GetFrontMapTile(dir) == MapTile.Empty;
    }

    private void OnHitboxEntered(Hitbox hitbox)
    {
        velocity = Vector2.Zero;
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
