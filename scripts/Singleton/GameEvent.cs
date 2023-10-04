using Godot;

public partial class  GameEvent: Node {

	[Signal]
	public delegate void updatePeopleCountEventHandler();
	
	[Signal]
	public delegate void fullCrewOxygenRefuelEventHandler();
	
	[Signal]
    public delegate void lessCrewOxygenRefuelEventHandler();
}