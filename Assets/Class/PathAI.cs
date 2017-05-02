using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class PathAI : MonoBehaviour
{

    public Vector3 startPositionMatrix;
    public GameObject standartNode;
    public float nodeDistance;
    public float nodeCountX;
    public float nodeCountY;

    public int wallPercent = 10;

    private AStar pathfinder;
    private List<Node> matrix;
    private Node startNode, endNode;


    void Start()
    {
        matrix = new List<Node>();
        CreateMatrix();

        pathfinder = new AStar();
        pathfinder.SetPath(startNode, endNode);
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            pathfinder.FindPathNextStep();
        }
        if (Input.GetKeyDown("f"))
        {
            pathfinder.FindPathRestSteps();

            Node[] roots = pathfinder.GetPath().ToArray();


            for (int i = 0; i < roots.Count(); i++)
            {
                if (i + 1 < roots.Count())
                {
                    GameObject lineObject = new GameObject();
                    LineRenderer line = lineObject.AddComponent<LineRenderer>();
                    line.SetPosition(0, new Vector3(roots[i].cordinate.y * 3 - 1.5f, 4, roots[i].cordinate.x * 3 - 1.5f));
                    line.SetPosition(1, new Vector3(roots[i + 1].cordinate.y * 3 - 1.5f, 4, roots[i + 1].cordinate.x * 3 - 1.5f));
                    line.SetWidth(0.1f, 0.1f);
                }

            }


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
                Vector3 delta = new Vector3(deltaPositionX - 1.5f, 2, deltaPositionY - 1.5f);

                GameObject newNode = Instantiate(standartNode);
                newNode.transform.position = startPositionMatrix + delta;
                newNode.GetComponent<Node>().cordinate = new Vector2(i, j);

                matrix.Add(newNode.GetComponent<Node>());

                deltaPositionX += nodeDistance;

                if (Random.Range(0, 100) <= wallPercent)
                {
                    newNode.GetComponent<Node>().setToWall();
                }
                else
                {
                    newNode.GetComponent<Node>().SetDefault();
                }


                newNode.GetComponent<Node>().cordinate = new Vector2(i, j);

            }
            deltaPositionX = 0;
            deltaPositionY += nodeDistance;
        }

        foreach (Node node in matrix)
        {

            node.SetNeigbours(matrix, nodeDistance);
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
