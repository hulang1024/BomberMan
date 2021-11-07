namespace GameProps
{
    public class Toton : GameProp
    {
        public override void Use(Character character)
        {
            character.Props.HasToton = true;
        }
    }
}