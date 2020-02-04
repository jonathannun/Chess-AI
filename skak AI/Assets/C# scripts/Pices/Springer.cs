using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Springer : Pices {
    private void Start()
    {
        PicesLetter = 'N';
    }

    public override bool[,] PossibleMove()
    {
        bool[,] moves = new bool[8, 8];
        //Up Left
        KnightMove(CurrentX - 1, CurrentY + 2,ref moves);

        //Up Right
        KnightMove(CurrentX + 1, CurrentY + 2, ref moves);

        //Down Left
        KnightMove(CurrentX - 1, CurrentY - 2, ref moves);

        //Down Right
        KnightMove(CurrentX + 1, CurrentY - 2, ref moves);

        //Left Up
        KnightMove(CurrentX - 2, CurrentY + 1, ref moves);

        //Right Up
        KnightMove(CurrentX + 2, CurrentY + 1, ref moves);

        //Left Down
        KnightMove(CurrentX - 2, CurrentY - 1, ref moves);
    
        //Right Down
        KnightMove(CurrentX + 2, CurrentY - 1, ref moves);

        oldMoves = moves;
        return moves;
    }

    public void KnightMove(int x, int y, ref bool[,] Moves) 
    {
        Pices c;
        if (x >= 0 && x < 8 && y >= 0 && y < 8) //if inside board
        {
            c = Board_Manager.Instance.piceses[x, y];
            if (c == null) //if empty tile chosen
            {
                Moves[x, y] = true;
            }
            else if (isWhite != c.isWhite) //if oponent pieces on chosen tile
            {
                Moves[x, y] = true;
            }
        }
    } //easy macking knight logic
}
