using Godot;

public class TestBox : Area2D {
    public Node2D CollisionNode { get; private set; }

    /// <summary>
    /// 设置/获取启用
    /// </summary>
    /// <value></value>
    public bool Enabled
    {
        set { CollisionNode.Set("disabled", !value); }
        get { return !(bool)CollisionNode.Get("disabled"); }
    }

    public override void _Ready()
    {
        foreach (string name in new string[]{ "CollisionShape2D", "CollisionPolygon2D" })
        {
            if (HasNode(name))
            {
                CollisionNode = GetNode<Node2D>(name);
                break;
            }
        }
    }
}