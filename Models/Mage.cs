namespace ConsoleRpgArena.Models;

public class Mage(string name, Random random) : Character(name, maxHealth: 70), IHealer
{
    private readonly int MinDamage = 15;
    private readonly int MaxDamage = 30;
    private readonly double MissChance = 0.25;
    private readonly int MinHeal = 15;
    private readonly int MaxHeal = 25;

    public override void Attack(Character target, Random random)
    {
        if (random.NextDouble() < MissChance)
        {
            Console.WriteLine($"{Name} casts Fireball at {target.Name}... but it fizzles!");
            return;
        }

        int damage = random.Next(MinDamage, MaxDamage + 1);
        Console.WriteLine($"{Name} hurls a Fireball at {target.Name} for {damage} damage!");
        target.TakeDamage(damage);
    }

    public void Heal(Character target)
    {
        if (!target.IsAlive)
        {
            Console.WriteLine($"{Name} tries to heal {target.Name}, but they have already fallen.");
            return;
        }

        int amount = random.Next(MinHeal, MaxHeal + 1);
        target.RestoreHealth(amount);
        Console.WriteLine($"{Name} heals {target.Name} for {amount} HP.");
    }
}
