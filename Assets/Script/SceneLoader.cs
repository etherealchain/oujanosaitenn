using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : SingletonObject<SceneLoader>{

	public Texture2D blackTexture;
	public Texture2D whiteTexture;
	[HideInInspector]
	public Language currentLanguage;
	[HideInInspector]
	public Font currentFont;
	[HideInInspector]
	public enum Language{
		japanese,
		english,
		chinese
	}
	[HideInInspector]
	public LangTable table;

	float fadeSpeed ;
	float alpha ;
	int fadeDir ; 
	int textureDepth ;
	AudioSource audioSource ;
	bool setAudio ;
	bool useWhite;
	
	
	void Awake(){
		DontDestroyOnLoad(this.gameObject);
		fadeSpeed = 0.5f;
		alpha = 1;
		fadeDir = -1;   // in = -1, out = 1;
		textureDepth = -10;
		audioSource = null;
		setAudio = false;
		useWhite = true;
		currentLanguage = Language.japanese;
		currentFont = Resources.Load("font/JKG-M_3") as Font;

		// load language string
		TextAsset json = Resources.Load("lang_table") as TextAsset;
		table = JsonUtility.FromJson<LangTable>(json.text);
	}
	void OnEnable(){
		SceneManager.sceneLoaded += onLevelLoaded;
		if(whiteTexture == null){
			whiteTexture = Resources.Load("Square") as Texture2D;
		}
		if(blackTexture == null){
			blackTexture = Resources.Load("black") as Texture2D;
		}
	}
	void OnDisable(){
		SceneManager.sceneLoaded -= onLevelLoaded;
	}

	void OnGUI(){
		alpha += fadeDir*fadeSpeed*Time.deltaTime;
		alpha = Mathf.Clamp01(alpha);
		if(setAudio){
			audioSource.volume = 1 - alpha;
			if(audioSource.volume == 0 || audioSource.volume == 1)
				setAudio = false;
		}

		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = textureDepth;
		if(useWhite)
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),whiteTexture);
		else
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),blackTexture);
	}

	void onLevelLoaded(Scene scene, LoadSceneMode mode){
		audioSource = GameObject.Find("Manager").GetComponent<AudioSource>();
		if(audioSource != null){
			setAudio = true;
		}
		else{
			setAudio = false;
		}
		fadeDir = -1;
	}
	IEnumerator waitforFade(string scene){
		yield return new WaitForSeconds(1.5f);
		SceneManager.LoadScene(scene,LoadSceneMode.Single);
	}

	public void ChangeScene(string scene, bool white){
		audioSource = GameObject.Find("Manager").GetComponent<AudioSource>();
		if(audioSource != null){
			setAudio = true;
		}
		else{
			setAudio = false;
		}
		useWhite = white;
		fadeDir = 1;
		StartCoroutine(waitforFade(scene));
	}

	public void setCurrentLanguage(Language lang){
		currentLanguage = lang;
		switch(currentLanguage){
			case SceneLoader.Language.japanese:
				currentFont = Resources.Load("font/JKG-M_3") as Font;
				break;
			case SceneLoader.Language.english:
				currentFont = Resources.Load("font/arialbd") as Font;
				break;
			case SceneLoader.Language.chinese:
				currentFont = Resources.Load("font/msjhbd") as Font;
				break;
		}
	}

	public string getCurrentSceneName(){
		return SceneManager.GetActiveScene().name;
	}
}
