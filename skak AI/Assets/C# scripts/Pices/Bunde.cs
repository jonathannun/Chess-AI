using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunde : Pices {
    string LastEnPassen;
    private void Start()
    {
        PicesLetter = 'P';
    }

    public override bool[,] PossibleMove()
    {
        bool[,] moves = new bool[8, 8];
        Pices c, c2;
        int[] e = Board_Manager.Instance.enPassant;

        // White Team
        if (isWhite)
        {
            //Diaganal Left
            if (CurrentX != 0 && CurrentY != 7)
            {
                if (e [0] == CurrentX - 1 && e [1] == CurrentY + 1)
                {
                    moves[CurrentX - 1, CurrentY + 1] = true;
                } //EnPassen

                c = Board_Manager.Instance.piceses [CurrentX - 1, CurrentY + 1];
                if (c != null && !c.isWhite)
                {
                    moves[CurrentX - 1, CurrentY + 1] = true;
                }
            }

            //Diaganal Right
            if (CurrentX != 7 && CurrentY != 7)
            {
                if (e[0] == CurrentX + 1 && e[1] == CurrentY + 1)
                {
                    moves[CurrentX + 1, CurrentY + 1] = true;
                } //EnPassen

                c = Board_Manager.Instance.piceses[CurrentX + 1, CurrentY + 1];
                if (c != null && !c.isWhite)
                {
                    moves[CurrentX + 1, CurrentY + 1] = true;
                }
            }

            //Forward
            if (CurrentY != 7)
            {
                c = Board_Manager.Instance.piceses[CurrentX, CurrentY + 1];
                if (c == null)
                {
                    moves[CurrentX, CurrentY + 1] = true;
                }
            }

            //Duble Foardward
            if (CurrentY == 1)
            {
                c = Board_Manager.Instance.piceses[CurrentX, CurrentY + 1];
                c2 = Board_Manager.Instance.piceses[CurrentX, CurrentY + 2];

                if (c == null && c2 == null)
                {
                    moves[CurrentX, CurrentY + 2] = true;
                }
            }

        }
        
        //Black Team 
        else
        {
            //Diaganal Left
            if (CurrentX != 0 && CurrentY != 0)
            {
                if (e[0] == CurrentX - 1 && e[1] == CurrentY - 1)
                {
                    moves[CurrentX - 1, CurrentY - 1] = true;
                } //EnPassen
                c = Board_Manager.Instance.piceses[CurrentX - 1, CurrentY - 1];
                if (c != null && c.isWhite)
                {
                    moves[CurrentX - 1, CurrentY - 1] = true;

                }
            }

            //Diaganal Right
            if (CurrentX != 7 && CurrentY != 0)
            {
                if (e[0] == CurrentX + 1 && e[1] == CurrentY - 1)
                {
                    moves[CurrentX + 1, CurrentY - 1] = true;
                } //EnPassen
                c = Board_Manager.Instance.piceses[CurrentX + 1, CurrentY - 1];
                if (c != null && c.isWhite)
                {
                    moves[CurrentX + 1, CurrentY - 1] = true;
                }
            }

            //Forward
            if (CurrentY != 0)
            {
                c = Board_Manager.Instance.piceses[CurrentX, CurrentY - 1];
                if (c == null)
                {
                    moves[CurrentX, CurrentY - 1] = true;
                }
            }

            //Duble Foardward
            if (CurrentY == 6)
            {
                c = Board_Manager.Instance.piceses[CurrentX, CurrentY - 1];
                c2 = Board_Manager.Instance.piceses[CurrentX, CurrentY - 2];

                if (c == null && c2 == null)
                {
                    moves[CurrentX, CurrentY - 2] = true;
                }
            }
        }

        oldMoves = moves;
        return moves;
    }
}
