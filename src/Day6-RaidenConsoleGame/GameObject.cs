namespace RaidenConsoleGame;

public abstract class GameObject
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; protected set; }
    public float Height { get; protected set; }

    protected GameObject(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public bool Intersects(GameObject other)
    {
        float leftA = X - Width * 0.5f;
        float rightA = X + Width * 0.5f;
        float topA = Y - Height * 0.5f;
        float bottomA = Y + Height * 0.5f;

        float leftB = other.X - other.Width * 0.5f;
        float rightB = other.X + other.Width * 0.5f;
        float topB = other.Y - other.Height * 0.5f;
        float bottomB = other.Y + other.Height * 0.5f;

        return leftA < rightB && rightA > leftB && topA < bottomB && bottomA > topB;
    }

    public abstract void Render(ConsoleRenderer renderer);
}
