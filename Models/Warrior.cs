namespace ConsoleRpgArena.Models;

public class Warrior(string name) : Character(name, maxHealth: 100)
{
    private readonly int MinDamage = 10;
    private readonly int MaxDamage = 20;

    public override void Attack(Character target, Random random)
    {
        int damage = random.Next(MinDamage, MaxDamage + 1);
        Console.WriteLine($"{Name} swings a greatsword at {target.Name} for {damage} damage!");
        target.TakeDamage(damage);
    }
}
