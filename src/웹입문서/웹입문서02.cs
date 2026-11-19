static void Main(string[] args)
{
    int a = 1;
    int b = 5;
    Console.WriteLine(a);
    Console.WriteLine(b);

    Swap(a, b);

    Add(4, 10);

    int c = Add2(a, b);
    Console.WriteLine(c);
}

static void Add(int a1, int a2)
{
    Console.WriteLine(a1 + a2);
}

static int Add2(int a1,int a2)
{
    return a1 + a2;
}

static void Swap(int a, int b)
{
    int temp=0;
    temp = a;
    a = b;
    b = temp;

    Console.WriteLine(a);
    Console.WriteLine(b);
}