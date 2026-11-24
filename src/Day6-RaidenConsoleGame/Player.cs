using System;

namespace RaidenConsoleGame;

public class Player : GameObject
{
    public int Lives { get; private set; } = 2;
    public bool IsInvincible => _invincibleTimer > 0f;

    private float _invincibleTimer = 0f;
    private float _fireCooldown = 0f;

    private const float SpeedX = 32f;
    private const float SpeedY = 20f;
    private const float FireInterval = 0.16f;

    public Player(float x, float y) : base(x, y, 1.6f, 1.0f)
    {
    }

    public void Update(float deltaTime, InputManager input, int screenWidth, int screenHeight, Action<Bullet> spawnBullet)
    {
        if (_invincibleTimer > 0f)
        {
            _invincibleTimer -= deltaTime;
        }

        if (_fireCooldown > 0f)
        {
            _fireCooldown -= deltaTime;
        }

        float dx = 0f;
        float dy = 0f;

        if (input.IsKeyDown(ConsoleKey.W)) dy -= 1f;
        if (input.IsKeyDown(ConsoleKey.S)) dy += 1f;
        if (input.IsKeyDown(ConsoleKey.A)) dx -= 1f;
        if (input.IsKeyDown(ConsoleKey.D)) dx += 1f;

        if (dx != 0f && dy != 0f)
        {
            float norm = 0.7071f;
            dx *= norm;
            dy *= norm;
        }

        X = Math.Clamp(X + dx * SpeedX * deltaTime, 1f, screenWidth - 3f);
        Y = Math.Clamp(Y + dy * SpeedY * deltaTime, 2f, screenHeight - 2f);

        if (input.IsKeyDown(ConsoleKey.Spacebar) && _fireCooldown <= 0f)
        {
            _fireCooldown = FireInterval;
            spawnBullet(new Bullet(X + 0.5f, Y - 1f, 0f, -34f, true));
        }
    }

    public void OnHit(float respawnX, float respawnY)
    {
        Lives--;
        if (Lives > 0)
        {
            X = respawnX;
            Y = respawnY;
            _invincibleTimer = 2.0f;
        }
    }

    public override void Render(ConsoleRenderer renderer)
    {
        if (IsInvincible && ((int)(_invincibleTimer * 10f) % 2 == 0))
        {
            return;
        }

        renderer.DrawChar((int)Math.Round(X), (int)Math.Round(Y), '▲', isWide: true);
    }
}
