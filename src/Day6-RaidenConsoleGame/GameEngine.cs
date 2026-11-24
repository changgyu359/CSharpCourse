using System;
using System.Collections.Generic;

namespace RaidenConsoleGame;

public enum GameState
{
    Playing,
    GameOver,
    Victory
}

public class GameEngine
{
    private readonly int _width;
    private readonly int _height;
    private readonly InputManager _input;
    private readonly Random _random = new();

    private Player _player;
    private readonly List<Enemy> _enemies = new();
    private readonly List<Bullet> _bullets = new();
    private Boss? _boss;

    private GameState _state = GameState.Playing;
    private float _elapsedTime = 0f;
    private float _squadronSpawnTimer = 1.0f;
    private int _patternIndex = 0;

    private const float BossSpawnTime = 40.0f;
    private const float SquadronSpawnInterval = 3.4f;

    public GameEngine(int width, int height, InputManager input)
    {
        _width = width;
        _height = height;
        _input = input;
        _player = new Player(_width / 2f, _height - 4f);
        ResetGame();
    }

    private void ResetGame()
    {
        _player = new Player(_width / 2f, _height - 4f);
        _enemies.Clear();
        _bullets.Clear();
        _boss = null;
        _state = GameState.Playing;
        _elapsedTime = 0f;
        _squadronSpawnTimer = 1.0f;
        _patternIndex = 0;
    }

    public void Update(float deltaTime)
    {
        if (_state != GameState.Playing)
        {
            if (_input.IsKeyDown(ConsoleKey.R))
            {
                ResetGame();
            }
            return;
        }

        _elapsedTime += deltaTime;

        _player.Update(deltaTime, _input, _width, _height, bullet => _bullets.Add(bullet));

        if (_boss == null)
        {
            if (_elapsedTime >= BossSpawnTime)
            {
                _boss = new Boss(_width / 2f, -2f);
            }
            else
            {
                _squadronSpawnTimer -= deltaTime;
                if (_squadronSpawnTimer <= 0f)
                {
                    _squadronSpawnTimer = SquadronSpawnInterval;
                    SpawnEnemySquadron();
                }
            }
        }

        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            _enemies[i].Update(deltaTime, bullet => _bullets.Add(bullet));
            if (_enemies[i].Y >= _height)
            {
                _enemies.RemoveAt(i);
            }
        }

        if (_boss != null)
        {
            _boss.Update(deltaTime, _width, bullet => _bullets.Add(bullet));
        }

        for (int i = _bullets.Count - 1; i >= 0; i--)
        {
            _bullets[i].Update(deltaTime);
            if (_bullets[i].IsOutOfBounds(_width, _height))
            {
                _bullets.RemoveAt(i);
            }
        }

        CheckCollisions();
    }

    private void SpawnEnemySquadron()
    {
        int pattern = _patternIndex % 4;
        _patternIndex++;

        switch (pattern)
        {
            case 0:
            {
                // 패턴 1: V자 대형 (5기)
                float cx = _random.Next(14, _width - 16);
                _enemies.Add(new Enemy(cx, 1f, 0.7f));
                _enemies.Add(new Enemy(cx - 5f, -1f, 1.1f));
                _enemies.Add(new Enemy(cx + 5f, -1f, 1.1f));
                _enemies.Add(new Enemy(cx - 10f, -3f, 1.5f));
                _enemies.Add(new Enemy(cx + 10f, -3f, 1.5f));
                break;
            }
            case 1:
            {
                // 패턴 2: 일자 횡대 진형 (4기)
                float startX = _random.Next(6, _width - 26);
                for (int i = 0; i < 4; i++)
                {
                    _enemies.Add(new Enemy(startX + i * 6f, 0f, 0.6f + i * 0.35f));
                }
                break;
            }
            case 2:
            {
                // 패턴 3: 사선 대각선 편대 (4기)
                bool leftToRight = _random.Next(2) == 0;
                float startX = leftToRight ? 8f : _width - 10f;
                float stepX = leftToRight ? 6f : -6f;

                for (int i = 0; i < 4; i++)
                {
                    _enemies.Add(new Enemy(startX + i * stepX, -i * 2f, 0.8f + i * 0.3f));
                }
                break;
            }
            case 3:
            {
                // 패턴 4: 양측 협공 편대 (좌 2기, 우 2기)
                _enemies.Add(new Enemy(8f, 0f, 0.7f));
                _enemies.Add(new Enemy(14f, -2f, 1.2f));
                _enemies.Add(new Enemy(_width - 10f, 0f, 0.7f));
                _enemies.Add(new Enemy(_width - 16f, -2f, 1.2f));
                break;
            }
        }
    }

    private void CheckCollisions()
    {
        for (int i = _bullets.Count - 1; i >= 0; i--)
        {
            Bullet b = _bullets[i];

            if (b.IsPlayerBullet)
            {
                bool hit = false;
                for (int j = _enemies.Count - 1; j >= 0; j--)
                {
                    if (b.Intersects(_enemies[j]))
                    {
                        _enemies.RemoveAt(j);
                        hit = true;
                        break;
                    }
                }

                if (!hit && _boss != null && b.Intersects(_boss))
                {
                    _boss.TakeDamage(10);
                    hit = true;
                    if (_boss.Hp <= 0)
                    {
                        _boss = null;
                        _state = GameState.Victory;
                    }
                }

                if (hit)
                {
                    _bullets.RemoveAt(i);
                }
            }
            else
            {
                if (!_player.IsInvincible && b.Intersects(_player))
                {
                    _bullets.RemoveAt(i);
                    HandlePlayerHit();
                    return;
                }
            }
        }

        if (!_player.IsInvincible)
        {
            foreach (var enemy in _enemies)
            {
                if (_player.Intersects(enemy))
                {
                    HandlePlayerHit();
                    return;
                }
            }

            if (_boss != null && _player.Intersects(_boss))
            {
                HandlePlayerHit();
            }
        }
    }

    private void HandlePlayerHit()
    {
        _player.OnHit(_width / 2f, _height - 4f);
        if (_player.Lives <= 0)
        {
            _state = GameState.GameOver;
        }
    }

    public void Render(ConsoleRenderer renderer)
    {
        _player.Render(renderer);

        foreach (var enemy in _enemies)
        {
            enemy.Render(renderer);
        }

        _boss?.Render(renderer);

        foreach (var bullet in _bullets)
        {
            bullet.Render(renderer);
        }

        RenderHud(renderer);
    }

    private void RenderHud(ConsoleRenderer renderer)
    {
        renderer.DrawText(2, 0, $"LIVES: {_player.Lives}");

        if (_boss == null && _state == GameState.Playing)
        {
            float remain = Math.Max(0f, BossSpawnTime - _elapsedTime);
            renderer.DrawText(_width - 20, 0, $"BOSS IN: {remain:F1}s");
        }
        else if (_boss != null)
        {
            int barWidth = 24;
            int filled = (int)Math.Round((double)_boss.Hp / _boss.MaxHp * barWidth);
            filled = Math.Clamp(filled, 0, barWidth);
            string bar = new string('#', filled) + new string('-', barWidth - filled);
            renderer.DrawText(16, 0, $"BOSS [{bar}] {_boss.Hp}/{_boss.MaxHp}");
        }

        if (_state == GameState.GameOver)
        {
            renderer.DrawText(_width / 2 - 5, _height / 2 - 1, "GAME OVER");
            renderer.DrawText(_width / 2 - 11, _height / 2 + 1, "Press [R] to Restart");
        }
        else if (_state == GameState.Victory)
        {
            renderer.DrawText(_width / 2 - 6, _height / 2 - 1, "GAME CLEAR!");
            renderer.DrawText(_width / 2 - 11, _height / 2 + 1, "Press [R] to Restart");
        }
    }
}
