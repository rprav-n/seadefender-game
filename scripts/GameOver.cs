using Godot;
using System;

public partial class GameOver : Control
{
	private Global global;
	private GameEvent gameEvent;
	
	private Label currentScoreLabel;
	private Timer gameOverDelayTimer;
		
	public override void _Ready()
	{
		global = GetNode<Global>("/root/Global");
		gameEvent = GetNode<GameEvent>("/root/GameEvent");
		
		currentScoreLabel = GetNode<Label>("CurrentScoreLabel");
		gameOverDelayTimer = GetNode<Timer>("GameOverDelayTimer");
		
		gameEvent.Connect("GameOver", new Callable(this, "_on_activateGameOver"));
		
		this.Visible = false;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("shoot") && this.Visible) 
		{
			global.Reset();
			GetTree().ReloadCurrentScene();
		}
	}

	private void _on_activateGameOver() 
	{
		currentScoreLabel.Text = "Score " + global.currentPoints.ToString();
		gameOverDelayTimer.Start();
	}
	
	private void _on_game_over_delay_timer_timeout() 
	{
		this.Visible = true;
	}
		
}
