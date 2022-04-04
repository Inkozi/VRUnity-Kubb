using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	public GameState State;
	public static event Action<GameState> OnGameStateChanged;


  public GameObject TeamABlocks;
  public GameObject TeamASkulls;
  public GameObject TeamARebuttalSkulls;

  public GameObject TeamBBlocks;
  public GameObject TeamBSkulls;
  public GameObject TeamBRebuttalSkulls;

  private bool gameStarted = false;

	void Awake() {
		Instance = this;

	}

    void Start()
    {
        //UpdateGameState(GameState.PreGame);
    }

    void Update(){

    }


    public void StartGame(bool startingTeam){
      if (gameStarted == false){
        HandlePreGame();
        if (startingTeam == true){
          UpdateGameState(GameState.TeamATurn);
        }
        else{
          UpdateGameState(GameState.TeamBTurn);
        }
      }
    }

    // Update is called once per frame
    public void UpdateGameState(GameState newState)
    {
    	State = newState;

    	switch(newState){
    		case GameState.PreGame:
          HandlePreGame();
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
    		case GameState.TeamAVictory:
    			break;
        case GameState.TeamBVictory:
          break;
    		default:
    			Debug.Log("State Does Not Exist");
    			break;
    	}
        OnGameStateChanged?.Invoke(newState);
    }


  private void HandlePreGame(){
    TeamARebuttalSkulls.SetActive(false);
    TeamBRebuttalSkulls.SetActive(false);
    TeamBBlocks.SetActive(false);
    TeamABlocks.SetActive(false);
  }


	private void HandleTeamATurn(){
    TeamBBlocks.SetActive(false);
    TeamABlocks.SetActive(true);
	}

	private void HandleTeamBTurn(){
    TeamABlocks.SetActive(false);
    TeamBBlocks.SetActive(true);
	}

}

public enum GameState{
	PreGame,
	TeamATurn,
	TeamBTurn,
	TeamASkullTurn,
	TeamBSkullTurn,
	TeamAVictory,
  TeamBVictory
}
