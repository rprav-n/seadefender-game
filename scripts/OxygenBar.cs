using System.Globalization;
using Godot;
using System;

public partial class OxygenBar : Node2D
{
	
	private Global global;
	private TextureProgressBar textureProgress;
	
	Random r = new Random();
	
	private int previousAmount = 0;
	
	public override void _Ready()
	{
		global = GetNode<Global>("/root/Global");
		textureProgress = GetNode<TextureProgressBar>("TextureProgress");
	}

	public override void _PhysicsProcess(double delta)
	{
		var newScale = this.Scale;
		newScale.X = (float)Mathf.Lerp(newScale.X, 1, 6 * delta);
		newScale.Y = (float)Mathf.Lerp(newScale.Y, 1, 6 * delta);
		
		this.Scale = newScale;
		
		this.RotationDegrees = (float)Mathf.Lerp(this.RotationDegrees, 0, 6 * delta);
	}
	

	
	public override void _Process(double delta)
	{
		textureProgress.Value = global.oxygenLevel;
		
		int amountRounded = (int)Math.Round(global.oxygenLevel);
		
		if (amountRounded != previousAmount) 
		{
			if (amountRounded == 25) 
			{
				alert(1.25f, 5);
			} else if (amountRounded == 15) 
			{
				alert(1.3f, 7);
			} else if (amountRounded == 10) 
			{
				alert(1.35f, 10);
			} else if (amountRounded == 7) 
			{
				alert(1.4f, 15);
			} else if (amountRounded == 5) 
			{
				alert(1.5f, 20);
			} else if (amountRounded == 2) 
			{
				alert(1.6f, 25);
			} else if (amountRounded == 1) 
			{
				alert(1.8f, 35);
			}
			
			previousAmount = amountRounded;
		}
	}
	
	private void alert(float scaleVal, int rotVal) 
	{
		this.Scale = new Vector2(scaleVal, scaleVal);
		this.RotationDegrees = r.Next(-rotVal, rotVal);
	}
}
