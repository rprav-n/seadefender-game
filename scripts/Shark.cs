using Godot;
using System;

public partial class Shark : Area2D
{
	const int SPEED = 50;
	const float MOVEMENT_FREQUENCY = 0.15f;
	
	

	private Vector2 direction = new Vector2(1, 0);
	private AnimatedSprite2D animatedSprite;
	
	private Global global;
	private GameEvent gameEvent;
	
	const int POINT_VALUE = 25;
	
	private string state = "default";

	public override void _Ready()
	{
		base._Ready();
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite.Play();
		
		global = GetNode<Global>("/root/Global");
		gameEvent = GetNode<GameEvent>("/root/GameEvent");
		
		gameEvent.Connect("PauseEnemies", new Callable(this, "_on_PauseEnemies"));
	}

	public override void _PhysicsProcess(double delta)
	{
		if (state == "default") 
		{
			direction.Y = Mathf.Sin(this.GlobalPosition.X * MOVEMENT_FREQUENCY) * 0.5f;
			this.GlobalPosition += direction * SPEED * (float)delta;	
		}
	}

	public override void _Process(double delta)
	{
		if (GlobalPosition.X >= global.SCREEN_BOUND_MAX_X || GlobalPosition.X <= global.SCREEN_BOUND_MIN_X) 
		{
			this.QueueFree();
		}
	}

	private void _on_Shark_area_entered(Area2D area)
	{
		if (area is Bullet bullet)
		{
			bullet.QueueFree();
			this.QueueFree();
			
			global.currentPoints += POINT_VALUE;
			
			gameEvent.EmitSignal("UpdatePoints");
		}
		
		if (area.IsInGroup("Player")) 
		{
			gameEvent.EmitSignal("GameOver");
		}
	}


	public void ChangeDirection()
	{
		direction = -direction;
		animatedSprite.FlipH = !animatedSprite.FlipH;
	}
	
	private void _on_PauseEnemies(bool pause) 
	{
		if (pause) 
		{
			state = "paused";
		} else 
		{
			state = "default";
		}
	}
}