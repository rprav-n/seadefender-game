using Godot;
using System;

public partial class SoundManager : Node
{
	
	private CSharpScript soundScript;
	
	Random random = new Random();

	const float MIN_PITCH = 0.8f;
	const float MAX_PITCH = 1.2f;

	public override void _Ready()
	{
		soundScript = ResourceLoader.Load<CSharpScript>("res://scripts/Singleton/Sound Manager/Sound.cs");
	}

	public void playSound(AudioStream sound) 
	{
		
		var audioStreamPlayer = new Sound();	
		audioStreamPlayer.Stream = sound;
	
		var randomPicth = random.NextSingle() * (MAX_PITCH - MIN_PITCH) + MIN_PITCH;
		audioStreamPlayer.PitchScale = randomPicth;
				
		this.AddChild(audioStreamPlayer);
		audioStreamPlayer.Play();
	}
}
