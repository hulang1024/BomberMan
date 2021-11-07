using Godot;

[Tool]
public class GridWall : TileMap
{
    [Export]
    private Rect2 rect = new Rect2(Vector2.Zero, new Vector2(30, 12));
    public Rect2 Rect
    {
        get => rect;
        set
        {
            Generate();
        }
    }

    public override void _Ready()
    {
        Generate();
    }

    private void Generate()
    {
        GD.Print("Generate GridWall");
        foreach (Vector2 cell in GetUsedCells())
        {
            SetCellv(cell, -1);
        }
        GenerateRectBorder();
        GenerateGrid();
    }

    private void GenerateRectBorder()
    {
        var tileMap = this;

        void DrawLine(Vector2 start, Vector2 end)
        {
            for (Vector2 point = start; point != end; point += point.DirectionTo(end))
            {
                tileMap.SetCellv(point, (int)MapTile.Wall);
            }
        }

        var left = Rect.Position;
        var size = Rect.Size;
        DrawLine(left, new Vector2(size.x, left.y));
        DrawLine(new Vector2(size.x, left.y), size);
        DrawLine(size, new Vector2(left.x, size.y));
        DrawLine(new Vector2(left.x, size.y), left);
    }

    private void GenerateGrid()
    {
        var tileMap = this;

        for (int r = (int)rect.Position.y + 2; r < rect.Size.y; r += 2)
        {
            for (int c = (int)rect.Position.x + 2; c < rect.Size.x; c += 2)
            {
                tileMap.SetCell(c, r, (int)MapTile.Wall);
            }
        }
    }
}
