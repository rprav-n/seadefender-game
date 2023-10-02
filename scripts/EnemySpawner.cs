using Godot;
using System;

public class EnemySpawner : Node2D
{

    Random r = new Random();
    private Node left, right;
    private PackedScene SharkScene;

    public override void _Ready()
    {
        left = GetNode<Node>("Left");
        right = GetNode<Node>("Right");
        SharkScene = GD.Load<PackedScene>("res://scenes/Shark.tscn");
    }

    private void _on_SpawnEnemyTimer_timeout()
    {
        var randomSpawnPointNumber = r.Next(1, 5);

        var selectedSideNode = left;

        var spawnRight = Convert.ToBoolean(r.Next(0, 2)) ;
        if (spawnRight)
        {
            selectedSideNode = right;
        }

        var selectedSpawnPosition = selectedSideNode.GetNode<Position2D>(randomSpawnPointNumber.ToString());

        var sharkInstance = SharkScene.Instance<Shark>();

        sharkInstance.GlobalPosition = selectedSpawnPosition.GlobalPosition;

        GetTree().CurrentScene.AddChild(sharkInstance);

        if (spawnRight)
        {
            sharkInstance.ChangeDirection();
        }
    }

  
}
