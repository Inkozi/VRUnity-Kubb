using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

	//private Dictionary<string, List<GameObject>> teamPiecesA;
	//private Dictionary<string, List<GameObject>> teamPiecesB;

  public GameObject BlockParentA;
  public GameObject BlockParentB;

  private GameObject king;

	private List<GameObject> skullsA = new List<GameObject>();
	private List<GameObject> skullsB = new List<GameObject>();
  private List<Vector3> skullsA_origpos = new List<Vector3>();
  private List<Vector3> skullsB_origpos = new List<Vector3>();

	private List<GameObject> skullsAA = new List<GameObject>();
	private List<GameObject> skullsBB = new List<GameObject>();
  private List<Vector3> skullsAA_origpos = new List<Vector3>();
  private List<Vector3> skullsBB_origpos = new List<Vector3>();

	private List<GameObject> woodBlocksA = new List<GameObject>();
	private List<GameObject> woodBlocksB = new List<GameObject>();
  private List<Vector3> woodBlocksA_origpos = new List<Vector3>();
  private List<Vector3> woodBlocksB_origpos = new List<Vector3>();

	private const int numTeams = 2;
	private const int numSkulls = 5;
	private const int numBlocks = 6;

  private int origASkulls = 5;
  private int origBSkulls = 5;

	void Awake(){
		GameManager.OnGameStateChanged += gameStateChanged;
	}

	void OnDestroy(){
		GameManager.OnGameStateChanged -= gameStateChanged;
	}

	private void gameStateChanged(GameState state){

	}

    // Start is called before the first frame update
    void Start()
    {

      king = GameObject.Find("King");
    	//populate gameobjects
    	PopulatePieces("A", numSkulls, "Skull", skullsA);
    	PopulatePieces("B", numSkulls, "Skull", skullsB);
    	//PopulatePieces("AA", numSkulls, "Skull", skullsAA);
    	//PopulatePieces("BB", numSkulls, "SKull", skullsBB);
    	PopulatePieces("A", numBlocks, "WoodBlock", woodBlocksA);
    	PopulatePieces("B", numBlocks, "WoodBlock", woodBlocksB);

      //save original positions
      OriginalPositions(skullsA, skullsA_origpos);
      OriginalPositions(skullsB, skullsB_origpos);
      //OriginalPositions(skullsAA, skullsAA_origpos);
      //OriginalPositions(skullsB, skullsBB_origpos);
      OriginalPositions(woodBlocksA, woodBlocksA_origpos);
      OriginalPositions(woodBlocksB, woodBlocksB_origpos);

    }

    //Save Original Positions
    private void OriginalPositions(List<GameObject> items, List<Vector3> pos){
      for (int item = 0; item < items.Count; item++){
        pos.Add(new Vector3(items[item].transform.position.x, items[item].transform.position.y, items[item].transform.position.z));
      }
    }

    private void RestorePositions(List<GameObject> items, List<Vector3> pos){
      Debug.Log("Restoring Positions");
      for (int item = 0; item < items.Count; item++){
        items[item].transform.position = pos[item];
      } 
    }

    //Populate
    private void PopulatePieces(string team, int numPieces, string item, List<GameObject> pieces){
      Debug.Log("Populating Pieces");
    	for (int piece = 0; piece < numPieces; piece++){
    		pieces.Add(GameObject.Find(item + "_" + (piece + 1).ToString() + team));
    	}
    }

    //Reparent peices and this is meant for blocks
    private void ParentPeices(List<GameObject> pieces, GameObject parent){
      for (int piece = 0; piece < pieces.Count; piece++){
        pieces[piece].transform.SetParent(parent.transform, false);
      }
    }

    //Report position of anything you throw in this function;
    private void ReportPiecePosition(List<GameObject> pieces){
      for (int piece = 0; piece < pieces.Count; piece++){
        Debug.Log(pieces[piece].transform.position);
      }
    }

    //removing skulls from the game when they are destroyed
    private void RemovePiece(List<GameObject> pieces){
    	bool removal = false;
    	int index = 0;
      for (int piece = 0; piece < pieces.Count; piece++){
        if (pieces[piece] == null){
            removal = true;
            index = piece;
        }
      }
      if (removal){
        pieces.RemoveAt(index);
      }
    }


    private bool hasStopped(Rigidbody rb){
    	bool stopped = true;
    	if (rb.velocity.x != 0 || rb.velocity.y != 0 || rb.velocity.z != 0){
    		stopped = false;
    	}
    	return stopped;
    }



    private bool teamsTurn(List<GameObject> pieces, string team){
    	bool stillTurn = false;
    	GameObject plane = GameObject.Find("TeamAPlane");
    	if (team == "B"){
    		plane = GameObject.Find("TeamBPlane");
    	}
       

      //Debug.Log("first piece: " + pieces[0].transform.position);
      //Debug.Log("plane: " + plane.transform.position);
    	for (int piece = 0; piece < pieces.Count; piece++){
    		Rigidbody rb = pieces[piece].GetComponent<Rigidbody>();
    		if (team == "A"){
          //Debug.Log("piece: " +  (piece + 1).ToString() + " " + pieces[piece].transform.position.z);
    			if (pieces[piece].transform.position.x > plane.transform.position.x || 
              (pieces[piece].transform.position.x < plane.transform.position.x && !hasStopped(rb))){
    				stillTurn = true;
    			}
    		}
    		else{
    			if (pieces[piece].transform.position.x < plane.transform.position.x || 
              (pieces[piece].transform.position.x > plane.transform.position.x && !hasStopped(rb))){
    				stillTurn = true;
    			}
    		}
    	}
    	return stillTurn;
    }

    private void win(){
    	//win conditions
    	//if teamA's turn and (!king and skullsB.Count == 0) or (skullB.Count > 0 and !king)
      if (GameManager.Instance.State == GameState.TeamATurn && ((king == null && skullsB.Count == 0) || (skullsB.Count > 0 && king == null))){
        GameManager.Instance.UpdateGameState(GameState.TeamAVictory);      
      }
    	//if teamB's turn and (!king and skullsA.Count == 0) or (skullA.Count > 0 and !king)
      else if (GameManager.Instance.State == GameState.TeamBTurn && ((king == null && skullsA.Count == 0) || (skullsA.Count > 0 && king == null))){
        GameManager.Instance.UpdateGameState(GameState.TeamBVictory);      
      }
    }

    private void loop(){
    	//game flow
    	
      //if teamA's turn and !teamsTurn(woodBlocksA, "A")
      Debug.Log(GameManager.Instance.State);
    	if (GameManager.Instance.State == GameState.TeamATurn && !teamsTurn(woodBlocksA, "A")){
    		//if (skullsB < initialSkulls)
    	//	if (skullsB.Count < origBSkulls){
    			//invoke teamB's rebuttal
    			//GameManager.Instance.UpdateGameState(GameState.TeamBSkullTurn);
    	//	}
    	//	else{
    			//invoke teamB's turn
          origBSkulls = skullsB.Count;
          Debug.Log("Team B's Turn");
    			GameManager.Instance.UpdateGameState(GameState.TeamBTurn);
          //woodBlocksB.Clear();
    	    //PopulatePieces("B", numBlocks, "WoodBlock", woodBlocksB);
          RestorePositions(woodBlocksB, woodBlocksB_origpos);
    	//	}
    	}


    	//if teamB's turn and !teamsTurn(woodBlocksB, "B")
    	if (GameManager.Instance.State == GameState.TeamBTurn && !teamsTurn(woodBlocksB, "B")){
    		//if (skullsA < initialSkulls)
    		// if (skullsA.Count < origASkulls){
    			//invoke teamA's rebuttal
    			//GameManager.Instance.UpdateGameState(GameState.TeamASkullTurn);
       // }
       // else{
    			//invoke teamA's turn
          origASkulls = skullsA.Count; 
          Debug.Log("Team A's Turn");
    			GameManager.Instance.UpdateGameState(GameState.TeamATurn);
          //woodBlocksA.Clear();
    	    //PopulatePieces("A", numBlocks, "WoodBlock", woodBlocksA);
          RestorePositions(woodBlocksA, woodBlocksA_origpos);
        //}
      }

      /*
    	//if teamA's rebuttal and !teamsTurn(skullsAA, "A")
      if (GameManager.Instance.State == GameState.TeamASkullTurn && !teamsTurn(skullsAA, "A")){
          origBSkulls = skullsB.Count;
    			GameManager.Instance.UpdateGameState(GameState.TeamBTurn);
      }
    	//if teamB's rebuttal and !teamsTurn(skullsBB, "B")
      if (GameManager.Instance.State == GameState.TeamASkullTurn && !teamsTurn(skullsBB, "B")){
    			//invoke teamA's turn  
          origASkulls = skullsA.Count; 
    			GameManager.Instance.UpdateGameState(GameState.TeamATurn);
      }
      */
    }


    // Update is called once per frame
    void FixedUpdate()
    {
    	loop();

    	//Debug.Log(GameState.state);
      //UpdatePieces("A", "WoodBlock", woodBlocksA);
      //UpdatePieces("B", "WoodBlock", woodBlocksB);

      /*
    	RemovePiece(skullsA);
    	RemovePiece(skullsAA);
    	RemovePiece(skullsB);
    	
      RemovePiece(skullsBB);*/

    	//win();
    }
}
