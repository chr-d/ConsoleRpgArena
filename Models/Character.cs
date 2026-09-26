namespace ConsoleRpgArena.Models;

public abstract class Character(string name, int maxHealth)
{
    public string Name { get; } = name;
    public int Health { get; private set; } = maxHealth;
    public int MaxHealth { get; } = maxHealth;
    public bool IsAlive => Health > 0;

    public void TakeDamage(int amount)
    {
        if (amount < 0)
            return;
        Health = Math.Max(0, Health - amount);
    }

    public void RestoreHealth(int amount)
    {
        if (amount < 0)
            return;
        Health = Math.Min(MaxHealth, Health + amount);
    }

    public abstract void Attack(Character target, Random random);
}
