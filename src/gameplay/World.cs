using Godot;
using System;
using GameProps;

public class World : Control
{
    public Map Map { get; private set; }

    private ScoreProcessor scoreProcessor;

    private Character playerCharacter;

    public override void _Ready()
    {
        Map = GetNode<Map>("Map");

        var hud = HUD.Scene.Instance<HUD>();
        AddChild(hud);

        scoreProcessor = new ScoreProcessor();
        scoreProcessor.ScoreTextSprite = hud.GetNode<TextSprite>("ScoreTextSprite");

        playerCharacter = GetNode<Character>("PlayerCharacter");

        var controller = GetNode<PlayerCharacterController>("PlayerCharacterController");
        controller.World = this;

        foreach (Node node in Map.GetNode("Npc").GetChildren())
        {
            Balloon balloon = (Balloon)node;
            balloon.World = this;
            balloon.OnDead += () => OnMonsterDead(balloon);
        }
        foreach (Node node in Map.GetNode("Props").GetChildren())
        {
            GamePropSprite gamePropSprite = (GamePropSprite)node;
            gamePropSprite.OnUse += (character) =>
            {
                if (character != playerCharacter) return;
                scoreProcessor.Score += 1000;
            };
        }
    }

    public void OnMonsterDead(Balloon monster)
    {
        if (monster.DeadByBomb.PlaceBy != playerCharacter) return;
        scoreProcessor.Score += 100;
    }

    public Bomb PlaceBomb(Character owner)
    {
        var worldPosition = owner.GlobalPosition + Vector2.Down * 5;

        if (Map.GetTileId(Map.WorldToMap(worldPosition)) != MapTile.Empty)
        {
            return null;
        }

        var bomb = Bomb.Scene.Instance<Bomb>();
        bomb.PlaceBy = owner;
        bomb.Power = owner.Props.BombPower;
        bomb.World = this;
        owner.Props.RunningBombs++;
        bomb.OnDestory = () =>
        {
            owner.Props.RunningBombs--;
        };
        Map.ReplaceTile(worldPosition, bomb, true);
        return bomb;
    }
}
