using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Pathfinder {
	
	public abstract void SetPath (Node start, Node end);

	public abstract bool FindPathNextStep ();

	public abstract void FindPathRestSteps ();

	public abstract LinkedList<Node> FindPathFast (Node start, Node end);

	public abstract LinkedList<Node> GetPath ();
}
