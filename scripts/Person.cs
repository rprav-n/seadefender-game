using Godot;
using System;

public class Person : Area2D
{
    private Vector2 direction = new Vector2(1, 0);
    const int SPEED = 25;

    private AnimatedSprite animatedSprite;
    private Global global; 
    private GameEvent gameEvent;

    public override void _Ready() {
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        global = GetNode<Global>("/root/Global");
        gameEvent = GetNode<GameEvent>("/root/GameEvent");
    }

    public override void _PhysicsProcess(float delta)
    {
        this.GlobalPosition += direction * SPEED * delta;
    }

    private void _on_Person_area_entered(Area2D area) {
        if (area.IsInGroup("Player")) {
            global.savedPeopleCount += 1;
            gameEvent.EmitSignal("updatePeopleCount");
            this.QueueFree();
        }
    }

    private void _on_VisibilityNotifier2D_screen_exited() {
        this.QueueFree();
    }

    public void ChangeDirection() {
        direction = -direction;
        animatedSprite.FlipH = !animatedSprite.FlipH;
    }
}
