using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	public GameState State;
	public static event Action<GameState> OnGameStateChanged;

	public GameObject TeamASpawner;
	public GameObject TeamBSpawner;

	void Awake() {
		Instance = this;

	}

    void Start()
    {
        UpdateGameState(GameState.PreGame);
    }

    void Update(){

    }

    public void StartGame(bool startingTeam){
    	if (startingTeam == true){
    		UpdateGameState(GameState.TeamATurn);
    	}
    	else{
    		UpdateGameState(GameState.TeamBTurn);
    	}
    }

    // Update is called once per frame
    public void UpdateGameState(GameState newState)
    {
    	State = newState;

    	switch(newState){
    		case GameState.PreGame:
    			break;
    		case GameState.TeamATurn:
    			HandleTeamATurn();
    			break;
    		case GameState.TeamBTurn:
    			HandleTeamBTurn();
    			break;
    		case GameState.TeamASkullTurn:
    			break;
    		case GameState.TeamBSkullTurn:
    			break;
    		case GameState.PostGame:
    			break;
    		default:
    			Debug.Log("State Does Not Exist");
    			break;
    	}
        OnGameStateChanged?.Invoke(newState);
    }

	private void HandleTeamATurn(){

		//spawn X number of wood blocks
		TeamASpawner.GetComponent<Spawner>().Spawn();
		TeamASpawner.GetComponent<Spawner>().Spawn();
	
		
	}

	private void HandleTeamBTurn(){

		//spawn X number of wood blocks
		TeamBSpawner.GetComponent<Spawner>().Spawn();
		TeamBSpawner.GetComponent<Spawner>().Spawn();

	
	}

}

public enum GameState{
	PreGame,
	TeamATurn,
	TeamBTurn,
	TeamASkullTurn,
	TeamBSkullTurn,
	PostGame


}
