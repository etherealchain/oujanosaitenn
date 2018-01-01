using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class TextPlayer : MonoBehaviour {

	public Transform textView;
	public Transform arrow;
	public float waitTimes = 0.08f;
	[HideInInspector]
	public bool isPlaying ;
	[HideInInspector]
	public bool over ;
	[HideInInspector]
	public bool showArrow;
	
	bool isFile ;
	Coroutine TextRoutine ;
	string showedLine;

	string[] fileLines;
	int fileIndex;
	string[] lines;
	int playIndex ;
	

	// Use this for initialization
	void Awake () {
		TextRoutine = null;
		fileIndex = 0;
		playIndex = 0;

		isPlaying = false;
		over = false;
		showArrow = true;
		arrow.gameObject.SetActive(false);

		string filename = SceneLoader.Instance.getCurrentSceneName();
		TextAsset story;

		switch(SceneLoader.Instance.currentLanguage){
			default:
			case SceneLoader.Language.japanese:
			story = Resources.Load("jp/"+filename) as TextAsset;
			break;
			case SceneLoader.Language.english:
			story = Resources.Load("en/"+filename) as TextAsset;
			break;
			case SceneLoader.Language.chinese:
			story = Resources.Load("ch/"+filename) as TextAsset;
			break;
		}
		textView.GetComponent<Text>().font = SceneLoader.Instance.currentFont;
		loadTextFile(story);
	}

	void loadTextFile(TextAsset script){
		fileIndex = 0;
		fileLines = script.text.Split(new string[]{"\r\n","\n"},StringSplitOptions.None);
		isFile = true;
	}

	public void finishText(){
		StopCoroutine(TextRoutine);
		textView.GetComponent<Text>().text = showedLine;
		isPlaying = false;
		if(showArrow)
			arrow.gameObject.SetActive(true);
	}
	public void setTextIndex(int index){
		if(index < fileLines.Length){
			fileIndex = index;
			if(TextRoutine != null)
				StopCoroutine(TextRoutine);
			TextRoutine = StartCoroutine(showOneLine(fileLines[fileIndex]));
		}
	}

	public void updateText(){
		if(isFile){
			if(fileIndex < fileLines.Length){
				TextRoutine = StartCoroutine(showOneLine(fileLines[fileIndex]));
			}
			else{
				over = true;
			}
		}
		else{
			if(playIndex < lines.Length){
				TextRoutine = StartCoroutine(showOneLine(lines[playIndex]));
			}
			else{
				if(fileIndex < fileLines.Length){
					isFile = true;
				}
				else{
					over = true;
				}
			}
		}
	}

	public int TextIndex{
		get{
			return fileIndex;
		}
	}

	public void playText(string text){
		isFile = false;
		lines = text.Split(new string[]{"\r\n","\n"},StringSplitOptions.None);
		playIndex = 0;
		TextRoutine = StartCoroutine(showOneLine(lines[0]));
	}
	public void activeArrow(){
		arrow.gameObject.SetActive(true);
		showArrow = true;
	}

	IEnumerator showOneLine(string line){
		arrow.gameObject.SetActive(false);
		int i = 0;
		textView.GetComponent<Text>().text = "";
		isPlaying = true;
		showedLine = line;
		if(isFile)
			fileIndex++;
		else
			playIndex++;
		
		while(i < line.Length){
			textView.GetComponent<Text>().text += line[i++];
			yield return new WaitForSeconds(waitTimes);
		}
		isPlaying = false;
		if(showArrow)
			arrow.gameObject.SetActive(true);
	}
}
