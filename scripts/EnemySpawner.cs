using Godot;
using System;
using System.Collections;

public partial class EnemySpawner : Node2D
{

    Random r = new Random();
    private Node2D left, right;
    private PackedScene SharkScene;
    private PackedScene PersonScene;

    const int TOTAL_SPAWNS = 4;

    public override void _Ready()
    {
        left = GetNode<Node2D>("Left");
        right = GetNode<Node2D>("Right");
        SharkScene = GD.Load<PackedScene>("res://scenes/Shark.tscn");
        PersonScene = GD.Load<PackedScene>("res://scenes/Person.tscn");
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
        var selectedSpawnPosition = selectedSideNode.GetNode<Marker2D>(positionPoint.ToString());
        
        var sharkInstance = SharkScene.Instantiate<Shark>();
        sharkInstance.GlobalPosition = selectedSpawnPosition.GlobalPosition;
        
        GetTree().CurrentScene.AddChild(sharkInstance);

        if (selectedSideNode == right)
        {
            sharkInstance.ChangeDirection();
        }
    }

    private void _on_SpawnPersonTimer_timeout() {

        var positionPoint = r.Next(1, 5);

        var randomNumber = r.Next(0, 2);
        var selectedSideNode = randomNumber == 0 ? left : right;
        var selectedSpawnPosition = selectedSideNode.GetNode<Marker2D>(positionPoint.ToString());

        var personInstance = PersonScene.Instantiate<Person>();
        personInstance.GlobalPosition = selectedSpawnPosition.GlobalPosition;

        GetTree().CurrentScene.AddChild(personInstance);

        if (selectedSideNode == right)
        {
            personInstance.ChangeDirection();
        }

    }
  
}
