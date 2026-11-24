NotePad.Exe();

// 클래스

// 기초 문법.
// 클래스를 만드는 법.

// 속성과 기능
// 클래스를 사용하는 법.

// 클래스를 왜 써야할까요?

// OOP 객체지향프로그래밍


public class Enemy
{
    public string name;
    public int hp;
    

    // 위에 정의한 속성으로 무엇을 하는가? - 기능, 멤버 함수

    public void Hello()
    {
        Console.WriteLine($"{name}의 체력:{hp}");
    }
}

public class NotePad
{
    public static void Exe()
    {
        Enemy SlimeA = new Enemy();
        Enemy SlimeB = new Enemy();

        SlimeA.name = "Slime_A";
        SlimeB.name = "Slime_B";

        SlimeA.Hello();
        SlimeB.Hello();
    }
}