using Godot;
using System;

public partial class Sound : AudioStreamPlayer
{

	public override void _Ready()
	{
		this.Connect("finished", new Callable(this, "_on_Finished"));
	}
	
	private void _on_Finished() 
	{
		this.QueueFree();
	}

}
