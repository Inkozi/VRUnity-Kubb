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
  public GameObject TeamASockets;

  public GameObject TeamBBlocks;
  public GameObject TeamBSkulls;
  public GameObject TeamBRebuttalSkulls;
  public GameObject TeamBSockets;

  private PieceManager pm;

  private bool gameStarted = false;

  private bool activated = false;

  void Awake() {
      Instance = this;

  }

  void Start()
  {
      pm = this.GetComponent<PieceManager>();
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
    pm.toggleSkulls('A', false, false);
    pm.toggleSkulls('B', false, false);
  }


  private void HandleTeamATurn(){
    pm.toggleObjects('B', false);
    pm.toggleObjects('A', true);
  }

  private void HandleTeamBTurn(){
    pm.toggleObjects('A', false);
    pm.toggleObjects('B', true);
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
