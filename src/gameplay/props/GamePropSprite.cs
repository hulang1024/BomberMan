using Godot;
using System;

namespace GameProps
{
    [Tool]
    public class GamePropSprite : Node2D
    {
        public Action<Character> OnUse;
    
        private PropType type;
        [Export]
        public PropType Type
        {
            get => type;
            set
            {
                type = value;
                LoadPropType();
            }
        }

        private Sprite sprite;

        public override void _Ready()
        {
            sprite = GetNode<Sprite>("Sprite");

            var area2d = GetNode<Area2D>("Area2D");
            area2d.Connect("body_entered", this, "OnBodyEntered");

            LoadPropType();
        }

        private void LoadPropType()
        {
            if (sprite == null) return;
            sprite.Texture = GD.Load<Texture>($"res://assets/props/{type.ToString().ToLower()}.png");
        }

        private void OnBodyEntered(Node body)
        {
            if (!(body is Character)) return;

            var character = (Character)body;

            var gameProp = GamePropFactory.Create(type);
            gameProp.Use(character);
            gameProp = null;

            OnUse?.Invoke(character);

            QueueFree();
        }
    }
}
