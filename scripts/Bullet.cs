using Godot;
using System;

public partial class Bullet : Area2D
{
    public Vector2 direction = new Vector2(1, 0);
    private const int SPEED = 300;

    Random r = new Random();

    private AnimatedSprite2D animatedSprite;

    public override void _Ready()
    {
        base._Ready();
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        animatedSprite.Frame = 0;
        animatedSprite.Play();

        this.RotationDegrees = r.Next(-3, 3);
        direction = direction.Rotated(this.Rotation) * (animatedSprite.FlipH ? -1 : 1);
    }

    public override void _PhysicsProcess(double delta)
    {
        this.GlobalPosition += direction * SPEED * (float)delta;
    }

    private void _on_VisibilityNotifier2D_screen_exited()
    {
        this.QueueFree();
    }

    public void ChangeFlipHTo(bool val)
    {
        animatedSprite.FlipH = val;
    }
}
