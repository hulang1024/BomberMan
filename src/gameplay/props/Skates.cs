namespace GameProps
{
    public class Skates : GameProp
    {
        public override void Use(Character character)
        {
            if (character.Props.ExtraSpeed < 2)
            {
                character.Props.ExtraSpeed++;
            }
        }
    }
}