using System;
using System.Collections.Generic;
using System.Threading;

namespace RaidenConsoleGame;

public class InputManager
{
    private readonly Dictionary<ConsoleKey, float> _keyHoldTimers = new();
    private readonly object _lock = new();
    private Thread? _inputThread;
    private volatile bool _running;

    private const float KeyHoldDuration = 0.14f;

    public bool IsExitRequested { get; private set; }

    public event Action<ConsoleKey>? OnKeyPressed;

    public void Start()
    {
        _running = true;
        _inputThread = new Thread(ReadInputLoop)
        {
            IsBackground = true
        };
        _inputThread.Start();
    }

    public void Stop()
    {
        _running = false;
    }

    private void ReadInputLoop()
    {
        while (_running)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                ConsoleKey key = keyInfo.Key;

                if (key == ConsoleKey.Escape)
                {
                    IsExitRequested = true;
                }

                lock (_lock)
                {
                    _keyHoldTimers[key] = KeyHoldDuration;
                }

                OnKeyPressed?.Invoke(key);
            }
            else
            {
                Thread.Sleep(2);
            }
        }
    }

    public void Update(float deltaTime)
    {
        lock (_lock)
        {
            var keys = new List<ConsoleKey>(_keyHoldTimers.Keys);
            foreach (var key in keys)
            {
                _keyHoldTimers[key] -= deltaTime;
                if (_keyHoldTimers[key] <= 0f)
                {
                    _keyHoldTimers.Remove(key);
                }
            }
        }
    }

    public bool IsKeyDown(ConsoleKey key)
    {
        lock (_lock)
        {
            return _keyHoldTimers.ContainsKey(key);
        }
    }
}
