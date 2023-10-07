using Godot;
using System;

public partial class GameOver : Control
{
	private Global global;
	private GameEvent gameEvent;
	
	private Label currentScoreLabel;
	private Label highScoreLabel;
	private Timer gameOverDelayTimer;
	
	private SoundManager soundManager;
	private AudioStream gameOverSound;
		
	public override void _Ready()
	{
		global = GetNode<Global>("/root/Global");
		gameEvent = GetNode<GameEvent>("/root/GameEvent");
		
		currentScoreLabel = GetNode<Label>("CurrentScoreLabel");
		highScoreLabel = GetNode<Label>("HighScoreLabel");
		gameOverDelayTimer = GetNode<Timer>("GameOverDelayTimer");
		
		gameEvent.Connect("GameOver", new Callable(this, "_on_activateGameOver"));
		
		soundManager = GetNode<SoundManager>("/root/SoundManager");
		
		gameOverSound = GD.Load<AudioStream>("res://assets/player/game_over.ogg");
		
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
		if (global.currentPoints > global.highScore) 
		{
			global.highScore = global.currentPoints;
		}
		highScoreLabel.Text = "Highscore " + global.highScore.ToString(); 
		gameOverDelayTimer.Start();
	}
	
	private void _on_game_over_delay_timer_timeout() 
	{
		this.Visible = true;
		soundManager.playSound(gameOverSound);
	}
		
}
