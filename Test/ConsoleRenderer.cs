using System;
using System.Text;

namespace RaidenConsoleGame;

public class ConsoleRenderer
{
    public int Width { get; }
    public int Height { get; }

    private readonly char[,] _buffer;
    private readonly bool[,] _widePlaceholder;
    private readonly StringBuilder _frameBuilder;

    public ConsoleRenderer(int width, int height)
    {
        Width = width;
        Height = height;
        _buffer = new char[height, width];
        _widePlaceholder = new bool[height, width];
        _frameBuilder = new StringBuilder((width + 4) * (height + 5));
    }

    public void Clear()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                _buffer[y, x] = ' ';
                _widePlaceholder[y, x] = false;
            }
        }
    }

    public void DrawChar(int x, int y, char ch, bool isWide = false)
    {
        if (y < 0 || y >= Height || x < 0 || x >= Width)
        {
            return;
        }

        if (isWide && x + 1 >= Width)
        {
            return;
        }

        _buffer[y, x] = ch;
        _widePlaceholder[y, x] = false;

        if (isWide && x + 1 < Width)
        {
            _buffer[y, x + 1] = ' ';
            _widePlaceholder[y, x + 1] = true;
        }
    }

    public void DrawText(int x, int y, string text)
    {
        if (y < 0 || y >= Height)
        {
            return;
        }

        int cursorX = x;
        for (int i = 0; i < text.Length; i++)
        {
            if (cursorX >= Width)
            {
                break;
            }

            char ch = text[i];
            bool wide = IsWideChar(ch);
            if (wide && cursorX + 1 >= Width)
            {
                break;
            }

            if (cursorX >= 0)
            {
                _buffer[y, cursorX] = ch;
                _widePlaceholder[y, cursorX] = false;
                if (wide)
                {
                    _buffer[y, cursorX + 1] = ' ';
                    _widePlaceholder[y, cursorX + 1] = true;
                }
            }

            cursorX += wide ? 2 : 1;
        }
    }

    private static bool IsWideChar(char ch)
    {
        return ch == '▲' || ch == '▼' || (ch >= 0xAC00 && ch <= 0xD7A3);
    }

    public void Present()
    {
        _frameBuilder.Clear();

        _frameBuilder.Append('+');
        _frameBuilder.Append('-', Width);
        _frameBuilder.AppendLine("+");

        for (int y = 0; y < Height; y++)
        {
            _frameBuilder.Append('|');
            for (int x = 0; x < Width; x++)
            {
                if (_widePlaceholder[y, x])
                {
                    continue;
                }
                _frameBuilder.Append(_buffer[y, x]);
            }
            _frameBuilder.AppendLine("|");
        }

        _frameBuilder.Append('+');
        _frameBuilder.Append('-', Width);
        _frameBuilder.AppendLine("+");

        Console.SetCursorPosition(0, 0);
        Console.Write(_frameBuilder.ToString());
    }
}
