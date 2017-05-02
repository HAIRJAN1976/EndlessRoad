using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar : Pathfinder
{

    public List<AStarNode> openList, closedList;
    private AStarNode start, end;
    private bool foundPath;

    public override void SetPath(Node start, Node end)
    {
        this.start = start as AStarNode;
        this.end = end as AStarNode;
        openList = new List<AStarNode>();
        closedList = new List<AStarNode>();
        openList.Add(start as AStarNode);
        foundPath = false;
    }

    // Returns true if found end
    public override bool FindPathNextStep()
    {
        if (!foundPath)
        {
            AStarNode current = GetLowestFCost(openList);
            if (current != null)
            {
                RemoveOpenList(current);
                AddClosedList(current);

                if (current.Equals(end))
                {
                    foundPath = true;
                    return foundPath;
                }

                foreach (AStarNode neighbour in current.neighbours)
                {
                    if (!neighbour.walkable || closedList.Contains(neighbour))
                    {
                        continue;
                    }

                    if (!(current.cordinate.x == neighbour.cordinate.x || current.cordinate.y == neighbour.cordinate.y))
                    {
                        continue;
                    }

                    float newFCost = neighbour.CalculateFCost(current, end);
                    if (newFCost < neighbour.fcost || !openList.Contains(neighbour))
                    {
                        neighbour.fcost = newFCost;
                        neighbour.parent = current;
                        if (!openList.Contains(neighbour))
                        {
                            AddOpenList(neighbour);
                        }
                    }
                }
            }
        }
        else
        {
            GetPath();
        }
        return foundPath;
    }

    public override void FindPathRestSteps()
    {
        do
        {
            foundPath = FindPathNextStep();
        }
        while ((!foundPath && openList.Count > 0));

        GetPath();
    }

    public override LinkedList<Node> FindPathFast(Node start, Node end)
    {
        SetPath(start, end);
        FindPathRestSteps();

        if (foundPath)
        {
            return GetPath();
        }
        return null;
    }

    public override LinkedList<Node> GetPath()
    {
        if (foundPath)
        {

            LinkedList<Node> path = new LinkedList<Node>();
            AStarNode current = (AStarNode)end;
            while (current != null)
            {
                path.AddFirst(current);
                current.setPath();
                current = current.parent;
            }

            return path;
        }
        return null;
    }

    private AStarNode GetLowestFCost(List<AStarNode> list)
    {
        AStarNode lowestFcurrent = null;
        foreach (AStarNode current in list)
        {
            if (lowestFcurrent == null || current.fcost < lowestFcurrent.fcost)
            {
                lowestFcurrent = current;
            }
        }
        return lowestFcurrent;
    }

    private void AddOpenList(AStarNode node)
    {
        openList.Add(node);
        node.setInspected();
    }

    private void RemoveOpenList(AStarNode node)
    {
        openList.Remove(node);
        node.setVisited();
    }

    private void AddClosedList(AStarNode node)
    {
        closedList.Add(node);
        node.setVisited();
    }
}
