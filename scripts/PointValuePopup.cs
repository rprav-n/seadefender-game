using Godot;
using System;

public partial class PointValuePopup : Node2D
{	
	
	private Label pointLabel;
	
	Random random = new Random();
	
	const int SPEED = 15;
		
	public override void _Ready()
	{
		pointLabel = GetNode<Label>("PointLabel");
		
		this.RotationDegrees = random.Next(0, 360);
	}

	public override void _PhysicsProcess(double delta)
	{
		this.RotationDegrees = Mathf.Lerp(this.RotationDegrees, 0, 18 * (float)delta);
		
		var newGlobalPosition = this.GlobalPosition;
		newGlobalPosition.Y -= SPEED * (float)delta;
		
		this.GlobalPosition = newGlobalPosition;
	}

	public void SetPopupText(int score = 100) 
	{
		pointLabel.Text = score.ToString();
	}
}
