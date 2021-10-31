using Godot;
using System.Collections.Generic;

public class Map : YSort
{
    private List<TileMap> tileMaps = new List<TileMap>();
    private TileMap otherTileMap;

    public override void _Ready()
    {
        foreach (Node node in GetChildren())
        {
            if (node is TileMap)
            {
                var tileMap = (TileMap)node;
                if (tileMap.Name != "Floor")
                {
                    tileMaps.Add(tileMap);
                }
                if (otherTileMap == null && tileMap.Name == "Other")
                {
                    otherTileMap = tileMap;
                }
            }
        }
    }

    public MapTile GetTileId(Vector2 position)
    {
        TileMap tileMap;
        return GetTileId(position, out tileMap);
    }

    public MapTile GetTileId(Vector2 position, out TileMap tileMap)
    {
        foreach (TileMap map in tileMaps)
        {
            var cell = (MapTile)map.GetCellv(position);
            if (cell != MapTile.Empty)
            {
                tileMap = map;
                return cell;
            }
        }
        tileMap = null;
        return MapTile.Empty;
    }

    public void ReplaceTile(Vector2 position, Node2D obj, bool isWorldPosition = false)
    {
        if (isWorldPosition)
        {
            position = WorldToMap(position);
        }
        TileMap tileMap;
        var tileId = GetTileId(position, out tileMap);
        tileMap?.SetCellv(position, -1);
        obj.GlobalPosition = MapToWorld(position);
        otherTileMap.AddChild(obj);
    }

    public Vector2 WorldToMap(Vector2 position)
    {
       return tileMaps[0].WorldToMap(position);
    }

    public Vector2 MapToWorld(Vector2 position)
    {
       return tileMaps[0].MapToWorld(position);
    }
}
