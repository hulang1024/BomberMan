using Godot;
using System.Collections.Generic;

public class DirHelper
{
    /// <summary>
    /// 方向表，（和Dir枚举定义的顺序一致，可以用Dir值作为索引）
    /// </summary>
    /// <value></value>
    private static readonly Vector2[] DirVectorTable = new Vector2[]
    {
        Vector2.Down, Vector2.Up, Vector2.Right, Vector2.Left
    };

    public static Vector2 ToVector(Dir dir)
    {
        return DirVectorTable[(int)dir];
    }

    public static Dir Random()
    {
        return (Dir)GD.RandRange(0, 4);
    }

    public static Dir Random(List<Dir> dirs)
    {
        return dirs[(int)GD.RandRange(0, dirs.Count)];
    }

    public static Dir Back(Dir dir)
    {
        Dir newDir = dir;
        switch (dir)
        {
            case Dir.Up:
                newDir = Dir.Down;
                break;
            case Dir.Down:
                newDir = Dir.Up;
                break;
            case Dir.Left:
                newDir = Dir.Right;
                break;
            case Dir.Right:
                newDir = Dir.Left;
                break;
        }

        return newDir;
    }
}