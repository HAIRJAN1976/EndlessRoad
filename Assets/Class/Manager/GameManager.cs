using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public Transform board;
    private GameObject[,] boardBase;

    public string[,] stage01;
    private int stageXCord = 10;
    private int stageYCord = 10;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private Vector2 startCordinate;
    private Vector2 endCordinate;

    private List<Node> matrix;

    private GameObject character;

    // Use this for initialization
    void Start()
    {
        string[,] temp = {  {"1:61:1","1:62:1","1:63:1","1:64:1","1:65:1","1:66:1","1:67:1","1:68:1","1:69:1","1:70:1"},
                            {"1:62:1","1:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:3","1:70:1"},
                            {"1:63:1","1:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","1:70:1"},
                            {"1:64:1","1:0:0","3:0:2","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","1:70:1"},
                            {"1:65:1","1:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","1:70:1"},
                            {"1:66:1","1:0:0","3:0:0","3:0:0","6:0:0","3:0:0","3:0:0","3:0:0","3:0:0","1:70:1"},
                            {"1:67:1","1:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","1:70:1"},
                            {"1:69:1","1:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","1:70:1"},
                            {"1:70:1","1:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","1:70:1"},
                            {"1:61:1","1:62:1","1:63:1","1:64:1","1:65:1","1:66:1","1:67:1","1:68:1","1:69:1","1:70:1"}};

        matrix = new List<Node>();
        boardBase = new GameObject[stageXCord, stageYCord];

        stage01 = temp;

        CreateBoard();

        CreateCharacter();

        this.GetComponent<TargetFollowMovement>().sourceTransform = character.transform;
        this.GetComponent<ClickManager>().characterTransform = character.transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateCharacter()
    {
        for (int x = 0; x < stageXCord; x++)
        {
            for (int y = 0; y < stageYCord; y++)
            {
                if (int.Parse(stage01[x, y].Split(':')[2]) == 2)
                {
                    GameObject model = Resources.Load<GameObject>("Prefabs/Character");
                    character = (GameObject)Instantiate(model, new Vector3(x * 3, 2, y * 3 - 1.5f), Quaternion.identity, transform);
                    character.name = "Character";

                    startPosition = new Vector3(x * 3, 2, y * 3 - 1.5f);
                    startCordinate = new Vector2(x, y);
                }

                if (int.Parse(stage01[x, y].Split(':')[2]) == 3)
                {
                    endPosition = new Vector3(x * 3, 2, y * 3 - 1.5f);
                    endCordinate = new Vector2(x, y);
                }
            }
        }
    }

    private void CreateBoard()
    {
        for (int x = 0; x < stageXCord; x++)
        {
            for (int y = 0; y < stageYCord; y++)
            {
                boardBase[x, y] = BlockManager.GetBlock(board, x, y, stage01[x, y]);

                if (boardBase[x, y].tag == "UnPassable")
                {
                    //matrix.Add(new Node(x, y, false));
                }
                else
                {
                    //matrix.Add(new Node(x, y));
                }
            }
        }

        foreach (Node node in matrix)
        {
            node.SetNeigbours(matrix, 0.3f);
        }
    }
}
