using ConsoleRpgArena.Models;

Random random = new();

List<Character> playerParty = [new Warrior("Bob"), new Mage("John", random), new Rogue("Alice")];

List<Character> enemies =
[
    new Warrior("Orc Warrior"),
    new Mage("Dark Cultist", random),
    new Rogue("Goblin Thief"),
];

Console.WriteLine("=== CONSOLE RPG ARENA ===");
ShowStatus(playerParty, enemies);

// Battle loop
bool playerQuit = false;

while (AnyAlive(playerParty) && AnyAlive(enemies) && !playerQuit)
{
    Console.WriteLine();
    Console.WriteLine("――――――――― NEW ROUND ―――――――――");

    // List of every living character
    List<Character> turnOrder = playerParty.Concat(enemies).Where(c => c.IsAlive).ToList();

    foreach (Character active in turnOrder)
    {
        if (!active.IsAlive)
            continue; // died earlier this round
        if (!AnyAlive(playerParty) || !AnyAlive(enemies))
            break; // battle already over

        Console.WriteLine();
        if (playerParty.Contains(active))
        {
            bool keepGoing = PlayerTurn(active, playerParty, enemies, random);
            if (!keepGoing)
            {
                playerQuit = true;
                break;
            }
        }
        else
        {
            EnemyTurn(active, playerParty, random);
        }
    }
}

// Result
Console.WriteLine();
if (playerQuit)
    Console.WriteLine("🚪 You fled the arena. Coward!");
else if (AnyAlive(playerParty))
    Console.WriteLine("🏆 VICTORY! The enemy party has been defeated!");
else
    Console.WriteLine("💀 DEFEAT! Your party has been defeated...");

// Helper functions

static bool AnyAlive(List<Character> party) => party.Any(c => c.IsAlive);

static void ShowStatus(List<Character> playerParty, List<Character> enemies)
{
    Console.WriteLine();
    Console.WriteLine("--- YOUR PARTY ---");
    PrintParty(playerParty);
    Console.WriteLine("--- ENEMIES ---");
    PrintParty(enemies);
}

static void PrintParty(List<Character> party)
{
    foreach (Character c in party)
    {
        string status = c.IsAlive ? $"{c.Health}/{c.MaxHealth} HP" : "DEFEATED";

        Console.WriteLine($"  [{c.GetType().Name}] {c.Name} {status}");
    }
}

static bool PlayerTurn(
    Character active,
    List<Character> allies,
    List<Character> enemies,
    Random random
)
{
    while (true)
    {
        Console.WriteLine($"\n> {active.Name}'s turn ({active.Health}/{active.MaxHealth} HP)");

        IHealer? healer = active as IHealer;

        Console.WriteLine("  1) Attack");
        if (healer is not null)
            Console.WriteLine("  2) Heal an ally");

        Console.WriteLine("  3) Show status");
        Console.WriteLine("  4) Quit game");

        int choice = GetInt("Choose", 1, 4);
        if (choice == 0)
            return false;

        switch (choice)
        {
            case 1:
            {
                Character? target = PickTarget("Attack whom?", enemies);
                if (target is null)
                    continue;
                active.Attack(target, random);
                return true;
            }

            case 2 when healer is not null:
            {
                Character? ally = PickTarget("Heal whom?", allies);
                if (ally is null)
                    continue;
                healer.Heal(ally);
                return true;
            }

            case 3:
                ShowStatus(allies, enemies);
                continue;

            case 4:
                return false;

            default:
                Console.WriteLine("Invalid choice.");
                continue;
        }
    }
}

static Character? PickTarget(string prompt, List<Character> possibleTargets)
{
    List<Character> living = [.. possibleTargets.Where(c => c.IsAlive)];
    if (living.Count == 0)
    {
        Console.WriteLine("No valid targets.");
        return null;
    }

    Console.WriteLine(prompt);
    for (int i = 0; i < living.Count; i++)
        Console.WriteLine(
            $"  {i + 1}) {living[i].Name} ({living[i].Health}/{living[i].MaxHealth} HP)"
        );

    int index = GetInt("Target #", min: 0, max: living.Count, extraHint: "0 to cancel");

    return index == 0 ? null : living[index - 1];
}

static void EnemyTurn(Character active, List<Character> playerParty, Random random)
{
    List<Character> livingPlayers = [.. playerParty.Where(c => c.IsAlive)];
    if (livingPlayers.Count == 0)
        return;

    Character target = livingPlayers[random.Next(livingPlayers.Count)];
    Console.WriteLine($"\n> Enemy turn: {active.Name}");
    active.Attack(target, random);
}

static int GetInt(string prompt, int min, int max, string? extraHint = null)
{
    while (true)
    {
        Console.Write($"{prompt}");

        if (!string.IsNullOrWhiteSpace(extraHint))
            Console.Write($" ({extraHint})");

        Console.Write(": ");

        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Please enter a number.");
            continue;
        }

        if (!int.TryParse(input, out int value))
        {
            Console.WriteLine("That's not a number.");
            continue;
        }

        if (value < min || value > max)
        {
            Console.WriteLine(
                $"Out of range. Pick {min}-{max}{(min == 0 ? " or 0 to cancel" : "")}."
            );
            continue;
        }

        return value;
    }
}
