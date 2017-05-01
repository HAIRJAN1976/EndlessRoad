using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour
{

    public Vector3 startPositionMatrix;
    public GameObject standartNode;
    public float nodeDistance;
    public float nodeCountX;
    public float nodeCountY;

    public int wallPercent = 10;


    private Pathfinder pathfinder;
    private List<Node> matrix;
    private Node startNode, endNode;


    void Start()
    {
        matrix = new List<Node>();
        CreateMatrix();

        pathfinder = new AStar();
        pathfinder.SetPath(startNode, endNode);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            pathfinder.FindPathNextStep();
        }
        if (Input.GetKeyDown("f"))
        {
            pathfinder.FindPathRestSteps();
        }
        if (Input.GetKeyDown("r"))
        {
            Reset();
        }
    }

    private void Reset()
    {
        if (matrix == null || matrix.Count != 0)
        {
            DestroyMatrix();
        }
        matrix = new List<Node>();
        CreateMatrix();

        pathfinder = new AStar();
        pathfinder.SetPath(startNode, endNode);
    }

    private void CreateMatrix()
    {
        float deltaPositionX = 0;
        float deltaPositionY = 0;

        for (int i = 0; i < nodeCountX; i++)
        {
            for (int j = 0; j < nodeCountY; j++)
            {
                Vector3 delta = new Vector3(deltaPositionX, deltaPositionY, 0);

                GameObject newNode = Instantiate(standartNode);
                newNode.transform.position = startPositionMatrix + delta;

                matrix.Add(newNode.GetComponent<Node>());

                deltaPositionX += nodeDistance;

                if (Random.Range(0, 100) <= wallPercent)
                {
                    newNode.GetComponent<Node>().setToWall();
                }
                else
                {
                    newNode.GetComponent<Node>().setDefault();
                }
            }
            deltaPositionX = 0;
            deltaPositionY += nodeDistance;
        }

        foreach (Node node in matrix)
        {
            node.setNeigbours(matrix, nodeDistance);
        }

        SetStartEnd();
    }

    private void DestroyMatrix()
    {
        foreach (Node node in matrix)
        {
            Destroy(node.gameObject);
        }
    }

    private void SetStartEnd()
    {
        int start = Random.Range(0, matrix.Count);
        int end;
        do
        {
            end = Random.Range(0, matrix.Count);
        } while (end == start);

        startNode = matrix[start];
        startNode.setToStart();
        endNode = matrix[end];
        endNode.setToEnd();
    }
}
