using Godot;
using System;

public class OxygenZone : Area2D
{

	private Global global;
	private GameEvent gameEvent;

	public override void _Ready()
	{
		global = GetNode<Global>("/root/Global");
		gameEvent = GetNode<GameEvent>("/root/GameEvent");
	}

	public override void _Process(float delta)
	{
		
	}
	
	private void _on_OxygenZone_area_entered(Area2D area) 
	{
		if (area.IsInGroup("Player")) 
		{
			if (global.savedPeopleCount >= 7) 
			{
				gameEvent.EmitSignal("fullCrewOxygenRefuel");
			} else 
			{
				gameEvent.EmitSignal("lessCrewOxygenRefuel");
			}
		}
	}
}
