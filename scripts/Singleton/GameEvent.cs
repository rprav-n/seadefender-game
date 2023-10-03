using Godot;

class GameEvent: Node {

    [Signal]
    public delegate void updatePeopleCount();
}