using Godot;
using System;

public class Player : Area2D
{

	private Vector2 velocity;
	private Vector2 SPEED = new Vector2(125, 90);

	private PackedScene BulletScene;
	private AnimatedSprite animatedSprite;
	private bool canShoot = true;

	private Timer reloadTimer;
	const int BULLETOFFSET = 7;

	public override void _Ready()
	{
		base._Ready();
		BulletScene = GD.Load<PackedScene>("res://scenes/Bullet.tscn");
		reloadTimer = GetNode<Timer>("ReloadTimer");
		animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
	}

	public override void _Process(float delta)
	{
		velocity.x = Input.GetAxis("move_left", "move_right");
		velocity.y = Input.GetAxis("move_up", "move_down");

		playerDirection();
		playerShoot();

		velocity = velocity.Normalized();
	}

	public override void _PhysicsProcess(float delta)
	{
		this.GlobalPosition += velocity * SPEED * delta;
	}

	private void playerDirection()
	{
		if (velocity.x != 0)
		{
			this.animatedSprite.FlipH = velocity.x == -1;
		}
	}

	private void playerShoot()
	{
		if (Input.IsActionPressed("shoot") && canShoot)
		{
			var bulletInstance = BulletScene.Instance<Bullet>();
			GetTree().CurrentScene.AddChild(bulletInstance);


			if (animatedSprite.FlipH)
			{
				bulletInstance.direction.x = -1;
				bulletInstance.ChangeFlipHTo(true);
				bulletInstance.GlobalPosition = this.GlobalPosition - new Vector2(BULLETOFFSET, 0);
			} else
			{
				bulletInstance.GlobalPosition = this.GlobalPosition + new Vector2(BULLETOFFSET, 0);
			}

			canShoot = false;
			reloadTimer.Start();
		}
	}

	private void _on_ReloadTimer_timeout()
	{
		canShoot = true;
	}
}
