using Godot;

public partial class Global: Node {
	public int savedPeopleCount = 0;
	public float oxygenLevel = 100f;
	public int currentPoints = 0;
	public int highScore = 0;
	
	public int SCREEN_BOUND_MAX_X = 300;
	public int SCREEN_BOUND_MIN_X = -50;
	
	public void Reset() 
	{
		savedPeopleCount = 0;
		oxygenLevel = 100f;
		currentPoints = 0;
		highScore = currentPoints;
	}
}