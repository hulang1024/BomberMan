namespace GameProps
{
    public class BombAmmo : GameProp
    {
        public override void Use(Character character)
        {
            if (character.Props.HoldBombs < 10)
            {
                character.Props.HoldBombs++;
            }
        }
    }
}