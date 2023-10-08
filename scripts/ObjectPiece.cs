using Godot;
using System;

public partial class ObjectPiece : Sprite2D
{
	
	private Vector2 direction = new Vector2(1, 0);
	
	private int moveSpeed = 150;
	private int rotationSpeed = 50;
	
	Random random = new Random();
	
	public override void _Ready()
	{
		var randomAngle = Mathf.DegToRad(random.Next(0, 360));
		direction = direction.Rotated(randomAngle);
		rotationSpeed = random.Next(-70, 70);
	}

	public override void _Process(double delta)
	{
		
	}

	public override void _PhysicsProcess(double delta)
	{
		this.GlobalPosition += direction * moveSpeed * (float)delta;
		this.RotationDegrees += rotationSpeed * (float)delta;
		
		moveSpeed = (int)Mathf.Lerp(moveSpeed, 0, 6 * delta);
	}


}
