using Godot;
using System;

public class Shark : Area2D
{
    const int SPEED = 50;
    const float MOVEMENT_FREQUENCY = 0.15f;

    private Vector2 direction = new Vector2(1, 0);
    private AnimatedSprite animatedSprite;

    public override void _Ready()
    {
        base._Ready();
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        animatedSprite.Play();
    }

    public override void _PhysicsProcess(float delta)
    {
        direction.y = Mathf.Sin(this.GlobalPosition.x * MOVEMENT_FREQUENCY) * 0.5f;
        this.GlobalPosition += direction * SPEED * delta;
    }

    private void _on_Shark_area_entered(Area2D area)
    {
        if (area is Bullet bullet)
        {
            bullet.QueueFree();
            this.QueueFree();
        }
    }

    private void _on_VisibilityNotifier2D_screen_exited()
    {
        this.QueueFree();
    }

    public void ChangeDirection()
    {
        direction = -direction;
        animatedSprite.FlipH = !animatedSprite.FlipH;
    }
}
