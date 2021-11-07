namespace GameProps
{
    public class Controller : GameProp
    {
        public override void Use(Character character)
        {
            character.Props.HasController = true;
        }
    }
}