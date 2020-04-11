using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMax : MonoBehaviour
{
    private List<string> moves;
    public bool IsActiveWhite;
    public bool IsActiveBlack;
    public bool IsSinglePlayerWhite;
    public bool IsSinglePlayerBlack;
    public int WhiteSertchDebth;
    public int BlackSertchDebth;
    int test = 0;
    bool isPlayingWhite;
    bool isPlayingBlack;

    // Start is called before the first frame update
    void Start()
    {

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
        Board_Manager.Instance.isMinimaxing = true;
        int[] Score = new int[allMoves.Count];
        int OptimalIndex = -1;
        string Optimal;
        List<string> PosibleMoves = new List<string>();

        for (int i = 0; i < Score.Length; i++)
        {
            print("brance: " + i);
            Score[i] = MiniMaxing(allMoves[i], !Maxing, searchDepth, isWhite);
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
        print(test);
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

            if (Maxing)
            {
                print("white - " + Board_Manager.Instance.isWhiteTurn + " is maxing");
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
                print("white - " + Board_Manager.Instance.isWhiteTurn + " is mining");
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


/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMax : MonoBehaviour
{
    private List<string> moves;
    public bool IsActiveWhite;
    public bool IsActiveBlack;
    public bool IsSinglePlayerWhite;
    public bool IsSinglePlayerBlack;
    public bool IsWorseWhite;
    public bool IsWorseBlack;
    public int WhiteSertchDebth;
    public int BlackSertchDebth;
    int test = 0;
    bool IsWhitePlaying;
    bool IsBlackPlaying;

    // Update is called once per frame
    void Update()
    {
        if (!Board_Manager.Instance.GameEnded) 
        {
            if (IsActiveWhite && Board_Manager.Instance.isWhiteTurn)
            {
                moves = Board_Manager.Instance.AllMoves();
                IsWhitePlaying = true;
                if (IsWorseWhite)
                {
                    Board_Manager.Instance.MoveFromeString(Minimax(moves, false, WhiteSertchDebth, true));
                }
                else 
                {
                    Board_Manager.Instance.MoveFromeString(Minimax(moves, true, WhiteSertchDebth, true));
                }
                IsWhitePlaying = false;
            }
            if (IsActiveBlack && !Board_Manager.Instance.isWhiteTurn)
            {
                moves = Board_Manager.Instance.AllMoves();
                IsBlackPlaying = true;
                if (IsWorseBlack)
                {
                    Board_Manager.Instance.MoveFromeString(Minimax(moves, false, BlackSertchDebth, true));
                }
                else
                {
                    Board_Manager.Instance.MoveFromeString(Minimax(moves, true, BlackSertchDebth, true));
                }
                IsBlackPlaying = false;
            }
        }
    }

    public string Minimax(List<string> allMoves, bool Maxing, int searchDepth, bool isWhite)
    {
        Board_Manager.Instance.isMinimaxing = true;
        int[] Score = new int[allMoves.Count];
        int OptimalIndex = -1;
        string Optimal;
        List<string> PosibleMoves = new List<string>();

        for (int i = 0; i < Score.Length; i++)
        {
            Score[i] = MiniMaxing(allMoves[i], Maxing, searchDepth, isWhite);
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
        //print(test);
        //print(Optimal);
        return Optimal;
    }

    private int MiniMaxing(string Move, bool Maxing, int searchDepth, bool isWhite)
    {
        int Value;
        Board_Manager.Instance.MoveFromeString(Move);
        if (IsSinglePlayerWhite && IsWhitePlaying || IsSinglePlayerBlack && IsBlackPlaying) 
        {
            Board_Manager.Instance.isWhiteTurn = true;
        }
        if (IsWorseWhite && IsWhitePlaying || IsWorseBlack && IsBlackPlaying) 
        {
            Maxing = !Maxing;
        }
        List<string> allMoves = Board_Manager.Instance.AllMoves();
        int[] Score = new int[allMoves.Count];

        if (searchDepth > 0)
        {
            for (int i = 0; i < Score.Length; i++)
            {
                Score[i] = MiniMaxing(allMoves[i], !Maxing, searchDepth - 1, isWhite);
            }

            if (Maxing == true)
            {
                print(Board_Manager.Instance.isWhiteTurn + " isMaxing");
                Value = int.MinValue;
                for (int i = 0; i < Score.Length; i++)
                {
                    if (Score[i] > Value)
                    {
                        Value = Score[i];
                    }
                }
            }
            else if (Maxing == false)
            {
                print(Board_Manager.Instance.isWhiteTurn + " isMining");
                Value = int.MaxValue;
                for (int i = 0; i < Score.Length; i++)
                {
                    if (Score[i] < Value)
                    {
                        Value = Score[i];
                    }
                }
            }
            else 
            {
                print(Maxing);
                print("at rong value place");
                Value = 0;
            }
        }
        else
        {
            Value = FindBoardValue(isWhite);
        }

        Board_Manager.Instance.ReversMove();
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
                    Value += 1000;
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
*/
