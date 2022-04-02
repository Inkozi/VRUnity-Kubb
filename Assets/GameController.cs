using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

	//private Dictionary<string, List<GameObject>> teamPiecesA;
	//private Dictionary<string, List<GameObject>> teamPiecesB;

	private List<GameObject> skullsA = new List<GameObject>();
	private List<GameObject> skullsB = new List<GameObject>();

	private List<GameObject> skullsAA = new List<GameObject>();
	private List<GameObject> skullsBB = new List<GameObject>();

	private List<GameObject> woodBlocksA = new List<GameObject>();
	private List<GameObject> woodBlocksB = new List<GameObject>();

	private const int numTeams = 2;
	private const int numSkulls = 5;
	private const int numBlocks = 2;

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
    	//populate gameobjects
    	PopulatePieces("A", numSkulls, "Skull", skullsA);
    	PopulatePieces("B", numSkulls, "Skull", skullsB);
    	PopulatePieces("AA", numSkulls, "Skull", skullsAA);
    	PopulatePieces("BB", numSkulls, "SKull", skullsBB);
    	PopulatePieces("A", numBlocks, "Block", woodBlocksA);
    	PopulatePieces("B", numBlocks, "Block", woodBlocksB);
    }

    //Populate
    public void PopulatePieces(string team, int numPieces, string item, List<GameObject> pieces){
    	for (int piece = 0; piece < numPieces; piece++){
    		pieces.Add(GameObject.Find(item + "_" + (piece + 1).ToString() + team));
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
    	if (rb.velocity.x > 0 || rb.velocity.y > 0 || rb.velocity.z > 0){
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

    	for (int piece = 0; piece < pieces.Count; piece++){
    		Rigidbody rb = pieces[piece].GetComponent<Rigidbody>();
    		if (team == "A"){
    			if (pieces[piece].transform.position.z < plane.transform.position.z || 
    					!hasStopped(rb)){
    				stillTurn = true;
    			}
    		}
    		else{
    			if (pieces[piece].transform.position.z > plane.transform.position.z || 
    					!hasStopped(rb)){
    				stillTurn = true;
    			}
    		}
    	}
    	return stillTurn;
    }

    private void win(){
    	//win conditions
    	//if teamA's turn and (!king and skullsB.Count == 0) or (skullB.Count > 0 and !king)
    	//if (state == GameState.)
    		//invoke teamB's victory
    	//if teamB's turn and (!king and skullsA.Count == 0) or (skullA.Count > 0 and !king)
    		//invoke teamA's victory
    }

    private void loop(){
    	//game flow
    	
    	if (GameManager.Instance.State == GameState.TeamATurn && !teamsTurn(woodBlocksA, "A")){
    		if (skullsB < 5){
    			GameManager.Instance.UpdateGameState(GameState.TeamBSkullTurn);
    		}
    		else{
    			GameManager.Instance.UpdateGameState(GameState.TeamBTurn);
    		}
    	}
    	//if teamA's turn and !teamsTurn(woodBlocksA, "A")
    		//if (skullsB < initialSkulls)
    			//invoke teamB's rebuttal
    		//else
    			//invoke teamB's turn

    	//if teamB's turn and !teamsTurn(woodBlocksB, "B")
    		//if (skullsA < initialSkulls)
    			//invoke teamA's rebuttal
    		//else
    			//invoke teamA's turn  

    	//if teamA's rebuttal and !teamsTurn(skullsAA, "A")
    		//invoke teamB's turn

    	//if teamB's rebuttal and !teamsTurn(skullsBB, "B")
    		//invoke teamA's turn

    }


    // Update is called once per frame
    void FixedUpdate()
    {
    	loop();
    	//Debug.Log(GameState.state);

    	/*
    	RemovePiece(skullsA);
    	RemovePiece(skullsAA);
    	RemovePiece(skullsB);
    	RemovePiece(skullsBB);

    	win();
    	loop();*/
    }
}
