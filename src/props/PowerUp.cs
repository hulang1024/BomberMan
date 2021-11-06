namespace GameProps
{
    public class PowerUp : GameProp
    {
        public override void Use(Character character)
        {
            if (character.Props.BombPower < Bomb.MaxPower)
            {
                character.Props.BombPower++;
            }
        }
    }
}