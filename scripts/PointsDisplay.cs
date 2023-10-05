using Godot;
using System;

public partial class PointsDisplay : Label
{

	private Global global;
	private GameEvent gameEvent;
	
	public override void _Ready()
	{
		global = GetNode<Global>("/root/Global");
		gameEvent = GetNode<GameEvent>("/root/GameEvent");
		
		gameEvent.Connect("UpdatePoints", new Callable(this, "_on_updatePoints"));
		
		_on_updatePoints();
	}
	
	private void _on_updatePoints() 
	{
		this.Text = global.current_points.ToString();
	}
}
