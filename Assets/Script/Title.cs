using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour {

	public Button start;
	// Use this for initialization
	void Start () {
		start.onClick.AddListener(() =>{SceneLoader.Instance.ChangeScene("start",true);});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
