using Godot;
using System;

public class Bullet : AnimatedSprite
{
    public Vector2 direction = new Vector2(1, 0);
    private const int SPEED = 300;

    Random r = new Random();

    public override void _Ready()
    {
        base._Ready();
        this.Play();

        this.RotationDegrees = r.Next(-7, 7);
        direction = direction.Rotated(this.Rotation);
    }

    public override void _PhysicsProcess(float delta)
    {
        this.GlobalPosition += direction * SPEED * delta;
    }

    private void _on_VisibilityNotifier2D_screen_exited()
    {
        this.QueueFree();
    }
}
