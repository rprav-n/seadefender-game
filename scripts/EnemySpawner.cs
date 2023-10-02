using Godot;
using System;
using System.Collections;

public class EnemySpawner : Node2D
{

    Random r = new Random();
    private Node2D left, right;
    private PackedScene SharkScene;

    const int TOTAL_SPAWNS = 4;

    public override void _Ready()
    {
        left = GetNode<Node2D>("Left");
        right = GetNode<Node2D>("Right");
        SharkScene = GD.Load<PackedScene>("res://scenes/Shark.tscn");
    }

    private void _on_SpawnEnemyTimer_timeout()
    {
        ArrayList availableSpawnPoints  = new ArrayList() { 1, 2, 3, 4 };
        ArrayList usedSpawnPoints = new ArrayList();

        for (var i = 0; i < TOTAL_SPAWNS; i++)
        {
            var randomPositionIndexPoint = r.Next(0, availableSpawnPoints.Count);

            usedSpawnPoints.Add(availableSpawnPoints[randomPositionIndexPoint]);

            availableSpawnPoints.RemoveAt(randomPositionIndexPoint);
        }

        foreach (int spawnPoint in usedSpawnPoints)
        {
            spawnEnemy(spawnPoint);
        }
    }

    private void spawnEnemy(int positionPoint)
    {
        var randomNumber = r.Next(0, 2);

        var selectedSideNode = randomNumber == 0 ? left : right;

        var selectedSpawnPosition = selectedSideNode.GetNode<Position2D>(positionPoint.ToString());

        var sharkInstance = SharkScene.Instance<Shark>();

        sharkInstance.GlobalPosition = selectedSpawnPosition.GlobalPosition;

        GetTree().CurrentScene.AddChild(sharkInstance);

        if (selectedSideNode == right)
        {
            sharkInstance.ChangeDirection();
        }
    }
  
}
