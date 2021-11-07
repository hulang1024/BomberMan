using Godot;
using System;

[Tool]
public class TextSprite : HBoxContainer
{
    private readonly Vector2 char_size = new Vector2(7, 7);

    private string text = string.Empty;
    [Export]
    public string Text
    {
        get => text;
        set
        {
            if (value != text)
            {
                text = value;
                UpdateText();
            }
        }
    }

    private TextureRect textureRect;

    public override void _Ready()
    {
        textureRect = GetNode<TextureRect>("TextureRect");
        UpdateText();
    }

    public void UpdateText()
    {
        // TODO: 优化性能

        if (textureRect == null) return;

        foreach (Node node in GetChildren())
        {
            RemoveChild(node);
        }

        for (int i = 0; i < text.Length; i++)
        {
            var ch = text[i];
            var charTextureRect = (TextureRect)textureRect.Duplicate();
            var charTexture = (AtlasTexture)charTextureRect.Texture.Duplicate();
            charTextureRect.Texture = charTexture;
            SetTexture(charTexture, ch);
            AddChild(charTextureRect);
        }
    }

    private void SetTexture(Texture texture, char ch)
    {
        int r = 0, c = 0;
        if (char.IsUpper(ch))
        {
            r = 0;
            c = ch - 'A';
        }
        else if (char.IsDigit(ch))
        {
            r = 1;
            c = ch - '0';
        }
        texture.Set("region", new Rect2(c * char_size.x + c, r * char_size.y + r, char_size));
    }
}
