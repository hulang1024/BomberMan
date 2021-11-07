namespace GameProps
{
    public class GamePropFactory
    {
        public static GameProp Create(PropType type)
        {
            switch (type)
            {
                case PropType.Power:
                    return new PowerUp();
                case PropType.Controller:
                    return new Controller();
                case PropType.Invincible:
                    return new Invincible();
                case PropType.Bomb:
                    return new BombAmmo();
                case PropType.Skates:
                    return new Skates();
                case PropType.Katon:
                    return new Katon();
                case PropType.Toton:
                    return new Toton();
                case PropType.Freelander:
                    return new FreeLander();
            }
            return null;
        }
    }
}
