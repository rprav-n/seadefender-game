using Godot;
using System;

public partial class OxygenBar : TextureProgressBar
{
	
	private Global global;
	
	public override void _Ready()
	{
		global = GetNode<Global>("/root/Global");
	}

	public override void _Process(double delta)
	{
		Value = global.oxygenLevel;
	}
}
