using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pices : MonoBehaviour
{
    public int CurrentX { get; set; } //saving local piece position
    public int CurrentY { get; set; } //saving local piece position
    public bool HasMoved { get; set; } //saving if the pice has moved doing the game
    public bool isWhite; //stawing team
    public bool[,] oldMoves = new bool[8,8];
    public char PicesLetter = 'F';

    public void SetPosition(int x, int y) //updating local position variables
    {
        CurrentX = x;
        CurrentY = y;
    }

    public virtual bool[,] PossibleMove() //standert posible moves
    {
        return new bool[8,8];
    }

    public bool Check()
    {
        bool[,] moves = PossibleMove();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (moves[i, j]) 
                {
                    if (Board_Manager.Instance.piceses[i, j] != null)
                    {
                        if (Board_Manager.Instance.piceses[i, j].GetType() == typeof(Konge) && Board_Manager.Instance.piceses[i, j].isWhite != isWhite)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
}

                     

