using Godot;
using System;

public partial class GameOver : Control
{
	private Global global;
	private GameEvent gameEvent;
	
	private Label currentScoreLabel;
		
	public override void _Ready()
	{
		global = GetNode<Global>("/root/Global");
		gameEvent = GetNode<GameEvent>("/root/GameEvent");
		
		currentScoreLabel = GetNode<Label>("CurrentScoreLabel");
		
		gameEvent.Connect("GameOver", new Callable(this, "_on_activateGameOver"));
		
		this.Visible = false;
	}
	
	private void _on_activateGameOver() 
	{
		currentScoreLabel.Text = "Score " + global.current_points.ToString();
		this.Visible = true;
	}
	
}
