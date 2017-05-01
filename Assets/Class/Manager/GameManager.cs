using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
    public Transform board;
    private GameObject[,] boardBase;

    public string[,] stage01;
    private int stageXCord = 10;
    private int stageYCord = 10;

    private Vector3 positionStart;
    private Vector3 positionEnd;

    private GameObject character;

    private AStar astar;

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

        astar = this.GetComponent<AStar>();
        astar.Initialize(stageXCord, positionEnd);

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

                    positionStart = new Vector3(x * 3, 2, y * 3 - 1.5f);
                }

                if (int.Parse(stage01[x, y].Split(':')[2]) == 3)
                {
                    positionEnd = new Vector3(x * 3, 2, y * 3 - 1.5f);
                }
            }
        }
    }

    private void CreateBoard()
    {
        boardBase = new GameObject[10, 10];

        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                boardBase[x, y] = BlockManager.GetBlock(board, x, y, stage01[x, y]);

                if(boardBase[x, y].transform.tag == GameCode.Tag.UnPassable.ToString())
                {
                    astar.nodes[x, y] = new Node(new Vector2(x, y), true);
                }
                else
                {
                    astar.nodes[x, y] = new Node(new Vector2(x, y));
                }
                //astar.nodes[x, y] = new Node(new Vector2(x, y));
                astar.openNodes[x, y] = new Node(new Vector2(x, y));
                astar.closedNodes[x, y] = new Node(new Vector2(x, y));
            }
        }
    }
}
