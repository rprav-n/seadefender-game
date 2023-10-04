using Godot;
using System;

public partial class OxygenZone : Area2D
{

	private Global global;
	private GameEvent gameEvent;

	public override void _Ready()
	{
		global = GetNode<Global>("/root/Global");
		gameEvent = GetNode<GameEvent>("/root/GameEvent");
	}
	
	private void _on_OxygenZone_area_entered(Area2D area) 
	{
		if (area.IsInGroup("Player")) 
		{
			if (global.savedPeopleCount >= 7) 
			{
				gameEvent.EmitSignal("fullCrewOxygenRefuelEventHandler");
			} else 
			{
				gameEvent.EmitSignal("lessCrewOxygenRefuelEventHandler");
			}
		}
	}
}
