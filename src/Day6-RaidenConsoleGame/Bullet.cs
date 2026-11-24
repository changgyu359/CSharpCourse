using System;

namespace RaidenConsoleGame;

public class Bullet : GameObject
{
    public bool IsPlayerBullet { get; }
    private readonly float _vx;
    private readonly float _vy;

    public Bullet(float x, float y, float vx, float vy, bool isPlayerBullet)
        : base(x, y, 0.8f, 0.8f)
    {
        _vx = vx;
        _vy = vy;
        IsPlayerBullet = isPlayerBullet;
    }

    public void Update(float deltaTime)
    {
        X += _vx * deltaTime;
        Y += _vy * deltaTime;
    }

    public bool IsOutOfBounds(int width, int height)
    {
        return X < 0 || X >= width || Y < 0 || Y >= height;
    }

    public override void Render(ConsoleRenderer renderer)
    {
        char symbol = IsPlayerBullet ? '|' : '*';
        renderer.DrawChar((int)Math.Round(X), (int)Math.Round(Y), symbol, isWide: false);
    }
}
