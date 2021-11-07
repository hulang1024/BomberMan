namespace GameProps
{
    public class Invincible : GameProp
    {
        public override void Use(Character character)
        {
            character.Props.IsInvincible = true;
        }
    }
}