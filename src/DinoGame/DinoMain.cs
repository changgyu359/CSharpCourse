using System;
using System.Collections.Generic;
using System.Threading;

namespace ChromeDinoClone
{
    class Program
    {
        static void Main(string[] args)
        {
            DinosourGame game = new DinosourGame();
            game.Run();
        }
    }

    public class DinosourGame
    {
        // 게임 화면 크기 설정
        private const int ScreenWidth = 60;
        private const int ScreenHeight = 15;
        private const int GroundY = 12;

        // 공룡 상태 변수
        private int dinoX = 5;
        private int dinoY = GroundY;
        private int velocityY = 0;
        private bool isJumping = false;

        // 장애물 및 점수 변수
        private List<int> obstacles = new List<int>();
        private int score = 0;
        private int spawnTimer = 0;
        private bool isGameOver = false;
        private Random random = new Random();

        // 화면 버퍼 (깜빡임 방지용)
        private char[,] screenBuffer = new char[ScreenHeight, ScreenWidth];

        public void Run()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(ScreenWidth + 2, ScreenHeight + 4);

            while (!isGameOver)
            {
                Input();
                Update();
                Rendering();

                // 게임 속도 조절 (점수가 오를수록 조금씩 빨라짐)
                int sleepTime = Math.Max(20, 60 - (score / 100));
                Thread.Sleep(sleepTime);
            }

            // 게임 오버 출력
            Console.SetCursorPosition(ScreenWidth / 2 - 5, ScreenHeight / 2);
            Console.Write("GAME OVER!");
            Console.SetCursorPosition(ScreenWidth / 2 - 8, ScreenHeight / 2 + 1);
            Console.Write($"Final Score: {score}");
            Console.ReadKey();
        }

        private void Input()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                // 스페이스바 또는 위쪽 화살표로 점프
                if ((key.Key == ConsoleKey.Spacebar || key.Key == ConsoleKey.UpArrow) && !isJumping)
                {
                    isJumping = true;
                    velocityY = -3; // 점프 초기 속도 (위로 이동하므로 음수)
                }
            }
        }

        private void Update()
        {
            // 1. 공룡 점프 및 중력 물리 처리
            if (isJumping)
            {
                dinoY += velocityY;
                velocityY += 1; // 중력 가속도

                if (dinoY >= GroundY)
                {
                    dinoY = GroundY;
                    isJumping = false;
                    velocityY = 0;
                }
            }

            // 2. 장애물 생성 (랜덤 간격)
            spawnTimer++;
            if (spawnTimer > random.Next(15, 35))
            {
                obstacles.Add(ScreenWidth - 2);
                spawnTimer = 0;
            }

            // 3. 장애물 이동 및 충돌 체크
            for (int i = obstacles.Count - 1; i >= 0; i--)
            {
                obstacles[i]--;

                // 충돌 판정 (공룡 좌표와 장애물 좌표가 겹칠 때)
                if (obstacles[i] == dinoX && dinoY == GroundY)
                {
                    isGameOver = true;
                }

                // 화면 밖으로 나간 장애물 제거
                if (obstacles[i] < 0)
                {
                    obstacles.RemoveAt(i);
                }
            }

            // 4. 점수 증가
            score++;
        }

        private void Rendering()
        {
            // 버퍼 초기화
            for (int y = 0; y < ScreenHeight; y++)
            {
                for (int x = 0; x < ScreenWidth; x++)
                {
                    if (y == GroundY + 1)
                        screenBuffer[y, x] = '='; // 바닥 (기본 ASCII 문자)
                    else
                        screenBuffer[y, x] = ' ';
                }
            }

            // 버퍼에 공룡 그리기 (2칸 높이)
            screenBuffer[dinoY, dinoX] = '#'; // 공룡 몸통
            if (dinoY - 1 >= 0)
                screenBuffer[dinoY - 1, dinoX] = 'O'; // 공룡 머리

            // 버퍼에 장애물(선인장) 그리기
            foreach (int obsX in obstacles)
            {
                if (obsX >= 0 && obsX < ScreenWidth)
                {
                    screenBuffer[GroundY, obsX] = 'Y'; // 선인장 모양 특수문자
                }
            }

            // 커서를 맨 위로 옮겨 한 번에 출력
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"Score: {score:D5}");

            for (int y = 0; y < ScreenHeight; y++)
            {
                char[] row = new char[ScreenWidth];
                for (int x = 0; x < ScreenWidth; x++)
                {
                    row[x] = screenBuffer[y, x];
                }
                Console.WriteLine(new string(row));
            }
        }

    }
}
