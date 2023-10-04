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
}