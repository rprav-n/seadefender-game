using Godot;
using System;
using System.Net;

public partial class Camera : Camera2D
{
	private GameEvent gameEvent;
	
	private Vector2 targetPosition = Vector2.Zero;
	
	const int FOLLOW_SPEED = 5;
	const int MIN_Y_POS = 50;
	const int MAX_Y_POS = 150;
	
	public override void _Ready()
	{
		gameEvent = GetNode<GameEvent>("/root/GameEvent");
		gameEvent.Connect("CameraFollowPlayer", new Callable(this, "_on_CameraFollowPlayer"));
		targetPosition = this.GlobalPosition;
	}

	public override void _PhysicsProcess(double delta)
	{
		var newGlobalPosition = this.GlobalPosition;
		
		newGlobalPosition.Y = (float)Mathf.Lerp(newGlobalPosition.Y, targetPosition.Y, FOLLOW_SPEED * delta);
		
		this.GlobalPosition = newGlobalPosition;
	}

	private void _on_CameraFollowPlayer(float playerYPos) 
	{
		
		targetPosition.Y = Mathf.Clamp(playerYPos, MIN_Y_POS, MAX_Y_POS);
	}

	
}
