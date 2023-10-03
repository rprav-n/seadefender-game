using Godot;
using System;

public class OxygenBar : TextureProgress
{
	
	private Global global;
	
	public override void _Ready()
	{
		global = GetNode<Global>("/root/Global");
	}

	public override void _Process(float delta)
	{
		Value = global.oxygenLevel;
	}
}
