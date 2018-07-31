using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_InputPoller : InputPoller {

    public override InputState GetPlayer1Input()
    {
        InputState IS = InputState.GetBlankState();
        IS.AddAxis("LookHorizontal", Input.GetAxisRaw("Mouse Y"));         //These 2 items are typically for looking around (mouse/trackpad)
        IS.AddAxis("LookVertical", Input.GetAxisRaw("Mouse X"));
        IS.AddAxis("MoveHorizontal", Input.GetAxisRaw("Horizontal"));      //These 2 items are typically for movement (W, A, S, D)
        IS.AddAxis("MoveVertical", Input.GetAxisRaw("Vertical"));
        IS.AddButton("ActionMain", Input.GetButtonDown("Fire1"));       //Typically shoot (mouse1)
        IS.AddButton("ActionSecondary", Input.GetButtonDown("Fire2"));  //Typically zoom (mouse2)
        IS.AddButton("Interact", Input.GetButtonDown("Fire3"));         //Typically interact (E or F)
        IS.AddButton("Ability1", Input.GetButton("Fire4"));             //Typically jumping (spacebar)
        IS.AddButton("Ability2", Input.GetButton("Fire5"));             //Typically sprint (left shift)
        IS.AddButton("Ability3", Input.GetButton("Fire6"));             //Typically crouch (left ctrl)
        IS.AddButton("Cancel", Input.GetButtonDown("Cancel"));          //Typically used for pausing (escape)
        return IS;
    }
}
