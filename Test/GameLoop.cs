using System;
using System.Diagnostics;
using System.Threading;

namespace RaidenConsoleGame;

public class GameLoop
{
    private readonly GameEngine _engine;
    private readonly ConsoleRenderer _renderer;
    private readonly InputManager _input;
    private readonly Stopwatch _stopwatch;

    public GameLoop(GameEngine engine, ConsoleRenderer renderer, InputManager input)
    {
        _engine = engine;
        _renderer = renderer;
        _input = input;
        _stopwatch = new Stopwatch();
    }

    public void Run()
    {
        _input.Start();
        _stopwatch.Start();
        double lastTime = _stopwatch.Elapsed.TotalSeconds;

        while (!_input.IsExitRequested)
        {
            double currentTime = _stopwatch.Elapsed.TotalSeconds;
            float deltaTime = (float)(currentTime - lastTime);
            lastTime = currentTime;

            if (deltaTime > 0.05f)
            {
                deltaTime = 0.05f;
            }

            _input.Update(deltaTime);
            _engine.Update(deltaTime);

            _renderer.Clear();
            _engine.Render(_renderer);
            _renderer.Present();

            Thread.Sleep(15);
        }

        _input.Stop();
        Console.CursorVisible = true;
        Console.ResetColor();
        Console.Clear();
    }
}
