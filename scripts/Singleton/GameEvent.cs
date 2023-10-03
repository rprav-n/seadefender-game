using Godot;

class GameEvent: Node {

	[Signal]
	public delegate void updatePeopleCount();
	
	[Signal]
	public delegate void fullCrewOxygenRefuel();
	
	[Signal]
    public delegate void lessCrewOxygenRefuel();
}