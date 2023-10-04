using Godot;
using System;

public partial class PersonUI : Sprite2D
{

	[Export]
	private int orderNumber = 1;
	private Global global;

	private Texture2D emptyTexture;
	private Texture2D fullTexture;
	private GameEvent gameEvent;

	public override void _Ready()
	{
		global = GetNode<Global>("/root/Global");

		emptyTexture = GD.Load<Texture2D>("res://assets/user_interface/people_count/person_empty_ui.png");
		fullTexture = GD.Load<Texture2D>("res://assets/user_interface/people_count/person_ui.png");
		gameEvent = GetNode<GameEvent>("/root/GameEvent");

		gameEvent.Connect("updatePeopleCountEventHandler", new Callable(this, "_on_updatePeopleCount"));
	}


	private void _on_updatePeopleCount() {
		 if (global.savedPeopleCount >= orderNumber) {
			this.Texture = fullTexture;
		}
	}

}
