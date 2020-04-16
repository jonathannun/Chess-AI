using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board_Manager : MonoBehaviour {
    public static Board_Manager Instance { set; get; }
    private bool[,] allowedMoves { set; get; }
    public List<string> MoveHistory;
    public string[] FixedMoveHistory;

    public Pices[,] piceses { set; get; }
    private Pices selectedPices;

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionX = -1;
    private int selectionY = -1;
    public int[] enPassant { set; get; }
    private bool enPassantMade;
    public bool MiniMaxing; //to perevnt game from promoting ande enpassant if minimax is active

    public GameObject Board;
    public List<GameObject> chessPicesPrefabs;
    public List<GameObject> activeChessPices;
    public string[,] XKoordiantes = new string[8, 2];

    private Quaternion piceOrientationBlack = Quaternion.Euler(-90,90,0);
    private Quaternion piceOrientationWhite = Quaternion.Euler(-90, -90, 0);
    private Vector3 BoardPosition = Vector3.zero;

    public bool isWhiteTurn = true;
    public bool HvidRokade;
    public bool SortRokade;
    private bool whiteCheck;
    private bool blackCheck;

    public bool isMinimaxing = false; //to prevent game from stopping while minimaxing
    public bool GameEnded = false;

    private void Start()
    {
        Instance = this; //making data from this avalablee
        BoardPosition.x += 4; //setting Board Prefab position x
        BoardPosition.z += 4; //setting Board Prefab position x
        Instantiate(Board,BoardPosition, piceOrientationBlack); //instanciating Board Prefab
        SpawnAllCehssPices(); //Instanciating all pieces
        MakeXKoordiantesTable(); //Making X Table
        for (int i = 0; i < activeChessPices.Count; i++) 
        {
            activeChessPices[i].GetComponent<Pices>().PossibleMove();
        } //intialicing all posible moves in order for the king to see
    } //initialising data

    private void Update()
    {
        UodateSelectionr(); //updating mouse position
        if (Input.GetMouseButtonDown(0)) 
        {
            if (selectionX >= 0 && selectionY >= 0) //if the mouse position is inside the board
            {
                //selecting or moving piece
                if (selectedPices == null) 
                {
                    SelectPices(selectionX,selectionY);
                }
                else
                {
                    MovePices(selectionX, selectionY);
                }
            }
        } //when mouse clicked
        if (Input.GetKeyDown("p"))
        {
            List<string> Moveses = AllMoves();
            for (int i = 0; i < Moveses.Count; i++)
            {
                print(Moveses[i]);
            }
        } //find all moves
        if (Input.GetKeyDown("r")) 
        {
            ReversMove();
        } //reverse button
        if (Input.GetKeyDown("n"))
        {
          Pices c = selectedPices.GetComponent<Pices>();
          print(c.CurrentX + ":" + c.CurrentY);  
        } //Selected pice letter
        if (Input.GetKeyDown("h"))
        {
            foreach (string m in MoveHistory) 
            {
                print(m);
            }
        } //Print history
        if (Input.GetKeyDown("l"))
        {
            print(MoveHistory.Count);
        }
    } //gameloop

    private void SelectPices(int x, int y)
    {
        bool hasPosibleMoves = false; //used to not selecting
        if (piceses[x, y] == null) //if there are no piece where cliked then nothing
        {
            return;
        }

        if (piceses[x, y].isWhite != isWhiteTurn) //if the pice are on oponent team do nothing
        {
            return;
        }

        allowedMoves = piceses[x, y].PossibleMove(); //pulling posible moves from piece
        //if there are no moves dount select the pice
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (allowedMoves[i, j])
                {
                    hasPosibleMoves = true;
                    break;
                }
            }
            if (hasPosibleMoves)
            {
                break;
            }
        } //testing all places for posible move
        if (!hasPosibleMoves)
        {
            return;
        } //if the peace has no move do nothing
        selectedPices = piceses[x, y]; //selecting piece
        BoardHeighlights.Instance.HighlightAllowedMoves (allowedMoves); //heighlighting allowed moves
    }  //Selecting or reselecting piece

    private void MovePices(int x, int y)
    {
        enPassantMade = false;

        if (allowedMoves[x,y])
        {
            Pices c = piceses[x, y];

            if (c != null && c.isWhite != isWhiteTurn)
            {
                //Removing pices from game
                activeChessPices.Remove(c.gameObject);
                Destroy(c.gameObject);

                if (c.GetType() == typeof(Konge))
                {
                    if (!isMinimaxing) 
                    {
                        //win and end game
                        GameEnded = true;
                        EndGame(true);
                        return;
                    }
                }
            } //if there is an oponnent piece on place kill it 

            if (x == enPassant[0] && y == enPassant[1])
            {
                if (isWhiteTurn)
                {
                   c = piceses[x, y - 1];
                }
                else
                {
                  c = piceses[x, y + 1];
                }
                activeChessPices.Remove(c.gameObject);
                Destroy(c.gameObject);
                enPassantMade = true;
            }//destry if enPassan was made
            enPassant[0] = -1;
            enPassant[1] = -1;

            if (!MiniMaxing) {
                if (selectedPices.GetType() == typeof(Bunde))
                {
                    if (y == 7)
                    {
                        activeChessPices.Remove(selectedPices.gameObject); //removing pawn from active piceses
                        Destroy(selectedPices.gameObject); //removing pawn from game
                        SpawnChessPices(1,x,y,piceOrientationWhite); //Spawning rook in game
                        selectedPices = piceses[x, y]; //selecting rook
                    } //white promotion

                    if (y == 0)
                    {
                        activeChessPices.Remove(selectedPices.gameObject);
                        Destroy(selectedPices.gameObject);
                        SpawnChessPices(7, x, y, piceOrientationBlack);
                        selectedPices = piceses[x, y];
                    } //black promotion

                    if (selectedPices.CurrentY == 1 && y == 3) 
                    {
                        enPassant[0] = x;
                        enPassant[1] = y - 1;
                    } //saving pawn move for enPassant
                    else if (selectedPices.CurrentY == 6 && y == 4) 
                    {
                        enPassant[0] = x;
                        enPassant[1] = y + 1;
                    } //saving pawn move for enPassant
                } //Promotion
            }
            if (selectedPices.GetType() == typeof(Konge) && !selectedPices.HasMoved) //Rokade
            {
                if (x == 2 && y == 0) //white long Rokade
                {
                    MoveHistory.Add(MakeMoveString(selectedPices,x,y));
                    //Moving king
                    piceses[selectedPices.CurrentX, selectedPices.CurrentY] = null;
                    selectedPices.transform.position = GetTileCenter(x, y);
                    selectedPices.SetPosition(x, y);
                    piceses[x, y] = selectedPices;
                    selectedPices.HasMoved = true;
                    //Moving Rook
                    selectedPices = piceses[0, 0];

                    piceses[selectedPices.CurrentX, selectedPices.CurrentY] = null;
                    selectedPices.transform.position = GetTileCenter(3, 0);
                    selectedPices.SetPosition(3, 0);
                    piceses[3, 0] = selectedPices;
                    selectedPices.HasMoved = true;
                    isWhiteTurn = !isWhiteTurn;
                    BoardHeighlights.Instance.HideHighlights();
                    selectedPices = null;
                    
                    return;
                }
                else if (x == 6 && y == 0) //white short Rokade
                {
                    MoveHistory.Add(MakeMoveString(selectedPices, x, y));
                    //Moving king
                    piceses[selectedPices.CurrentX, selectedPices.CurrentY] = null;
                    selectedPices.transform.position = GetTileCenter(x, y);
                    selectedPices.SetPosition(x, y);
                    piceses[x, y] = selectedPices;
                    selectedPices.HasMoved = true;
                    //Moving Rook
                    selectedPices = piceses[7, 0];

                    piceses[selectedPices.CurrentX, selectedPices.CurrentY] = null;
                    selectedPices.transform.position = GetTileCenter(5, 0);
                    selectedPices.SetPosition(5, 0);
                    piceses[5, 0] = selectedPices;
                    selectedPices.HasMoved = true;
                    isWhiteTurn = !isWhiteTurn;
                    BoardHeighlights.Instance.HideHighlights();
                    selectedPices = null;
                    return;
                }
                else if (x == 6 && y == 7) //black short Rokade
                {
                    MoveHistory.Add(MakeMoveString(selectedPices, x, y));
                    //Moving king
                    piceses[selectedPices.CurrentX, selectedPices.CurrentY] = null;
                    selectedPices.transform.position = GetTileCenter(x, y);
                    selectedPices.SetPosition(x, y);
                    piceses[x, y] = selectedPices;
                    selectedPices.HasMoved = true;
                    //Moving Rook
                    selectedPices = piceses[7, 7];

                    piceses[selectedPices.CurrentX, selectedPices.CurrentY] = null;
                    selectedPices.transform.position = GetTileCenter(5, 7);
                    selectedPices.SetPosition(5, 7);
                    piceses[5, 7] = selectedPices;
                    selectedPices.HasMoved = true;
                    isWhiteTurn = !isWhiteTurn;
                    BoardHeighlights.Instance.HideHighlights();
                    selectedPices = null;
                    return;
                }
                else if (x == 2 && y == 7) //black long Rokade
                {
                    MoveHistory.Add(MakeMoveString(selectedPices, x, y));
                    //Moving king
                    piceses[selectedPices.CurrentX, selectedPices.CurrentY] = null;
                    selectedPices.transform.position = GetTileCenter(x, y);
                    selectedPices.SetPosition(x, y);
                    piceses[x, y] = selectedPices;
                    selectedPices.HasMoved = true;
                    //Moving Rook
                    selectedPices = piceses[0, 7];

                    piceses[selectedPices.CurrentX, selectedPices.CurrentY] = null;
                    selectedPices.transform.position = GetTileCenter(3, 7);
                    selectedPices.SetPosition(3, 7);
                    piceses[3, 7] = selectedPices;
                    selectedPices.HasMoved = true;
                    isWhiteTurn = !isWhiteTurn;
                    BoardHeighlights.Instance.HideHighlights();
                    selectedPices = null;
                    return;
                }


            } //Rokade

            //moving piece and chaning turn 
            MoveHistory.Add(MakeMoveString(selectedPices, x, y));
            piceses [selectedPices.CurrentX, selectedPices.CurrentY] = null;
            selectedPices.transform.position = GetTileCenter(x, y);
            selectedPices.SetPosition (x, y);
            piceses [x, y] = selectedPices;
            selectedPices.HasMoved = true;
            

            //did pice make a check
            if (selectedPices.isWhite)
            {
                if (selectedPices.Check() != blackCheck)
                {
                    blackCheck = !blackCheck;
                }
            }
            else
            {
                if (selectedPices.Check() != whiteCheck)
                {
                    whiteCheck = !whiteCheck;
                }
            }
            isWhiteTurn = !isWhiteTurn;
        }

        
        BoardHeighlights.Instance.HideHighlights();
        selectedPices = null;
    } //Moving Pice

    private void UodateSelectionr()
    {
        if (!Camera.main) //if wee are looking throught a camera
        {
            return;
        }

        RaycastHit hit; //defining hit
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ChessPlane"))) //sending out hit
        {
            selectionX = (int)hit.point.x; //saving position
            selectionY = (int)hit.point.z; //saving postion
        }
        else //if the position is outside the boards
        {
            selectionX = -1; //resetting saved position
            selectionY = -1; //resetting saved position 
        }
    }  //saving and finding mouse position

    private void SpawnChessPices(int index ,int x, int y, Quaternion orientation) {
        GameObject temp = Instantiate(chessPicesPrefabs[index], GetTileCenter(x,y), orientation) as GameObject; //instanciating piece prefab
        temp.transform.SetParent (transform); //setting Board GameObject as parent 
        piceses[x, y] = temp.GetComponent<Pices> (); //adding pice instance to piceses array at position
        piceses[x, y].SetPosition (x,y); //updating piece local position
        activeChessPices.Add (temp); //adding piece to the active list
    } //Spawning one piece in the scene

    private Vector3 GetTileCenter(int x, int y){
        Vector3 origin = Vector3.zero; //initializing 3d vector position
        origin.x += (TILE_SIZE * x) + TILE_OFFSET; //setting x koordinat
        origin.z += (TILE_SIZE* y) + TILE_OFFSET; //setting y koordinat
        return origin;
    } //getiing tile center position

    private void SpawnAllCehssPices(){
        MoveHistory = new List<string>();
        activeChessPices = new List<GameObject>();
        piceses = new Pices[8,8];
        enPassant = new int[2] {-1,-1};

        //--------Spawn white team
        //konge
        SpawnChessPices(0, 4, 0, piceOrientationWhite);

        //Droning
        SpawnChessPices(1, 3, 0, piceOrientationWhite);

        //Tårn
        SpawnChessPices(2, 0, 0, piceOrientationWhite);
        SpawnChessPices(2, 7, 0, piceOrientationWhite);

        //Løber
        SpawnChessPices(3, 2, 0, piceOrientationWhite);
        SpawnChessPices(3, 5, 0, piceOrientationWhite);

        //Hest
        SpawnChessPices(4, 1, 0, piceOrientationWhite);
        SpawnChessPices(4, 6, 0, piceOrientationWhite);

        //Bunde
        for (int i = 0; i < 8; i++) {
            SpawnChessPices(5, i, 1, piceOrientationWhite);
        }

        //--------Spawn black team
        //konge
        SpawnChessPices(6, 4, 7, piceOrientationBlack);

        //Droning
        SpawnChessPices(7, 3, 7, piceOrientationBlack);

        //Tårn
        SpawnChessPices(8, 0, 7, piceOrientationBlack);
        SpawnChessPices(8, 7, 7, piceOrientationBlack);

        //Løber
        SpawnChessPices(9, 2, 7, piceOrientationBlack);
        SpawnChessPices(9, 5, 7, piceOrientationBlack);

        //Hest
        SpawnChessPices(10, 1, 7, piceOrientationBlack);
        SpawnChessPices(10, 6, 7, piceOrientationBlack);

        //Bunde
        for (int i = 0; i < 8; i++)
        {
            SpawnChessPices(11, i, 6, piceOrientationBlack);
        }

    } //Spawining all piceses at the begining

    private void MakeXKoordiantesTable()
    {
        for (int i = 0; i < 8; i++)
        {
            XKoordiantes[i, 0] = ((char)(97 + i)).ToString();
            XKoordiantes[i, 1] = i.ToString();
        }
    }

    private string MakeMoveString(Pices c, int x, int y)
    {
        if (c.PicesLetter == 'K')
        {
            if (c.CurrentX == 4 && c.CurrentY == 0 && c.HasMoved == false && x == 2 && y == 0)
            {
                return "O-O-O-";
            }
            if (c.CurrentX == 4 && c.CurrentY == 7 && c.HasMoved == false && x == 2 && y == 7)
            {
                return "O-O-O+";
            }
            if (c.CurrentX == 4 && c.CurrentY == 0 && c.HasMoved == false && x == 6 && y == 0)
            {
                return "O-O---";
            }
            if (c.CurrentX == 4 && c.CurrentY == 7 && c.HasMoved == false && x == 6 && y == 7)
            {
                return "O-O+++";
            }
        }
        
        if (c != null)
        {
            string move = ""; //initialising string
            move = move + c.PicesLetter.ToString();
            move = move + XKoordiantes[c.CurrentX, 0]; //adding start x koordianl letter to string
            move = move + (c.CurrentY+1).ToString(); //adding start y koordinat to string
            move = move + XKoordiantes[x, 0]; //adding x koordianl letter to string
            move = move + (y+1).ToString(); //adding y koordinat to string
            if (piceses[x, y] != null)
            {
                move = move + piceses[x, y].PicesLetter;
            }
            else if (enPassantMade) 
            {
                move = move + "P";
            }
            else
            {
                move = move + "-";
            }
            return move;
        }
        else
        {
            return "A mistake has been made: The pice used in MakeMoveString did not exist (handmade note)";
        }
    }

    public void MoveFromeString(string move)
        {
            int x = -1;
            int y = -1;
        
            if (move == "O-O---")
            {
                if (isWhiteTurn)
                {
                    BoardSelection(4, 0);
                    MovePices(6, 0);
                    return;
                } else { print("Rokade not possible"); }
            }
            else if (move == "O-O+++") 
            {
                if (!isWhiteTurn)
                {
                    BoardSelection(4, 7);
                    MovePices(6, 7);
                    return;
                } else {print("Rokade not possible");}
            }
            else if (move == "O-O-O-")
            {
                if (isWhiteTurn)
                {
                    BoardSelection(4, 0);
                    MovePices(2, 0);
                    return;
                } else { print("Rokade not possible"); }
            }
            else if (move == "O-O-O+") 
            {
                if (!isWhiteTurn)
                {
                    BoardSelection(4, 7);
                    MovePices(2, 7);
                    return;
                } else { print("Rokade not possible"); }
            }

            else if (move.Length == 6)
            {
                y = int.Parse(move.Substring(2, 1)) - 1;

                for (int j = 0; j < 8; j++)
                {
                    if (move.Substring(1, 1) == XKoordiantes[j, 0])
                    {
                        x = int.Parse(XKoordiantes[j, 1]);
                    }
                }

                for (int i = 0; i < activeChessPices.Count; i++)
                {
                    Pices c = activeChessPices[i].GetComponent<Pices>();
                    if (c.PicesLetter.ToString() == move.Substring(0, 1) || c.PicesLetter.ToString() == "F") //F is when the revers EnPassent is made and the pawn letter hasent been initiatet yet
                    {
                        if (c.CurrentX == x && c.CurrentY == y)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                if (move.Substring(3, 1) == XKoordiantes[j, 0])
                                {
                                    x = int.Parse(XKoordiantes[j, 1]);
                                }
                            }
                            y = int.Parse(move.Substring(4, 1)) - 1;
                            BoardSelection(c.CurrentX, c.CurrentY);
                            MovePices(x, y);
                            return;
                        }
                    }
                }
            } //if a non pownd moves with more than one pice posability
        }

    public void ReversMove()
    {
        EndGame(false);
        for (int i = 0; i < FixedMoveHistory.Length - 1; i++) 
        {
            MoveFromeString(FixedMoveHistory[i]);
        }
    } //Revers last move in history

    private void BoardSelection(int x, int y)
    {
        if (piceses[x, y] != null)
        {
            if (piceses[x, y].isWhite == isWhiteTurn)
            {
                BoardHeighlights.Instance.HideHighlights();
                SelectPices(x, y);
            }
        }
    }

    public List<string> AllMoves()
    {
        List<string> AllMoves = new List<string>();
        for (int i = 0; i < activeChessPices.Count; i++)
        {
            Pices c = activeChessPices[i].GetComponent<Pices>();
            if (c.isWhite == isWhiteTurn)
            {
                for (int k = 0; k < 8; k++)
                {
                    for (int n = 0; n < 8; n++)
                    {
                        if (c.PossibleMove()[k, n])
                        {
                            AllMoves.Add(MakeMoveString(c, k, n));
                        }
                    }
                }
            }  
        }
        return AllMoves;
    }

    public void EndGame(bool clearHistory)
    {
        //printing wining team
        if (clearHistory) 
        {
            if (isWhiteTurn)
            {
                print("white wins");
            }
            else
            {
                print("black wins");
            }
        }

        if (!GameEnded)
        {
            foreach (GameObject go in activeChessPices) //removing all pices from scene
            {
                Destroy(go);
            }
        }
        
        
        if (!clearHistory) 
        {
            FixedMoveHistory = MoveHistory.ToArray();
        }

        if (!GameEnded)
        {
            isWhiteTurn = true; //reseting turn
            BoardHeighlights.Instance.HideHighlights(); //hiding all heighlights
            SpawnAllCehssPices(); //respawning pieses in start position
        }
    } //Ending and resetting game
}
