Player hero = new Player("용사",100);

Player slime = new Player("슬라임", 30);


hero.TakeDamage(25);
hero.PrintStatus();
slime.PrintStatus();



class Player
{
    public string Name = "";
    public int Hp = 100;

    public Player(string name, int hp)
    {
        Name = name;
        Hp = hp;
    }

    public void TakeDamage(int Hp)
    {
        this.Hp-= Hp;
    }

    public void PrintStatus()
    {
        Console.WriteLine($"{Name}: HP {Hp}");
    }
}