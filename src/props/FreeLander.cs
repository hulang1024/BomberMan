namespace GameProps
{
    public class FreeLander : GameProp
    {
        public override void Use(Character character)
        {
            character.Props.IsFreeLander = true;
        }
    }
}