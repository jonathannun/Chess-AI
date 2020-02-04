using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tårn : Pices {
    private void Start()
    {
        PicesLetter = 'R';
    }

    public override bool[,] PossibleMove()
    {
        bool[,] moves = new bool[8, 8];

        Pices c;
        int i;

        //Right
        i = CurrentX;
        while (true)
        {
            i++;
            if (i >= 8)
            {
                break;
            }

            c = Board_Manager.Instance.piceses[i, CurrentY];
            if (c == null)
            {
                moves[i, CurrentY] = true;
            }
            else
            {
                if (c.isWhite != isWhite)
                {
                    moves[i,CurrentY] = true;
                }

                break;
            }
        }

        //Left
        i = CurrentX;
        while (true)
        {
            i--;
            if (i < 0)
            {
                break;
            }

            c = Board_Manager.Instance.piceses[i, CurrentY];
            if (c == null)
            {
                moves[i, CurrentY] = true;
            }
            else
            {
                if (c.isWhite != isWhite)
                {
                    moves[i, CurrentY] = true;
                }

                break;
            }
        }

        //foardward
        i = CurrentY;
        while (true)
        {
            i++;
            if (i >= 8)
            {
                break;
            }

            c = Board_Manager.Instance.piceses[CurrentX, i];
            if (c == null)
            {
                moves[CurrentX, i] = true;
            }
            else
            {
                if (c.isWhite != isWhite)
                {
                    moves[CurrentX, i] = true;
                }

                break;
            }
        }

        //Backwards
        i = CurrentY;
        while (true)
        {
            i--;
            if (i < 0)
            {
                break;
            }

            c = Board_Manager.Instance.piceses[CurrentX, i];
            if (c == null)
            {
                moves[CurrentX, i] = true;
            }
            else
            {
                if (c.isWhite != isWhite)
                {
                    moves[CurrentX, i] = true;
                }

                break;
            }
        }
        oldMoves = moves;
        return moves;
    }
}
