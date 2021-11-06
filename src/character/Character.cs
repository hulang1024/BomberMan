using Godot;
using System;


public enum Dir { Down, Up, Right, Left }

public class Character : KinematicBody2D
{
    public Dir Dir { get; private set; } = Dir.Down;
    public Props Props;
    public bool IsDead { get; private set; } = false;
    public bool IsMoving { get; private set; } = false;
    public Vector2 Vector = Vector2.Zero;
    private Vector2 velocity = Vector2.Zero;
    private float speed = 16 * 4;
    public float RealSpeed
    {
        get => speed + Props.ExtraSpeed * (16 * 2);
    }

    internal Hurtbox Hurtbox;

    private AnimationPlayer animationPlayer;
    private AudioStreamPlayer audioPlayer;

    private AudioStream spawnAudio;
    private AudioStream dieAudio;

    public Character()
    {
        Props = new Props(this);
    }

    public override void _Ready()
    {
        var sprite = GetNode<Sprite>("Sprite");

        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.Connect("animation_finished", this, "OnAnimationFinished");

        audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");

        Hurtbox = GetNode<Hurtbox>("Hurtbox");
        Hurtbox.Connect("hitbox_entered", this, "OnHitEntered");

        spawnAudio = GD.Load<AudioStream>("res://assets/audio/character/spawn.mp3");
        dieAudio = GD.Load<AudioStream>("res://assets/audio/character/die.mp3");
    }

    public override void _Process(float delta)
    {
        if (IsDead)
        {
            IsMoving = false;
            animationPlayer.Play("dead");
            return;     
        }
        if (Vector.x != 0 || Vector.y != 0)
        {
            IsMoving = true;

            velocity.x = Vector.x * RealSpeed * delta;
            velocity.y = Vector.y * RealSpeed * delta;

            if (velocity.x > 0)
                Dir = Dir.Right;
            else if (velocity.x < 0)
                Dir = Dir.Left;
            else if (velocity.y > 0)
                Dir = Dir.Down;
            else if (velocity.y < 0)
                Dir = Dir.Up;
        }
        else
        {
            IsMoving = false;
        }

        if (IsMoving)
        {
            PlayDirAnimation("move");
        }
        else
        {
            PlayDirAnimation("idle");
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!IsMoving) return;
        MoveAndCollide(velocity);
    }

    public void OnPlaceBomb()
    {
        audioPlayer.Stream = spawnAudio;
        audioPlayer.Play();
    }

    private void PlayDirAnimation(string anim)
    {
        animationPlayer.Play($"{anim}_{Dir.ToString().ToLower()}");
    }

    private void OnAnimationFinished(string anim)
    {
        if (anim == "dead")
        {
            Visible = false;
        }
    }

    private void OnHitEntered(Hitbox hitbox)
    {
        if (IsDead) return;

        if (!Props.IsInvincible)
        {
            IsDead = true;

            audioPlayer.Stream = dieAudio;
            audioPlayer.Play();

            if (hitbox.Owner is FlameSegment)
            {
                var bomb = ((FlameSegment)hitbox.Owner).Bomb;
                bomb.AudioPlayer.VolumeDb = -16;
            }
        }
    }
}