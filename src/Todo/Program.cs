using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// 주석 comment

/*
 *  2026-11-18
 *  키워드 : namespace, internal, class, static, void ,string 문법
 *  
 *  Todo : 키워드 문법에 따라서 결정된 이름
 *  Program : 클래스. 
 *  Main() / WriteLine()
 * 
 */

/*
 * 게임에서 처음에 게임 제목 타이틀이 나오고 그 아래에 플레이가 가능한 플레이어 이름 만들기를 구현해보세요
 * 
 * 제목:문자열
 * 
 * <입출력>
 * step1.자연어 -> 프로그래밍 언어 빠구고 -> 실행 가능한가?
 * step2.효율적인가? 왜 이런 이유로 선택했는지 근거를 말할 수 있는가? -알고리즘
 * 
 * [링크의 전설 ver] [1.0.0]
 */


namespace Todo
{
    internal class Program
    {

        static void Main(string[] args)
        {
            // 텍스트, 실수, 정수
            // string 문자열 :...." / "" 없으면 에러가 뜬다. -> 컨텍스트가 없음 이라고 뜬다.
            // 변수 -> 

            string title = "링크의 전설 ver";
            float version = 1.01f;
             
            int hp = 10;

            // c# 콘솔에서 출력을 할 것인데 타이틀 이름과 버전을 출력하는 코드
            Console.WriteLine($"{title} {version}"); // 문자열 - string
            Console.WriteLine(hp); // 정수  - int
            Console.WriteLine(1.0); // 1이 나온다. 1.0을 적으면 실수가 안나온다.
            Console.WriteLine(true); //

            Console.ReadLine();
        }
    }
}
