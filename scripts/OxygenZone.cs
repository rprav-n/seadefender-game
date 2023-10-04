using Godot;
using System;

public partial class OxygenZone : Area2D
{

	private Global global;
	private Node gameEvent;

	public override void _Ready()
	{
		global = GetNode<Global>("/root/Global");
		gameEvent = GetNode<Node>("/root/GameEvent");
	}
	
	private void _on_OxygenZone_area_entered(Area2D area) 
	{
		if (area.IsInGroup("Player")) 
		{
			if (global.savedPeopleCount >= 7) 
			{
				gameEvent.EmitSignal("FullCrewOxygenRefuel");
			} else 
			{
				gameEvent.EmitSignal("LessCrewOxygenRefuel");
			}
		}
	}
}
