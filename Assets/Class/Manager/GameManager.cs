using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
    public Transform board;
    private GameObject[,] boardBase;
    public string[,] stage01;
    private int startingPointX = 5;
    private int startingPointY = 5;

    // Use this for initialization
    void Start()
    {
        string[,] temp = {  {"1:61:1","1:62:1","1:63:1","1:64:1","1:65:1","1:66:1","1:67:1","1:68:1","1:69:1","1:70:1"},
                            {"1:62:1","1:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","1:70:1"},
                            {"1:63:1","1:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","1:70:1"},
                            {"1:64:1","1:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","1:70:1"},
                            {"1:65:1","1:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","1:70:1"},
                            {"1:66:1","1:0:0","3:0:0","3:0:0","6:0:0","3:0:0","3:0:0","3:0:0","3:0:0","1:70:1"},
                            {"1:67:1","1:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","1:70:1"},
                            {"1:69:1","1:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","1:70:1"},
                            {"1:70:1","1:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","1:70:1"},
                            {"1:61:1","1:62:1","1:63:1","1:64:1","1:65:1","1:66:1","1:67:1","1:68:1","1:69:1","1:70:1"}};

        stage01 = temp;
        CreateBoard();
        CreateCharacter();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateCharacter()
    {
        GameObject model = Resources.Load<GameObject>("Prefabs/Character");
        GameObject gameObject = (GameObject)Instantiate(model, new Vector3(startingPointX * 3, 2, startingPointY * 3 - 1.5f), Quaternion.identity, transform);
        gameObject.name = "Character";
    }

    private void CreateBoard()
    {
        boardBase = new GameObject[10, 10];

        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                boardBase[x, y] = BlockManager.GetBlock(board, x, y, stage01[x, y]);
            }
        }
    }
}
