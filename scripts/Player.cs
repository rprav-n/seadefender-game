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
	private Timer decreasePeopleTimer;
	
	const int BULLETOFFSET = 7;
	const float OXYGEN_DECREASE_SPEED = 2.5f;
	const float OXYGEN_INCREASE_SPEED = 20f;
	const int OXYGEN_REFUEL_Y_POS = 38;
	const float OXYGEN_REFUEL_SPEED = 70;
	
	const int MIN_X_POS = 12;
	const int MAX_X_POS = 246;
	const int MIN_Y_POS = OXYGEN_REFUEL_Y_POS;
	const int MAX_Y_POS = 205;
	
	private Global global;
	private GameEvent gameEvent;
	
	enum States 
	{
		Default,
		OxygenRefuel,
		PeopleRefuel
	}
	
		private States state = States.Default;
	
	public override void _Ready()
	{
		
		BulletScene = GD.Load<PackedScene>("res://scenes/Bullet.tscn");
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		reloadTimer = GetNode<Timer>("ReloadTimer");
		decreasePeopleTimer = GetNode<Timer>("DecreasePeopleTimer");
		
		global = GetNode<Global>("/root/Global");
		gameEvent = GetNode<GameEvent>("/root/GameEvent");
		
		connectSignals();
	}
	
	private void connectSignals() 
	{	
		gameEvent.Connect("FullCrewOxygenRefuel", new Callable(this, "_on_fullCrewOxygenRefuel"));
		gameEvent.Connect("LessCrewOxygenRefuel", new Callable(this, "_on_lessCrewOxygenRefuel"));
		gameEvent.Connect("GameOver", new Callable(this, "_on_GameOver"));
	}

	public override void _Process(double delta)
	{
		if (state == States.Default) 
		{
			playerInput();
			playerDirection();
			playerShoot();
			playerLoseOxygen();	
			deathWhenOxygenReachesZero();
		} else if (state == States.OxygenRefuel) 
		{
			oxygenRefuel();
			moveToShoreLine();
		} else if (state == States.PeopleRefuel) 
		{
			moveToShoreLine();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (state == States.Default) 
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
			this.animatedSprite.FlipH = velocity.X < 0;
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
		clampPlayerPosition();
		gameEvent.EmitSignal("CameraFollowPlayer", this.GlobalPosition.Y);
	}
	
	private void clampPlayerPosition() 
	{
		var newGlobalPosition = GlobalPosition;
		newGlobalPosition.X = Mathf.Clamp(newGlobalPosition.X, MIN_X_POS, MAX_X_POS);
		newGlobalPosition.Y = Mathf.Clamp(newGlobalPosition.Y, MIN_Y_POS, MAX_Y_POS);
		this.GlobalPosition = newGlobalPosition;
	}

	private void playerLoseOxygen() 
	{
		var oxygenDecreaseDelta = OXYGEN_DECREASE_SPEED * GetProcessDeltaTime();
		global.oxygenLevel = (float)Mathf.MoveToward(global.oxygenLevel, 0, oxygenDecreaseDelta);
	}

	private void deathWhenOxygenReachesZero() 
	{
		if (global.oxygenLevel <= 0) 
		{
			gameEvent.EmitSignal("GameOver");
		}
	}
	
	private void deathWhenRefuelingWhileFull() 
	{
		if (global.oxygenLevel > 80) 
		{
			gameEvent.EmitSignal("GameOver");
		}
	}
	

	private void _on_ReloadTimer_timeout()
	{
		canShoot = true;
	}
	
	private void _on_fullCrewOxygenRefuel() 
	{
		state = States.PeopleRefuel;
		decreasePeopleTimer.Start();
		deathWhenRefuelingWhileFull();
		gameEvent.EmitSignal("PauseEnemies", true);
	}
	
	private void _on_lessCrewOxygenRefuel() 
	{
		removeOnePerson();
		state = States.OxygenRefuel;
		deathWhenRefuelingWhileFull();
		gameEvent.EmitSignal("PauseEnemies", true);
	}
	
	private void oxygenRefuel() 
	{
		global.oxygenLevel += (float)(OXYGEN_INCREASE_SPEED * GetProcessDeltaTime());
		
		if (global.oxygenLevel >= 100) 
		{
			state = States.Default;
			gameEvent.EmitSignal("PauseEnemies", false);
			//TODO moveBelowShoreLine();
		}
	}
	
	private void moveToShoreLine() 
	{
		var moveSpeed = OXYGEN_REFUEL_SPEED * GetProcessDeltaTime();
		var newGlobalPosition = GlobalPosition;
		newGlobalPosition.Y = (float)Mathf.MoveToward(newGlobalPosition.Y, OXYGEN_REFUEL_Y_POS, moveSpeed);
		GlobalPosition = newGlobalPosition;
	}
	
	private void moveBelowShoreLine() 
	{
		// var moveSpeed = OXYGEN_REFUEL_SPEED * GetProcessDeltaTime();
		var newGlobalPosition = GlobalPosition;
		newGlobalPosition.Y = 100;
		GlobalPosition = newGlobalPosition;
	}
	
	private void _on_decrease_people_timer_timeout() 
	{
		removeOnePerson();
		if (global.savedPeopleCount <= 0) 
		{
			state = States.OxygenRefuel;
			decreasePeopleTimer.Stop();
		}
	}
	
	private void removeOnePerson() 
	{
		if (global.savedPeopleCount > 0) 
		{
			global.savedPeopleCount -= 1;
			gameEvent.EmitSignal("UpdatePeopleCount");	
		}
	}

	private void _on_GameOver() 
	{
		gameEvent.EmitSignal("PauseEnemies", true);
		this.QueueFree();
	}
}
