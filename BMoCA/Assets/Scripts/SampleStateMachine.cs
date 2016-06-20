using UnityEngine;
using System.Collections;

public class SampleStateMachine : MonoBehaviour {

	//Making the state machine class a singleton (optional, but kinda nice)
	private static SampleStateMachine instance;
	public static SampleStateMachine GetInstance(){
		return instance;
	}

	//Create your delegate -- the thing that will point to other functions
	private delegate void StateUpdate();
	private StateUpdate stateUpdate;

	//Declaring the enum, which is all the possible states in the game
	public enum State{
		FirstState,
		SecondState
	}

	//An instance of type State that will tell us what state we are currently in
	public State currentState;

	//A SetState function that we will call to change from one state to another
	void SetState(State newState){
		//Make our variable for holding the current state equal the new state
		currentState = newState;

		//Switch statement for all the cases of different states
		switch(newState){
		case State.FirstState:
			//Call the function to initialize that state
			//This is kind of like a start function for moving to that state in that it
			//only gets called when you first enter that state
			InitFirstState();

			//Set the delegate equal to state one's update function
			stateUpdate = FirstStateUpdate;

			//You must break out of cases in switch statements
			break;

		//Do the exact same thing for the next state
		case State.SecondState:
			InitSecondState ();
			stateUpdate = SecondStateUpdate;
			break;
		}
	}

	// Use this for initialization
	void Start () {
		//Set your singleton instance (this is optional, but kinda nice I think)
		instance = this;

		//In start, you want to set anything that will be shared between states,
		//and also set whatever state you want to start it
		SetState(State.FirstState);
	}
	
	// Update is called once per frame
	void Update () {
		//In update, you just want to do a safety check to make sure your delegate
		//points to SOME function, and if so run that the delegate every frame
		if (stateUpdate != null) {
			stateUpdate ();
		}
	}

	//The #region thing is just to stay organized, you don't have to use it at all (just a style thing)
	#region FIRST STATE
	void InitFirstState(){
		//Do whatever you want when you first enter the first state
	}

	void FirstStateUpdate(){
		//Do whatever you want to do every frame while in the first state
	}
	#endregion


	#region SECOND STATE
	void InitSecondState(){
		//Do whatever you want when you first enter the second state
	}

	void SecondStateUpdate(){
		//Do whatever you want to do every frame while in the second state
	}
	#endregion

	void SomethingHappenedThatChangesState(){
		//You can change the state anywhere just by calling the SetState function
		SetState(State.SecondState);
	}
}
