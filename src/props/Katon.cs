namespace GameProps
{
    public class Katon : GameProp
    {
        public override void Use(Character character)
        {
            character.Props.HasKaton = true;
        }
    }
}