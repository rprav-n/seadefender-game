using Godot;
using System;

public partial class Person : Area2D
{
	private Vector2 direction = new Vector2(1, 0);
	const int SPEED = 25;

	private AnimatedSprite2D animatedSprite;
	private Global global; 
	private Node gameEvent;
	
	const int POINT_VALUE = 30;

	public override void _Ready() {
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		global = GetNode<Global>("/root/Global");
		gameEvent = GetNode<GameEvent>("/root/GameEvent");
	}

	public override void _PhysicsProcess(double delta)
	{
		this.GlobalPosition += direction * SPEED * (float)delta;
	}
	
	public override void _Process(double delta)
	{
		if (GlobalPosition.X >= global.SCREEN_BOUND_MAX_X || GlobalPosition.X <= global.SCREEN_BOUND_MIN_X) 
		{
			this.QueueFree();
		}
	}

	private void _on_Person_area_entered(Area2D area) {
		if (area.IsInGroup("Player")) {
			global.savedPeopleCount += 1;
			global.current_points += POINT_VALUE;
			gameEvent.EmitSignal("UpdatePeopleCount");
			gameEvent.EmitSignal("UpdatePoints");
			this.QueueFree();
		}
	}
	

	public void ChangeDirection() {
		direction = -direction;
		animatedSprite.FlipH = !animatedSprite.FlipH;
	}
}
