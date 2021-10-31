using Godot;
using System;


public enum Dir { Down, Up, Right, Left }

public class Character : KinematicBody2D
{
    public Dir Dir { get; private set; } = Dir.Down;
    public bool isInvincible = false;
    public bool IsDead { get; private set; } = false;
    public bool IsMoving { get; private set; } = false;
    public Vector2 Vector = Vector2.Zero;
    private Vector2 velocity = Vector2.Zero;
    private float speed = 16 * 4;

    private Hurtbox hurtbox;
    private AnimationPlayer animationPlayer;
    private AudioStreamPlayer audioPlayer;

    private AudioStream spawnAudio;
    private AudioStream dieAudio;

    public override void _Ready()
    {
        var sprite = GetNode<Sprite>("Sprite");

        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.Connect("animation_finished", this, "OnAnimationFinished");

        audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");

        hurtbox = GetNode<Hurtbox>("Hurtbox");
        hurtbox.Connect("hitbox_entered", this, "OnHitEntered");

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
            velocity.x = Vector.x * speed * delta;
            velocity.y = Vector.y * speed * delta;

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

        if (!isInvincible)
        {
            IsDead = true;

            audioPlayer.Stream = dieAudio;
            audioPlayer.Play();
        }
    }
}