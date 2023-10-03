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
	const float OXYGEN_DECREASE_SPEED = 2.5f;

	private Global global;
	
	private string state = "default";
	
	public override void _Ready()
	{
		base._Ready();
		BulletScene = GD.Load<PackedScene>("res://scenes/Bullet.tscn");
		reloadTimer = GetNode<Timer>("ReloadTimer");
		animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		
		global = GetNode<Global>("/root/Global");
	}

	public override void _Process(float delta)
	{
		if (state == "default") 
		{
			playerInput();
			playerDirection();
			playerShoot();
			playerOxygen();	
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		if (state == "default") 
		{
			playerMovement();
		}
	}

	private void playerInput() 
	{
		velocity.x = Input.GetAxis("move_left", "move_right");
		velocity.y = Input.GetAxis("move_up", "move_down");

		velocity = velocity.Normalized();
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

	private void playerOxygen() 
	{
		global.oxygenLevel -= OXYGEN_DECREASE_SPEED * GetProcessDeltaTime();
	}

	private void playerMovement() 
	{
		this.GlobalPosition += velocity * SPEED * GetPhysicsProcessDeltaTime();
	}

	private void _on_ReloadTimer_timeout()
	{
		canShoot = true;
	}
}
