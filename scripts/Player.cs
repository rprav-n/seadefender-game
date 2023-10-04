using Godot;
using System;

public partial class Player : Area2D
{

	private Vector2 velocity;
	private Vector2 SPEED = new Vector2(125, 90);
	
	private PackedScene BulletScene;
	private AnimatedSprite2D animatedSprite;
	private bool canShoot = true;

	private Timer reloadTimer;
	const int BULLETOFFSET = 7;
	const float OXYGEN_DECREASE_SPEED = 2.5f;
	const float OXYGEN_INCREASE_SPEED = 20f;

	private Global global;
	private GameEvent gameEvent;
	
	private string state = "default";
	
	public override void _Ready()
	{
		base._Ready();
		BulletScene = GD.Load<PackedScene>("res://scenes/Bullet.tscn");
		reloadTimer = GetNode<Timer>("ReloadTimer");
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		global = GetNode<Global>("/root/Global");
		gameEvent = GetNode<GameEvent>("/root/GameEvent");
		
		connectSignals();
	}
	
	private void connectSignals() 
	{
		gameEvent.Connect("fullCrewOxygenRefuelEventHandler", new Callable(this, "_on_fullCrewOxygenRefuel"));
		gameEvent.Connect("lessCrewOxygenRefuelEventHandler", new Callable(this, "_on_lessCrewOxygenRefuel"));
	}

	public override void _Process(double delta)
	{
		if (state == "default") 
		{
			playerInput();
			playerDirection();
			playerShoot();
			playerLoseOxygen();	
		} else if (state == "less_people_refuel") 
		{
			oxygenRefuel();
		} else if (state == "full_people_refuel") 
		{
			oxygenRefuel();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (state == "default") 
		{
			playerMovement();
		}
	}

	private void playerInput() 
	{
		velocity.X = Input.GetAxis("move_left", "move_right");
		velocity.Y = Input.GetAxis("move_up", "move_down");

		velocity = velocity.Normalized();
	}

	private void playerDirection()
	{
		if (velocity.X != 0)
		{
			this.animatedSprite.FlipH = velocity.X == -1;
		}
	}

	private void playerShoot()
	{
		if (Input.IsActionPressed("shoot") && canShoot)
		{
			var bulletInstance = BulletScene.Instantiate<Bullet>();
			GetTree().CurrentScene.AddChild(bulletInstance);


			if (animatedSprite.FlipH)
			{
				bulletInstance.direction.X = -1;
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

	private void playerMovement() 
	{
		this.GlobalPosition += velocity * SPEED * (float)GetPhysicsProcessDeltaTime();
	}

	private void playerLoseOxygen() 
	{
		var oxygenDecreaseDelta = OXYGEN_DECREASE_SPEED * GetProcessDeltaTime();
		global.oxygenLevel = (float)Mathf.MoveToward(global.oxygenLevel, 0, oxygenDecreaseDelta);
		
	}

	private void _on_ReloadTimer_timeout()
	{
		canShoot = true;
	}
	
	private void _on_fullCrewOxygenRefuel() 
	{
		state = "full_people_refuel";
	}
	
	private void _on_lessCrewOxygenRefuel() 
	{
		state = "less_people_refuel";
	}
	
	private void oxygenRefuel() 
	{
		global.oxygenLevel += (float)(OXYGEN_INCREASE_SPEED * GetProcessDeltaTime());
		
		if (global.oxygenLevel >= 100) 
		{
			state = "default";	
		}
	}
}
