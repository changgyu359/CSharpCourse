using System;

namespace RaidenConsoleGame;

public class Boss : GameObject
{
    public int MaxHp { get; } = 350;
    public int Hp { get; private set; }

    private float _direction = 1f;
    private float _fireTimer = 1.0f;

    private const float TargetY = 5f;
    private const float MoveSpeed = 14f;
    private const float FireInterval = 1.25f;

    public Boss(float x, float y) : base(x, y, 11f, 3f)
    {
        Hp = MaxHp;
    }

    public void TakeDamage(int damage)
    {
        Hp = Math.Max(0, Hp - damage);
    }

    public void Update(float deltaTime, int screenWidth, Action<Bullet> spawnBullet)
    {
        if (Y < TargetY)
        {
            Y += 4f * deltaTime;
            return;
        }

        X += _direction * MoveSpeed * deltaTime;
        float halfW = Width * 0.5f;

        if (X - halfW <= 2f)
        {
            X = 2f + halfW;
            _direction = 1f;
        }
        else if (X + halfW >= screenWidth - 2f)
        {
            X = screenWidth - 2f - halfW;
            _direction = -1f;
        }

        _fireTimer -= deltaTime;
        if (_fireTimer <= 0f)
        {
            _fireTimer = FireInterval;
            spawnBullet(new Bullet(X, Y + 2f, 0f, 17f, false));
            spawnBullet(new Bullet(X - 3f, Y + 2f, -6f, 16f, false));
            spawnBullet(new Bullet(X + 3f, Y + 2f, 6f, 16f, false));
        }
    }

    public override void Render(ConsoleRenderer renderer)
    {
        int left = (int)Math.Round(X - Width * 0.5f);
        int top = (int)Math.Round(Y - 1f);

        renderer.DrawText(left, top,     " /======= ");
        renderer.DrawText(left, top + 1, "<=[ B O S S ]=>");
        renderer.DrawText(left, top + 2, " ==[V]==/ ");
    }
}
