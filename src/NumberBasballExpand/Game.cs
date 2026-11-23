using System;
using System.Collections.Generic;
using System.Linq;



public static class Game
{
    private const int SLOT_COUNT = 4; // 칸 개수 4칸으로 확장

    private static List<int> targetNumbers = new List<int>();
    private static List<int> currentGuess = new List<int>();
    private static List<string> history = new List<string>();

    private static int attempts = 0;
    private static bool isRunning = true;
    private static bool isGameOver = false;

    private static string statusMessage = "1~5로만 이루어진 서로 다른 2자리 숫자 4개를 띄어쓰기로 구분해 입력하세요. (예: 12 34 51 24 / 종료: exit)";
    private static ConsoleColor messageColor = ConsoleColor.White;

    public static void Run()
    {
        Init();

        while (isRunning)
        {
            Rendering();

            if (isGameOver)
            {
                break;
            }

            Input();
            Update();
        }

        Console.WriteLine("\n아무 키나 누르면 창이 닫힙니다...");
        Console.ReadKey();
    }

    // 1. 초기화: 각 자리가 1~5로만 이루어진 중복 없는(뒤집은 수 포함) 2자리 숫자 4개 생성
    private static void Init()
    {
        Random random = new Random();
        targetNumbers.Clear();
        history.Clear();

        while (targetNumbers.Count < SLOT_COUNT)
        {
            int tens = random.Next(1, 6); // 1~5
            int ones = random.Next(1, 6); // 1~5
            int num = tens * 10 + ones;

            bool isDuplicate = targetNumbers.Any(t => IsSameNumber(t, num));
            if (!isDuplicate)
            {
                targetNumbers.Add(num);
            }
        }
    }

    // 2. 입력 처리: 띄어쓰기로 구분된 4개의 숫자 검증 (각 자리는 1~5만 허용)
    private static string rawInput = string.Empty;
    private static bool isValidInput = false;

    private static void Input()
    {
        Console.Write($"\n[{attempts + 1}번째 시도] 입력: ");
        rawInput = Console.ReadLine()?.Trim() ?? string.Empty;

        if (rawInput.ToLower() == "exit")
        {
            isRunning = false;
            return;
        }

        List<string> tokens = rawInput.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
        if (tokens.Count != SLOT_COUNT)
        {
            isValidInput = false;
            statusMessage = "[오류] 2자리 숫자 4개를 띄어쓰기 3개로 구분해 입력해야 합니다. (예: 12 34 51 24)";
            messageColor = ConsoleColor.Red;
            return;
        }

        currentGuess.Clear();
        foreach (string token in tokens)
        {
            if (token.Length != 2 || !int.TryParse(token, out int num) || !IsValidDigits(num))
            {
                isValidInput = false;
                statusMessage = "[오류] 모든 숫자는 1~5 사이의 숫자로만 이루어진 2자리여야 합니다. (예: 11, 25, 43)";
                messageColor = ConsoleColor.Red;
                return;
            }
            currentGuess.Add(num);
        }

        // 입력한 4개의 숫자끼리 중복(뒤집었을 때 같은 숫자 포함) 확인
        for (int i = 0; i < SLOT_COUNT; i++)
        {
            for (int j = i + 1; j < SLOT_COUNT; j++)
            {
                if (IsSameNumber(currentGuess.ElementAt(i), currentGuess.ElementAt(j)))
                {
                    isValidInput = false;
                    statusMessage = "[오류] 서로 다른 숫자 4개를 입력해야 합니다. (뒤집었을 때 같은 숫자도 중복 처리)";
                    messageColor = ConsoleColor.Red;
                    return;
                }
            }
        }

        isValidInput = true;
    }

    // 3. 로직 업데이트: 4칸 각각의 판정 수행
    private static void Update()
    {
        if (!isRunning || !isValidInput)
        {
            return;
        }

        attempts++;
        List<string> slotResults = new List<string>();
        int exactMatchCount = 0;

        for (int i = 0; i < SLOT_COUNT; i++)
        {
            int guessNum = currentGuess.ElementAt(i);
            int targetNum = targetNumbers.ElementAt(i);

            // 1) 모두일치
            if (IsSameNumber(guessNum, targetNum))
            {
                slotResults.Add($"{guessNum}(모두일치)");
                exactMatchCount++;
            }
            // 2) 칸불일치
            else if (IsOtherSlotMatch(guessNum, i))
            {
                slotResults.Add($"{guessNum}(칸불일치)");
            }
            // 3) 부분일치
            else if (HasPartialMatch(guessNum, targetNum))
            {
                slotResults.Add($"{guessNum}(부분일치)");
            }
            // 4) 모두틀림
            else
            {
                slotResults.Add($"{guessNum}(모두틀림)");
            }
        }

        string formattedInput = string.Join(" ", currentGuess);
        string resultSummary = string.Join(" / ", slotResults);

        if (exactMatchCount == SLOT_COUNT)
        {
            history.Add($"{attempts,2}회차 | {formattedInput} -> [성공] {resultSummary}");
            statusMessage = $"축하합니다! 총 {attempts}번 만에 정답({string.Join(" ", targetNumbers)})을 모두 맞추셨습니다!";
            messageColor = ConsoleColor.Green;
            isGameOver = true;
        }
        else
        {
            history.Add($"{attempts,2}회차 | {formattedInput} -> {resultSummary}");
            statusMessage = "각 칸의 판정 결과를 참고하여 다음 숫자를 추리해 보세요!";
            messageColor = ConsoleColor.Yellow;
        }
    }

    // 4. 화면 렌더링
    private static void Rendering()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("======================================================================");
        Console.WriteLine("             C# 콘솔 확장 숫자 야구 (1~5 숫자 / 2자리 4칸)            ");
        Console.WriteLine("======================================================================");
        Console.ResetColor();
        Console.WriteLine(" * 숫자 제한: 각 자릿수는 1~5만 사용 가능 (예: 11, 24, 35, 52)");
        Console.WriteLine(" * 동일 규칙: 뒤집은 숫자는 동일 취급 (예: 12 == 21)");
        Console.WriteLine(" * 판정 순서: 모두일치 > 칸불일치 > 부분일치 > 모두틀림");
        Console.WriteLine("======================================================================");

        if (history.Count > 0)
        {
            Console.WriteLine("\n[ 시도 기록 ]");
            foreach (string record in history)
            {
                Console.WriteLine("  " + record);
            }
            Console.WriteLine("----------------------------------------------------------------------");
        }

        Console.WriteLine();
        Console.ForegroundColor = messageColor;
        Console.WriteLine(statusMessage);
        Console.ResetColor();
    }

    // --- 보조 판정 메서드 ---

    // 십의 자리와 일의 자리가 모두 1~5 범위 내인지 검증
    private static bool IsValidDigits(int num)
    {
        int tens = num / 10;
        int ones = num % 10;
        return (tens >= 1 && tens <= 5) && (ones >= 1 && ones <= 5);
    }

    // 두 숫자가 같거나, 뒤집었을 때 같은지 판별
    private static bool IsSameNumber(int a, int b)
    {
        if (a == b) return true;
        int reversedA = (a % 10) * 10 + (a / 10);
        return reversedA == b;
    }

    // 현재 칸을 제외한 다른 3개 칸의 정답과 일치하는지 판별
    private static bool IsOtherSlotMatch(int guessNum, int currentIndex)
    {
        for (int i = 0; i < SLOT_COUNT; i++)
        {
            if (i == currentIndex) continue;
            if (IsSameNumber(guessNum, targetNumbers.ElementAt(i)))
            {
                return true;
            }
        }
        return false;
    }

    // 두 2자리 숫자 사이에 공통으로 포함된 한 자리 숫자가 있는지 판별
    private static bool HasPartialMatch(int guessNum, int targetNum)
    {
        int g1 = guessNum / 10;
        int g2 = guessNum % 10;
        int t1 = targetNum / 10;
        int t2 = targetNum % 10;

        return g1 == t1 || g1 == t2 || g2 == t1 || g2 == t2;
    }
}
