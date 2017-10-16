using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequester : MonoBehaviour {

	static PathRequester staticRequester;
	Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
	PathFinding pathFinder;
	PathRequest currentRequest;
	bool isProcessingPath;

	struct PathRequest {
		public Vector3 pathStart;
		public Vector3 pathEnd;
		public Action<WayPoint[], bool> callback;

		public PathRequest(Vector3 _start, Vector3 _end, Action<WayPoint[], bool> _callback) {
			pathStart = _start;
			pathEnd = _end;
			callback = _callback;
		}
	}

	void Awake(){
		staticRequester = this;
		pathFinder = GetComponent<PathFinding>();
	}

	
	public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<WayPoint[], bool> callback) {
		PathRequest newRequest = new PathRequest(pathStart,pathEnd,callback);
		staticRequester.pathRequestQueue.Enqueue(newRequest);
		staticRequester.TryProcessNext();
	}

	void TryProcessNext() {
		if (!isProcessingPath && pathRequestQueue.Count > 0) {
			currentRequest = pathRequestQueue.Dequeue();
			isProcessingPath = true;
			pathFinder.StartFindPath(currentRequest.pathStart, currentRequest.pathEnd);
		}
	}

	public void FinishedProcessingPath(WayPoint[] path, bool success) {
		currentRequest.callback(path,success);
		isProcessingPath = false;
		TryProcessNext();
	}
}
