public class Props
{
    private Character character;

    public Props(Character character)
    {
        this.character = character;
    }

    public int BombPower = 1;
    public bool IsInvincible = false;
    public bool HasController = false;


    private bool isFreeLander = false;
    public bool IsFreeLander
    {
        get => isFreeLander;
        set
        {
            isFreeLander = value;
            character.SetCollisionMaskBit(7, !value);
        }
    }

    private bool hasKaton = false;
    public bool HasKaton
    {
        get => hasKaton;
        set
        {
            hasKaton = value;
            character.Hurtbox.SetCollisionMaskBit(6, !value);
        }
    }

    private bool hasToton = false;
    public bool HasToton
    {
        get => hasKaton;
        set
        {
            hasToton = value;
            character.SetCollisionMaskBit(2, !value);
        }
    }

    public int HoldBombs = 1;
    public int RunningBombs = 0;

    public int ExtraSpeed = 0;

    public bool IsCanPlaceBomb()
    {
        return RunningBombs < HoldBombs;
    }
}