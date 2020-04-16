using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingClutch : MonoBehaviour
{
    public bool IsActiveWhite;
    public bool IsActiveBlack;
    public bool YourKingsWhite;
    public bool YourKingsBlack;
    public bool AgressiveWhite;
    public bool AgressiveBlack;
    public bool WinOrientedWhite;
    public bool WinOrientedBlack;

    public int waightCound;
    List<string> moves;

    // Start is called before the first frame update
    void Start()
    {
        moves = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Board_Manager.Instance.GameEnded)
        {
            if (IsActiveWhite && Board_Manager.Instance.isWhiteTurn)
            {
                if (Time.frameCount % waightCound == 0)
                {
                    moves = Board_Manager.Instance.AllMoves();
                    Board_Manager.Instance.MoveFromeString(KingClutching(moves, YourKingsWhite, true));
                }
            }
            if (IsActiveBlack && !Board_Manager.Instance.isWhiteTurn)
            {
                if ((Time.frameCount + (waightCound / 2)) % waightCound == 0)
                {
                    moves = Board_Manager.Instance.AllMoves();
                    Board_Manager.Instance.MoveFromeString(KingClutching(moves, YourKingsBlack, false));
                }
            }
        }
    }

    private string KingClutching(List<string> Moves, bool yourKing, bool isWhite) 
    {
        float OptimalDiff = 0;
        int OptimalIndex;
        List<string> PossibleMoves = new List<string>();

        for (int i = 0; i < Moves.Count; i++) 
        {
            if (FindDiff(Moves[i], yourKing, isWhite) > OptimalDiff) 
            {
                OptimalDiff = FindDiff(Moves[i], yourKing, isWhite);
            }
        }

        if (OptimalDiff == 0) 
        {
            return RandomMove(Moves);
        }

        for (int i = 0; i < Moves.Count; i++) 
        {
            if (FindDiff(Moves[i], yourKing, isWhite) == OptimalDiff) 
            {
                PossibleMoves.Add(Moves[i]);
            }
        }
        OptimalIndex = (int)Random.Range(0, PossibleMoves.Count);

        return PossibleMoves[OptimalIndex];
    }

    private float FindDiff(string move, bool yourKing, bool isWhite) 
    {
        Pices c = Board_Manager.Instance.activeChessPices[0].GetComponent<Pices>();
        Pices check;
        float Diff;
        float nowX = 0;
        float nowY = 0;
        float movedX = 0;
        float movedY = 0;

        if (move == "O-O-O-" || move == "O-O-O+" || move == "O-O---" || move == "O-O+++") 
        {
            return 0;
        }

        foreach (GameObject p in Board_Manager.Instance.activeChessPices) 
        {
            check = p.GetComponent<Pices>();
            if (yourKing && isWhite && check.PicesLetter == 'K' && !check.isWhite || !yourKing && !isWhite && check.PicesLetter == 'K' && !check.isWhite)
            {
                c = check;
            }
            else if (!yourKing && isWhite && check.PicesLetter == 'K' && check.isWhite || yourKing && !isWhite && check.PicesLetter == 'K' && check.isWhite)
            {
                c = check;
            }
        }

        float kingX = (float)c.CurrentX;
        float kingY = (float)c.CurrentY;
        

        for (int j = 0; j < 8; j++)
        {
            if (move.Substring(1, 1) == Board_Manager.Instance.XKoordiantes[j, 0])
            {
                nowX = float.Parse(Board_Manager.Instance.XKoordiantes[j, 1]) + 1f;
            }
        }
        nowY = float.Parse(move.Substring(2, 1));

        for (int j = 0; j < 8; j++)
        {
            if (move.Substring(3, 1) == Board_Manager.Instance.XKoordiantes[j, 0])
            {
                movedX = float.Parse(Board_Manager.Instance.XKoordiantes[j, 1]) + 1f;
            }
        }
        movedY = float.Parse(move.Substring(4,1));

        Diff = Mathf.Sqrt(Mathf.Pow(nowX - kingX, 2f) + Mathf.Pow(nowY - kingY, 2f)) - Mathf.Sqrt(Mathf.Pow(movedX - kingX, 2f) + Mathf.Pow(movedY - kingY, 2f));

        if (move.Substring(5, 1) == "K" && WinOrientedWhite && isWhite || !isWhite && WinOrientedBlack && move.Substring(5, 1) == "K") 
        {
            return 100;
        }

        if (move.Substring(5, 1) != "-" && AgressiveBlack && !isWhite || move.Substring(5, 1) != "-" && AgressiveWhite && isWhite) 
        {
            return FindGain(move.Substring(5, 1));
        }


        if (Diff <= 0) 
        {
            return 0;
        }

        return Diff;
    }

    private string RandomMove(List<string> moveStrings)
    {
        int random = (int)Random.Range(0, moveStrings.Count);
        return moveStrings[random];
    }

    private float FindGain(string piece) 
    {
        if (piece == "K")
        {
            return 100;
        }
        else if (piece == "Q")
        {
            return 90;
        }
        else if (piece == "R")
        {
            return 50;
        }
        else if (piece == "B")
        {
            return 30;
        }
        else if (piece == "N")
        {
            return 30;
        }
        else if (piece == "P")
        {
            return 10;
        }
        else 
        {
            print("somthins wrong - In FindGain(KingClutch) but no pice letter found");
            return 0;
        }
    }
}
