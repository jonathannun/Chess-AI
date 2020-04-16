using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMax : MonoBehaviour
{
    private List<string> moves;
    public bool IsActiveWhite;
    public bool IsActiveBlack;
    public bool IsWorseBlack;
    public bool IsWorseWhite;
    public int WhiteSertchDebth;
    public int BlackSertchDebth;
    int test = 0;
    bool isPlayingWhite;
    bool isPlayingBlack;

    // Start is called before the first frame update
    void Start()
    {
        if (IsActiveWhite || IsActiveBlack) 
        {
            Board_Manager.Instance.MiniMaxing = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Board_Manager.Instance.GameEnded)
        {
            if (IsActiveWhite && Board_Manager.Instance.isWhiteTurn)
            {
                moves = Board_Manager.Instance.AllMoves();
                IsActiveWhite = true;
                Board_Manager.Instance.MoveFromeString(Minimax(moves, true, WhiteSertchDebth, true));
                IsActiveWhite = false;
            }
            if (IsActiveBlack && !Board_Manager.Instance.isWhiteTurn)
            {
                moves = Board_Manager.Instance.AllMoves();
                isPlayingBlack = true;
                Board_Manager.Instance.MoveFromeString(Minimax(moves, true, BlackSertchDebth, false));
                isPlayingBlack = false;
            }
        }
    }

    public string Minimax(List<string> allMoves, bool Maxing, int searchDepth, bool isWhite)
    {
        test = 0;
        Board_Manager.Instance.isMinimaxing = true;
        int[] Score = new int[allMoves.Count];
        int OptimalIndex = -1;
        string Optimal;
        List<string> PosibleMoves = new List<string>();

        for (int i = 0; i < Score.Length; i++)
        {
            Score[i] = MiniMaxing(allMoves[i], !Maxing, searchDepth, isWhite);
        }

        if (IsActiveBlack && IsWorseBlack || IsActiveWhite && IsWorseWhite) 
        {
            Maxing = false;
        }

        if (Maxing)
        {
            int OptimalValue = int.MinValue;
            for (int i = 0; i < Score.Length; i++)
            {
                if (Score[i] > OptimalValue)
                {
                    OptimalValue = Score[i];
                }
            }
            for (int i = 0; i < Score.Length; i++)
            {
                if (Score[i] == OptimalValue)
                {
                    PosibleMoves.Add(allMoves[i]);
                }
            }
            OptimalIndex = (int)Random.Range(0, PosibleMoves.Count);
        }
        else
        {
            int OptimalValue = int.MaxValue;
            for (int i = 0; i < Score.Length; i++)
            {
                if (Score[i] < OptimalValue)
                {
                    OptimalValue = Score[i];
                }
            }
            for (int i = 0; i < Score.Length; i++)
            {
                if (Score[i] == OptimalValue)
                {
                    PosibleMoves.Add(allMoves[i]);
                }
            }
            OptimalIndex = (int)Random.Range(0, PosibleMoves.Count);
        }
        Optimal = PosibleMoves[OptimalIndex];

        Board_Manager.Instance.isMinimaxing = false;
        print("Evaluations: " + test);
        return Optimal;
    }

    private int MiniMaxing(string Move, bool Maxing, int searchDepth, bool isWhite)
    {
        int Value;
        /*if (isPlayingWhite && IsSinglePlayerWhite || isPlayingBlack && IsSinglePlayerBlack)
        {
            print("here");
            Maxing = !Maxing;
            Board_Manager.Instance.isWhiteTurn = isWhite;
            print(Board_Manager.Instance.isWhiteTurn);
        }*/
        Board_Manager.Instance.MoveFromeString(Move);
        List<string> allMoves = Board_Manager.Instance.AllMoves();
        int[] Score = new int[allMoves.Count];
        

        if (searchDepth > 0)
        {
            for (int i = 0; i < Score.Length; i++)
            {
                Score[i] = MiniMaxing(allMoves[i], !Maxing, searchDepth - 1, isWhite);
            }

            if (IsActiveBlack && IsWorseBlack || IsActiveWhite && IsWorseWhite)
            {
                Maxing = false;
            }
            
            if (Maxing)
            {
                Value = int.MinValue;
                for (int i = 0; i < Score.Length; i++)
                {
                    if (Score[i] > Value)
                    {
                        Value = Score[i];
                    }
                }
            }
            else
            {
                Value = int.MaxValue;
                for (int i = 0; i < Score.Length; i++)
                {
                    if (Score[i] < Value)
                    {
                        Value = Score[i];
                    }
                }
            }
        }
        else
        {
            Value = FindBoardValue(isWhite);
        }
        Board_Manager.Instance.ReversMove();
        /*if (isPlayingWhite && IsSinglePlayerWhite || isPlayingBlack && IsSinglePlayerBlack)
        {
            Board_Manager.Instance.isWhiteTurn = isWhite;
        }*/
        return Value;
    }

    private int FindBoardValue(bool isWhite)
    {
        List<GameObject> activeChessPices = Board_Manager.Instance.activeChessPices;
        int Value = 0;
        foreach (GameObject p in activeChessPices)
        {
            Pices c = p.GetComponent<Pices>();
            if (c.isWhite == isWhite)
            {
                if (c.GetType() == typeof(Konge))
                {
                    Value += 20;
                }
                else if (c.GetType() == typeof(Droning))
                {
                    Value += 9;
                }
                else if (c.GetType() == typeof(Tårn))
                {
                    Value += 5;
                }
                else if (c.GetType() == typeof(Løber))
                {
                    Value += 3;
                }
                else if (c.GetType() == typeof(Springer))
                {
                    Value += 3;
                }
                else if (c.GetType() == typeof(Bunde))
                {
                    Value += 1;
                }
                else if (c.PicesLetter == 'F')
                {
                    //print("Somthings Wrong, There was an _F_ in Minimax Value System");
                }
            }
            if (c.isWhite == !isWhite)
            {
                if (c.GetType() == typeof(Konge))
                {
                    Value -= 1000;
                }
                else if (c.GetType() == typeof(Droning))
                {
                    Value -= 9;
                }
                else if (c.GetType() == typeof(Tårn))
                {
                    Value -= 5;
                }
                else if (c.GetType() == typeof(Løber))
                {
                    Value -= 3;
                }
                else if (c.GetType() == typeof(Springer))
                {
                    Value -= 3;
                }
                else if (c.GetType() == typeof(Bunde))
                {
                    Value -= 1;
                }
                else if (c.PicesLetter == 'F')
                {
                    //print("Somthings Wrong, There was an _F_ in Minimax Value System");
                }
            }
        }
        test += 1;
        return Value;
    }
}
