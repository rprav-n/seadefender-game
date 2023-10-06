using Godot;

public partial class GameEvent: Node {

	[Signal]
	public delegate void UpdatePeopleCountEventHandler();
	
	[Signal]
	public delegate void FullCrewOxygenRefuelEventHandler();
	
	[Signal]
	public delegate void LessCrewOxygenRefuelEventHandler();
	
	[Signal]
	public delegate void CameraFollowPlayerEventHandler(float yPos);
	
	[Signal]
	public delegate void UpdatePointsEventHandler();
	
	[Signal]
	public delegate void GameOverEventHandler();
	
	[Signal]
	public delegate void PauseEnemiesEventHandler(bool paused);
}