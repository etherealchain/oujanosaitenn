using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

	[HideInInspector]
	public string finalFace;
	[HideInInspector]
	public bool hideWhenFinish;
	[HideInInspector]
	public bool moveComplete;
	[HideInInspector]
	public Vector3 destination;
	public float speed = 1;

	WayPoint[] path;
	int targetIndex;
	Animator animator;

	void Awake(){
		finalFace = null;
		moveComplete = true;
		hideWhenFinish = false;
	}
	void Start(){
		animator = GetComponent<Animator>();
	}

	public void startPathFinding(Vector3 target){
		if(moveComplete){
			moveComplete = false;
			destination = target;
			PathRequester.RequestPath(transform.position, target, OnPathFound);
		}
	}

	public void OnPathFound(WayPoint[] newPath, bool pathSuccessful) {
		if (pathSuccessful) {
			path = newPath;
			targetIndex = 0;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
		else{
			Debug.Log("fail?");
		}
	}

	IEnumerator FollowPath() {
		WayPoint currentWaypoint = path[0];
		bool start = true;
		while (true) {
			if(start){
				switch(currentWaypoint.direction){
					default:
					case Walking.Facing.up:
						animator.SetTrigger(StaticUtil.animeWalkUp);
					break;
					case Walking.Facing.down:
						animator.SetTrigger(StaticUtil.animeWalkDown);
					break;
					case Walking.Facing.right:
						animator.SetTrigger(StaticUtil.animeWalkRight);
					break;
					case Walking.Facing.left:
						animator.SetTrigger(StaticUtil.animeWalkLeft);
					break;
				}
				start = false;
			}

			if (transform.position == currentWaypoint.point) {
				targetIndex ++;
				if (targetIndex >= path.Length) {
					if(finalFace != null){
						animator.SetTrigger(finalFace);
						finalFace = null;
					}
					if(hideWhenFinish)
						transform.GetComponent<Renderer>().material.color = new Color(0,0,0,0);
					moveComplete = true;
					yield break;
				}
				currentWaypoint = path[targetIndex];
				switch(currentWaypoint.direction){
					default:
					case Walking.Facing.up:
						animator.SetTrigger(StaticUtil.animeWalkUp);
					break;
					case Walking.Facing.down:
						animator.SetTrigger(StaticUtil.animeWalkDown);
					break;
					case Walking.Facing.right:
						animator.SetTrigger(StaticUtil.animeWalkRight);
					break;
					case Walking.Facing.left:
						animator.SetTrigger(StaticUtil.animeWalkLeft);
					break;
				}
			}

			transform.position = Vector3.MoveTowards(transform.position,currentWaypoint.point, speed * Time.deltaTime);
			yield return null;
		}
	}
}
