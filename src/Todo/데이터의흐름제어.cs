// 분기문
// 코드를 원하는 부분만 실행하겠다.

/*
 *  if( 조건문)
 *  {
 *      본문
 *  }
 */



// 보유 돈, 강화할 때 필요한 돈



//int myMoney = 10000;
//int cost = 500;
//int reinforce = 0;
//while(myMoney>=cost)
//{
//    Console.WriteLine("강화성공:"+ ++reinforce +"강");
//    myMoney -= cost;
//}

//string input = Console.ReadLine(); // 1=주먹, 2=가위, 3=보

//int.TryParse(input, out int choice);

//// 상수. 변하지 않게 강제한다.




//Random rand = new Random();

//int otherChoice = rand.Next(1, 4);
//Console.WriteLine(otherChoice);

//if(choice == (int)Todo.Choice.ROCK)
//{
//    Console.WriteLine("주먹을 냈습니다.");
//}
//else if(choice == (int)Todo.Choice.SCISSORS)
//{
//    Console.WriteLine("가위를 냈습니다.");
//}
//else if(choice == (int)Todo.Choice.PAPER)
//{
//    Console.WriteLine("보를 냈습니다.");
//}
//else
//{
//    Console.WriteLine("잘못된 접근입니다.");
//}

//switch(choice)
//{
//    case (int)Todo.Choice.ROCK: Console.WriteLine("주먹을 냈습니다.");
//        break;
//    case (int)Todo.Choice.SCISSORS:
//        Console.WriteLine("가위를 냈습니다.");
//        break;
//    case (int)Todo.Choice.PAPER:
//        Console.WriteLine("보를 냈습니다.");
//        break;
//    default:
//        Console.WriteLine("잘못된 접근입니다.");
//        break;
//}

//Random rand=new Random();
//int[] already=new int[] {1,5,18,23,27,28};
//int[] first = new int[6];
//for(int i=0; i<10; i++)
//{
//    int check = 0;
//    int[] myTurn = new int[6];
//    if (i==0)
//    {
//        while (check < 6)
//        {
//            int a = rand.Next(1, 46);
//            if (Array.Exists(already, x => x == a)||Array.Exists(myTurn, x => x == a))
//            {
                
//                continue;
//            }
//            else
//            {
//                Console.Write(a+" ");
//                first[check] = a;
//                myTurn[check] = a;
//                check++;
//            }
                
//        }
//        Console.WriteLine(" ");
//    }
//    else
//    {
//        while (check < 6)
//        {
//            int a = rand.Next(1, 46);
//            if (Array.Exists(already, x => x == a)|| Array.Exists(myTurn, x => x == a)||Array.Exists(first, x => x == a))
//            {
                
//                continue;
//            }
//            else
//            {
//                Console.Write(a + " ");
//                first[check] = a;
//                myTurn[check] = a;
//                check++;
//            }

//        }
//        Console.WriteLine(" ");
//    }



//}



