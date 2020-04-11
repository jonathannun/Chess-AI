using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomChouser : MonoBehaviour
{
    public bool IsActiveWhite;
    public bool IsActiveBlack;
    public int waightCound;
    private List<string> moves;

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
                    RandomMove(moves);
                }
            }
            if (IsActiveBlack && !Board_Manager.Instance.isWhiteTurn)
            {
                if ((Time.frameCount + (waightCound/2)) % waightCound == 0)
                {
                    moves = Board_Manager.Instance.AllMoves();
                    RandomMove(moves);
                }
            }
        }
    }

    private void RandomMove(List<string> moveStrings) 
    {
        int random = (int)Random.Range(0, moveStrings.Count);
        Board_Manager.Instance.MoveFromeString(moveStrings[random]);
    }
}
