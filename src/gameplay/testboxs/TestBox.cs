using Godot;

public class TestBox : Area2D {
    private Node2D collisionNode;
    public Node2D CollisionNode
    {
        get { return this.collisionNode; }
    }

    public bool Enabled
    {
        set { collisionNode.Set("disabled", !value); }
        get { return !(bool)collisionNode.Get("disabled"); }
    }

    public override void _Ready()
    {
        foreach (string name in new string[]{ "CollisionShape2D" })
        {
            if (HasNode(name))
            {
                collisionNode = GetNode<Node2D>(name);
                break;
            }
        }
    }
}