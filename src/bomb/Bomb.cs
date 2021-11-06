using Godot;
using System;
using System.Collections.Generic;

public class Bomb : StaticBody2D
{
    public static PackedScene Scene = GD.Load<PackedScene>("res://src/bomb/Bomb.tscn");

    public Action OnDestory;

    public const int MaxPower = 14;

    public Map Map;

    /// <summary>
    /// 定时计时
    /// </summary>
    public int DelayMS = 2000;
    /// <summary>
    /// 爆炸破坏威力，地图格子单位
    /// </summary>
    private int power = 1;
    public int Power
    {
        get => power;
        set
        {
            if (1 <= power && power <= MaxPower)
            {
                power = value;
            }
        }
    }

    private bool canExplode = false;

    public AudioStreamPlayer AudioPlayer;
    private CollisionShape2D collisionShape;
    private AnimatedSprite animatedSprite;
    private Timer timer;

    public override void _Ready()
    {
        // Power = (int)GD.RandRange(5, 5);

        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");

        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        animatedSprite.Connect("animation_finished", this, "OnAnimationFinished");
        animatedSprite.Connect("frame_changed", this, "OnFrameChanged");

        AudioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        AudioPlayer.Connect("finished", this, "OnExplodeAudioPlayFinished");

        timer = GetNode<Timer>("Timer");
        timer.Connect("timeout", this, "OnTimeout");

        var area2d = GetNode<Area2D>("Area2D");
        area2d.Connect("body_exited", this, "OnBodyExited");

        Start();
    }

    /// <summary>
    /// 启动爆炸定时器
    /// </summary>
    private void Start()
    {
        animatedSprite.SpeedScale = 0.5f;
        animatedSprite.Play("startup");
        timer.Start(DelayMS / 1000);
    }

    /// <summary>
    /// 引爆
    /// </summary>
    public void Explode()
    {
        if (collisionShape == null) return;
        timer.Stop();
        PerformExplode();
        canExplode = false;
    }

    public void PerformExplode()
    {        
        var centerCellPosition = Map.WorldToMap(GlobalPosition);
        var centerFlame = FlameSegment.Create(this, FlameSegment.Kind.Center);
        Map.ReplaceTile(centerCellPosition, centerFlame);

        collisionShape.QueueFree();
        collisionShape = null;
        animatedSprite.Visible = false;

        var growDirTable = new Vector2[]
        {
            Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right
        };
        // 火焰停止增长方向
        Dictionary<int, bool> stopGrowDirs = new Dictionary<int, bool>();
        // 破坏力停止方向
        Dictionary<int, bool> stopPowerDirs = new Dictionary<int, bool>();
        for (int power = 0; power < Power; power++)
        {
            for (int dir = 0; dir < 4; dir++)
            {
                var position = centerCellPosition + growDirTable[dir] * (power + 1);                
                switch (Map.GetTileId(position))
                {
                    case MapTile.Brick:
                        if (!stopPowerDirs.ContainsKey(dir))
                        {
                            var brokenBrick = BrokenBrick.Scene.Instance<BrokenBrick>();
                            brokenBrick.BrickType = BrickType.Brick_Gray;
                            Map.ReplaceTile(position, brokenBrick);
                        }
                        stopGrowDirs[dir] = true;   
                        break;
                    case MapTile.Wall:
                        stopGrowDirs[dir] = true;
                        stopPowerDirs[dir] = true;
                        break;
                    case MapTile.Empty:
                        if (stopGrowDirs.ContainsKey(dir))
                        {
                            stopPowerDirs[dir] = true;
                        }
                        else
                        {
                            var flame = FlameSegment.Create(this,
                                (FlameSegment.Kind)(power == Power - 1 ? dir + 5 : dir + 1));
                            Map.ReplaceTile(position, flame);
                        }
                        break;
                }
            }
        }

        AudioPlayer.Play();

        OnDestory?.Invoke();
    }

    private void OnTimeout()
    {
        Explode();
    }

    private void OnAnimationFinished()
    {
        if (animatedSprite.Animation == "startup")
        {
            animatedSprite.Play("running");
        }
    }

    private void OnFrameChanged()
    {
        if (canExplode && animatedSprite.Animation == "running")
        {
            canExplode = false;
            PerformExplode();
        }
    }

    private void OnExplodeAudioPlayFinished()
    {
        QueueFree();
    }

    private void OnBodyExited(PhysicsBody2D body)
    {
        collisionShape?.SetDeferred("disabled", false);
    }
}
