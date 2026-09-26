namespace ConsoleRpgArena.Models;

public class Rogue(string name) : Character(name, maxHealth: 80)
{
    private readonly int MinDamage = 5;
    private readonly int MaxDamage = 10;
    private readonly int Strikes = 2;

    public override void Attack(Character target, Random random)
    {
        for (int i = 0; i < Strikes && target.IsAlive; i++)
        {
            int damage = random.Next(MinDamage, MaxDamage + 1);
            Console.WriteLine(
                $"{Name} strikes {target.Name} (hit {i + 1}/{Strikes}) for {damage} damage!"
            );
            target.TakeDamage(damage);
        }
    }
}
