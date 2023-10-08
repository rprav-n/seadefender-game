using Godot;
using System;

public partial class Player : Area2D
{

	private Vector2 velocity;
	private Vector2 SPEED = new Vector2(125, 90);
	const int ROTATION_STRENGTH = 15;
	
	private PackedScene BulletScene;
	private AnimatedSprite2D animatedSprite;
	private bool canShoot = true;
	private bool isShooting = false;

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
	const int PIECE_COUNT = 10;
	
	private Global global;
	private GameEvent gameEvent;
	private SoundManager soundManager;
	
	enum States 
	{
		Default,
		OxygenRefuel,
		PeopleRefuel
	}
	
	private States state = States.Default;
	
	private AudioStream shootSound;
	private AudioStream deathSound;
	private AudioStream oxygenFullSound;
	
	private PackedScene objectScene;
	
	private Texture2D playerPieceTexture;
	
	public override void _Ready()
	{
		
		BulletScene = GD.Load<PackedScene>("res://scenes/Bullet.tscn");
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		reloadTimer = GetNode<Timer>("ReloadTimer");
		decreasePeopleTimer = GetNode<Timer>("DecreasePeopleTimer");
		
		global = GetNode<Global>("/root/Global");
		gameEvent = GetNode<GameEvent>("/root/GameEvent");
		soundManager = GetNode<SoundManager>("/root/SoundManager");
		
		shootSound = GD.Load<AudioStream>("res://assets/player/player_bullet/player_shoot.ogg");
		deathSound = GD.Load<AudioStream>("res://assets/player/player_death.ogg");
		oxygenFullSound = GD.Load<AudioStream>("res://assets/user_interface/oxygen_bar/full_oxygen_alert.ogg");
		
		objectScene = GD.Load<PackedScene>("res://scenes/ObjectPiece.tscn");
		playerPieceTexture = GD.Load<Texture2D>("res://assets/player/player_pieces.png");
		
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
			playerRotateToMovement();
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
		if (velocity.X != 0 && !isShooting)
		{
			this.animatedSprite.FlipH = velocity.X < 0;
		}
	}
	
	private void playerRotateToMovement() 
	{
		var rotationTarget = 0;
		if (velocity.Y == 0) 
		{
			rotationTarget = (int)velocity.X * ROTATION_STRENGTH;
		} else 
		{
			if (animatedSprite.FlipH) 
			{
				rotationTarget = -1 * (int)velocity.Y * ROTATION_STRENGTH;	
			} else 
			{
				rotationTarget = (int)velocity.Y * ROTATION_STRENGTH;
			}
			
		}
		
		this.RotationDegrees = Mathf.Lerp(this.RotationDegrees, rotationTarget, 15 * (float)GetPhysicsProcessDeltaTime());	
		
	}

	private void playerShoot()
	{
		
		if (Input.IsActionPressed("shoot")) 
		{
			isShooting = true;
		} else 
		{
			isShooting = false;
		}
		
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
			soundManager.playSound(shootSound);
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
		animatedSprite.Play("flash");
		decreasePeopleTimer.Start();
		deathWhenRefuelingWhileFull();
		gameEvent.EmitSignal("PauseEnemies", true);
		gameEvent.EmitSignal("KillAllSharks");
	}
	
	private void _on_lessCrewOxygenRefuel() 
	{
		removeOnePerson();
		state = States.OxygenRefuel;
		animatedSprite.Play("flash");
		deathWhenRefuelingWhileFull();
		gameEvent.EmitSignal("PauseEnemies", true);
	}
	
	private void oxygenRefuel() 
	{
		global.oxygenLevel += (float)(OXYGEN_INCREASE_SPEED * GetProcessDeltaTime());
		
		if (global.oxygenLevel >= 100) 
		{
			state = States.Default;
			animatedSprite.Play("default");
			gameEvent.EmitSignal("PauseEnemies", false);
			soundManager.playSound(oxygenFullSound);
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
		soundManager.playSound(deathSound);
		instanceDeathPieces();
		this.QueueFree();
	}
	
	private void instanceDeathPieces() 
	{
		for (var i = 0; i < PIECE_COUNT; i++) 
		{
			var pieceInstance = objectScene.Instantiate<ObjectPiece>();
			pieceInstance.Texture = playerPieceTexture;
			pieceInstance.Hframes = PIECE_COUNT;
			pieceInstance.Frame = i;
			
			GetTree().CurrentScene.AddChild(pieceInstance);
			
			pieceInstance.GlobalPosition = this.GlobalPosition;
		}
	}
}
