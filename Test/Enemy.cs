using System;

namespace RaidenConsoleGame;

public class Enemy : GameObject
{
    private float _fireTimer;
    private readonly float _speedY;
    private readonly float _fireInterval;

    public Enemy(float x, float y, float initialFireDelay = 0.6f, float speedY = 6.5f)
        : base(x, y, 1.6f, 1.0f)
    {
        _fireTimer = initialFireDelay;
        _speedY = speedY;
        _fireInterval = 2.1f;
    }

    public void Update(float deltaTime, Action<Bullet> spawnBullet)
    {
        Y += _speedY * deltaTime;

        _fireTimer -= deltaTime;
        if (_fireTimer <= 0f)
        {
            _fireTimer = _fireInterval;
            spawnBullet(new Bullet(X + 0.5f, Y + 1f, 0f, 16f, false));
        }
    }

    public override void Render(ConsoleRenderer renderer)
    {
        renderer.DrawChar((int)Math.Round(X), (int)Math.Round(Y), '▼', isWide: true);
    }
}
