using Godot;
using System;

public class PersonUI : Sprite
{

    [Export]
    private int orderNumber = 1;
    private Global global;

    private Texture emptyTexture;
    private Texture fullTexture;
    private GameEvent gameEvent;

    public override void _Ready()
    {
        global = GetNode<Global>("/root/Global");

        emptyTexture = GD.Load<Texture>("res://assets/user_interface/people_count/person_empty_ui.png");
        fullTexture = GD.Load<Texture>("res://assets/user_interface/people_count/person_ui.png");
        gameEvent = GetNode<GameEvent>("/root/GameEvent");

        gameEvent.Connect("updatePeopleCount", this, "_on_updatePeopleCount");
    }

  /*   public override void _Process(float delta)
    {
        if (global.savedPeopleCount >= orderNumber) {
            this.Texture = fullTexture;
        }
    } */

    private void _on_updatePeopleCount() {
         if (global.savedPeopleCount >= orderNumber) {
            this.Texture = fullTexture;
        }
    }

}
