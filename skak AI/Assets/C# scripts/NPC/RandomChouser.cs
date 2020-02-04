using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomChouser : MonoBehaviour
{
    public bool IsActiveWhite;
    public bool IsActiveBlack;
    private List<string> moves;

    // Start is called before the first frame update
    void Start()
    {
        print("start game");
        moves = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActiveWhite && Board_Manager.Instance.isWhiteTurn) 
        {
            moves = Board_Manager.Instance.AllMoves();
            RandomMove(moves);
        }
        if (IsActiveBlack && !Board_Manager.Instance.isWhiteTurn)
        {
            moves = Board_Manager.Instance.AllMoves();
            RandomMove(moves);
        }
    }

    private void RandomMove(List<string> moveStrings) 
    {
        int random = (int)Random.Range(0, moveStrings.Count);
        Board_Manager.Instance.MoveFromeString(moveStrings[random]);
    }
}
