

namespace FleetServer
{
    public class Ship
    {
        public int health;
        public int shield;
        private int maxShield;
        private int firepower;

        public static void Battle(Ship a, Ship b)
        {
            DealDamage(a, b);
            DealDamage(b, a);
        }

        public static void DealDamage(Ship a, Ship b)
        {
            if (a.firepower > b.shield)
            {
                b.health -= (a.firepower - b.shield);
                b.shield = 0;
            }
            else
                b.shield -= a.firepower;
        }
    }
}
