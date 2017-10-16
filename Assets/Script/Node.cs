using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>{

	public bool walkable;
	public Vector3 position;
	public int gridX;
	public int gridY;
	public int gCost;
	public int hCost;
	public Node parent;

	int heapIndex;

	public Node(bool w, Vector3 p, int x, int y){
		walkable = w;
		position = p;
		gridX = x;
		gridY = y;
	}

	public int fCost {
		get {
			return gCost + hCost;
		}
	}

	public int HeapIndex {
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

	public int CompareTo(Node nodeToCompare) {
		int compare = fCost.CompareTo(nodeToCompare.fCost);
		if (compare == 0) {
			compare = hCost.CompareTo(nodeToCompare.hCost);
		}
		return -compare;
	}
}

public class WayPoint{
	public Vector3 point;
	public Walking.Facing direction;

	public WayPoint(Vector3 p, Walking.Facing d){
		point = p;
		direction = d;
	}
}