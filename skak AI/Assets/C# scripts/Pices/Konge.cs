using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Konge : Pices {
    private void Start()
    {
        PicesLetter = 'K';
    }

    public override bool[,] PossibleMove()
    {
        bool[,] moves = new bool[8, 8];
        Pices c;
        int i, j;

        //Up Left
        i = CurrentX - 1;
        j = CurrentY + 1;
        if (i >= 0 && j < 8)
        {
            c = Board_Manager.Instance.piceses[i, j];
            if (c == null || c.isWhite != isWhite)
            {
                moves[i, j] = true;
            }
        }

        //Up Right
        i = CurrentX + 1;
        j = CurrentY + 1;
        if (i < 8 && j < 8)
        {
            c = Board_Manager.Instance.piceses[i, j];
            if (c == null || c.isWhite != isWhite)
            {
                moves[i, j] = true;
            }
        }

        //Up
        i = CurrentX;
        j = CurrentY + 1;
        if (j < 8)
        {
            c = Board_Manager.Instance.piceses[i, j];
            if (c == null || c.isWhite != isWhite)
            {
                moves[i, j] = true;
            }
        }

        //Left
        i = CurrentX - 1;
        j = CurrentY;
        if (i >= 0)
        {
            c = Board_Manager.Instance.piceses[i, j];
            if (c == null || c.isWhite != isWhite)
            {
                moves[i, j] = true;
            }
        }

        //Right
        i = CurrentX + 1;
        j = CurrentY;
        if (i < 8)
        {
            c = Board_Manager.Instance.piceses[i, j];
            if (c == null || c.isWhite != isWhite)
            {
                moves[i, j] = true;
            }
        }

        //Down Left
        i = CurrentX - 1;
        j = CurrentY - 1;
        if (i >= 0 && j >= 0)
        {
            c = Board_Manager.Instance.piceses [i, j];
            if (c == null || c.isWhite != isWhite)
            {
                moves[i, j] = true;
            }
        }

        //Down Right
        i = CurrentX + 1;
        j = CurrentY - 1;
        if (i < 8 && j >=0)
        {
            c = Board_Manager.Instance.piceses[i, j];
            if (c == null || c.isWhite != isWhite)
            {
                moves[i, j] = true;
            }
        }

        //Down
        i = CurrentX;
        j = CurrentY - 1;
        if (j >= 0)
        {
            c = Board_Manager.Instance.piceses[i, j];
            if (c == null || c.isWhite != isWhite)
            {
                moves[i, j] = true;
            }
        }

        //Rokade
        i = CurrentX;
        j = CurrentY;
        bool legalRokade = true;
        if (!HasMoved)
        {
            if (isWhite)
            {
                if (Board_Manager.Instance.piceses[i + 1, j] == null
                    && Board_Manager.Instance.piceses[i + 2, j] == null
                    && Board_Manager.Instance.piceses[i + 3, j] != null) //short Rokade
                {
                    for (int s = 0; s < Board_Manager.Instance.activeChessPices.Count; s++) //macking sure it is not indangering
                    {
                        c = Board_Manager.Instance.activeChessPices[s].GetComponent<Pices>();
                        for (int n = 0; n < 3; n++)
                        {
                            if (c.isWhite != isWhite && c.oldMoves[i + n, 0])
                            {
                                legalRokade = false;
                                //print("disturbuing pice: " + c.PicesLetter + " short " + c.isWhite);
                                break; //if found thats it
                            }
                        }
                    }
                    if (!Board_Manager.Instance.piceses[i + 3, j].HasMoved && legalRokade)
                    {
                        moves[i + 2, j] = true;
                    }
                    legalRokade = true;
                }
                if (Board_Manager.Instance.piceses[i - 1, j] == null
                    && Board_Manager.Instance.piceses[i - 2, j] == null
                    && Board_Manager.Instance.piceses[i - 3, j] == null
                    && Board_Manager.Instance.piceses[i - 4, j] != null) //Long Rokade
                {
                    for (int s = 0; s < Board_Manager.Instance.activeChessPices.Count; s++) //macking sure it is not indangering
                    {
                        c = Board_Manager.Instance.activeChessPices[s].GetComponent<Pices>();
                        for (int n = 0; n < 4; n++)
                        {
                            if (c.isWhite != isWhite && c.oldMoves[i - n, 0])
                            {
                                legalRokade = false;
                                //print("disturbuing pice:: " + c.PicesLetter + " long " + c.isWhite);
                                break; //if found thats it
                            }
                        }
                    }
                    if (!Board_Manager.Instance.piceses[i - 4, j].HasMoved && legalRokade)
                    {
                        moves[i - 2, j] = true;
                    }
                }
            }
            else
            {
                if (Board_Manager.Instance.piceses[i + 1, j] == null
                    && Board_Manager.Instance.piceses[i + 2, j] == null
                    && Board_Manager.Instance.piceses[i + 3, j] != null) //short Rokade
                {
                    for (int s = 0; s < Board_Manager.Instance.activeChessPices.Count; s++) //macking sure it is not indangering
                    {
                        c = Board_Manager.Instance.activeChessPices[s].GetComponent<Pices>();
                        for (int n = 1; n < 3; n++)
                        {
                            if (c.isWhite != isWhite && c.oldMoves[i + n, 7] || c.isWhite != isWhite && c.oldMoves[i, 7])
                            {
                                legalRokade = false;
                                //print("disturbuing pice: " + c.PicesLetter + " short " + c.isWhite);
                                break; //if found thats it
                            }
                        }
                    }
                    if (!Board_Manager.Instance.piceses[i + 3, j].HasMoved && legalRokade)
                    {
                        moves[i + 2, j] = true;
                    }
                    legalRokade = true;
                }
                if (Board_Manager.Instance.piceses[i - 1, j] == null
                    && Board_Manager.Instance.piceses[i - 2, j] == null
                    && Board_Manager.Instance.piceses[i - 3, j] == null
                    && Board_Manager.Instance.piceses[i - 4, j] != null) //Long Rokade
                {
                    for (int s = 0; s < Board_Manager.Instance.activeChessPices.Count; s++) //macking sure it is not indangering
                    {
                        c = Board_Manager.Instance.activeChessPices[s].GetComponent<Pices>();
                        for (int n = 1; n < 4; n++)
                        {
                            if (c.isWhite != isWhite && c.oldMoves[i - n, 7] || c.isWhite != isWhite && c.oldMoves[i, 7])
                            {
                                legalRokade = false;
                                //print("disturbuing pice: " + c.PicesLetter + " long " + c.isWhite);
                                break; //if found thats it
                            }
                        }
                    }
                    if (!Board_Manager.Instance.piceses[i - 4, j].HasMoved && legalRokade)
                    {
                        moves[i - 2, j] = true;
                    }
                }
            }
        }

        //-----------------------------------------------------------self checking (not working add later)
        /*
        //removing self checkking
        for (int k = 0; k < Board_Manager.Instance.activeChessPices.Count; k++) //removing pund diaganals from posability
        {
            c = Board_Manager.Instance.activeChessPices[k].GetComponent<Pices>();
            if (c.isWhite == true && isWhite == false && c.PicesLetter == ' ')
            {
                print("sort konge fandt hvid bunde ved: " + c.CurrentX + "," + c.CurrentY);
            }
            else if (c.isWhite == false && isWhite == true && c.PicesLetter == ' ')
            {
                print("curent Y: " + CurrentY);
                if (c.CurrentX + 1 <= 7)
                {
                    print("removing: " + (c.CurrentX + 1) + "," + (CurrentY - 1));
                    moves[c.CurrentX + 1, CurrentY - 1] = false;
                }
                if (c.CurrentX - 1 >= 0)
                {
                    print("removing: " + (c.CurrentX - 1) + "," + (CurrentY - 1));
                    moves[c.CurrentX - 1, CurrentY - 1] = false;
                }
            }
        }
        
        for (int k = 0; k < 8; k++)
        {
            for (int n = 0; n < 8; n++)
            {
                if (moves[k, n])
                {
                    for (int s = 0; s < Board_Manager.Instance.activeChessPices.Count; s++)
                    {
                        c = Board_Manager.Instance.activeChessPices[s].GetComponent<Pices>();
                        if (c.PicesLetter != 'K' && c.PicesLetter != ' ' && c.isWhite != isWhite && c.oldMoves[k, n])
                        {
                            moves[k, n] = false;
                            print("is white: " + c.isWhite + " " + c.PicesLetter + " prevents self check at " + k + "," + n);
                        }
                        if (c.PicesLetter == 'K' && c.isWhite != isWhite) //for the king in situation where moves has been removed do to self checking
                        {
                            if (c.CurrentX + 1 < 8)
                            {
                                moves[c.CurrentX + 1, c.CurrentY] = false;
                            }
                            if (c.CurrentX + 1 < 8 && c.CurrentY - 1 >= 0)
                            {
                                moves[c.CurrentX + 1, c.CurrentY - 1] = false;
                            }
                            if (c.CurrentY - 1 >= 0)
                            {
                               moves[c.CurrentX, c.CurrentY - 1] = false;
                            }
                            if (c.CurrentX - 1 >= 0 && c.CurrentY - 1 >= 0)
                            {
                                moves[c.CurrentX - 1, c.CurrentY - 1] = false;
                            }
                            if (c.CurrentX - 1 >= 0)
                            {
                                moves[c.CurrentX - 1, c.CurrentY] = false;
                            }
                            if (c.CurrentX - 1 >= 0 && c.CurrentY + 1 < 8)
                            {
                                moves[c.CurrentX - 1, c.CurrentY + 1] = false;
                            }
                            if (c.CurrentY + 1 < 8)
                            {
                                moves[c.CurrentX, c.CurrentY + 1] = false;
                            }
                            if (c.CurrentX + 1 < 8 && c.CurrentY + 1 < 8)
                            {
                                moves[c.CurrentX + 1, c.CurrentY + 1] = false;
                            }
                            //print("all awrond king is not leagle");
                        }
                    }
                }
            }
        }
        */
        oldMoves = moves;
        return moves;
    }
}
