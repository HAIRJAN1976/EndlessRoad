using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public GameObject standartNode;

    public Transform board;
    private GameObject[,] boardBase;

    public string[,] stage01;
    private int stageXCord = 10;
    private int stageYCord = 10;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private Vector2 startCordinate;
    private Vector2 endCordinate;

    private GameObject character;

    private List<Node> matrix;
    private float nodeDistance = 3;
    private Node startNode, endNode;
    private AStar pathfinder;

    // Use this for initialization
    void Start()
    {
        string[,] temp = {  {"1:61:1","1:62:1","1:63:1","1:64:1","1:65:1","1:66:1","1:67:1","1:68:1","1:69:1","1:70:1"},
                            {"1:62:1","1:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","1:70:1"},
                            {"1:63:1","1:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","1:70:1"},
                            {"1:64:1","1:0:0","3:0:0","3:0:2","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","1:70:1"},
                            {"1:65:1","1:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","1:70:1"},
                            {"1:66:1","1:0:0","3:0:0","3:0:0","6:0:0","3:0:0","3:0:0","3:0:0","3:0:0","1:70:1"},
                            {"1:67:1","1:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","1:70:1"},
                            {"1:69:1","1:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:3","3:0:0","1:70:1"},
                            {"1:70:1","1:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","3:0:0","1:70:1"},
                            {"1:61:1","1:62:1","1:63:1","1:64:1","1:65:1","1:66:1","1:67:1","1:68:1","1:69:1","1:70:1"}};

        matrix = new List<Node>();
        boardBase = new GameObject[stageXCord, stageYCord];

        stage01 = temp;

        CreateCharacter();
        CreateBoard();

        

        this.GetComponent<TargetFollowMovement>().sourceTransform = character.transform;
        this.GetComponent<ClickManager>().characterTransform = character.transform;

        ///////////////////////////
        matrix = new List<Node>();
        pathfinder = new AStar();
        pathfinder.SetPath(startNode, endNode);
        pathfinder.FindPathRestSteps();

        Node[] roots = pathfinder.GetPath().ToArray();


        for (int i = 0; i < roots.Count(); i++)
        {
            if (i + 1 < roots.Count())
            {
                GameObject lineObject = new GameObject();
                LineRenderer line = lineObject.AddComponent<LineRenderer>();
                line.SetPosition(0, new Vector3(roots[i].cordinate.x * 3 - 1.5f, 1, roots[i].cordinate.y * 3 - 1.5f));
                line.SetPosition(1, new Vector3(roots[i + 1].cordinate.x * 3 - 1.5f, 1, roots[i + 1].cordinate.y * 3 - 1.5f));
                line.SetWidth(0.1f, 0.1f);
            }
        }
        /////////////////////////////////

    }

    // Update is called once per frame
    void Update()
    {
        //iTween.PutOnPath(character, )
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
                    character = (GameObject)Instantiate(model, new Vector3(x * 3 - 1.5f, 2, y * 3 - 1.5f), Quaternion.identity, transform);
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
        float deltaPositionX = 0;
        float deltaPositionY = 0;

        for (int x = 0; x < stageXCord; x++)
        {
            for (int y = 0; y < stageYCord; y++)
            {
                boardBase[x, y] = BlockManager.GetBlock(board, x, y, stage01[x, y]);

                Vector3 delta = new Vector3(deltaPositionX - 1.5f, 1, deltaPositionY - 1.5f);

                GameObject newNode = Instantiate(standartNode);
                newNode.name = string.Format("Node {0},{1}", x, y);

                newNode.transform.position = delta;
                newNode.GetComponent<Node>().cordinate = new Vector2(x, y);

                matrix.Add(newNode.GetComponent<Node>());

                deltaPositionY += nodeDistance;


                if (boardBase[x, y].tag == "UnPassable")
                {
                    newNode.GetComponent<Node>().setToWall();
                }
                else
                {
                    newNode.GetComponent<Node>().SetDefault();
                }
                
            }
            deltaPositionY = 0;
            deltaPositionX += nodeDistance;
        }

        foreach (Node node in matrix)
        {
            node.SetNeigbours(matrix, nodeDistance);
        }

        SetStartEnd();
    }

    private void SetStartEnd()
    {
        //int start = UnityEngine.Random.Range(0, matrix.Count);
        //int end;
        //do
        //{
        //    end = UnityEngine.Random.Range(0, matrix.Count);
        //} while (end == start);

        startNode = matrix[stageXCord * (int)startCordinate.x + (int)startCordinate.y];
        startNode.setToStart();
        endNode = matrix[stageXCord * (int)endCordinate.x + (int)endCordinate.y];
        endNode.setToEnd();
    }
}
