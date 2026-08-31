using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class menuPause : MonoBehaviour {

	public Canvas canvasPausa;

	void Awake(){
		canvasPausa.enabled = false;
		Time.timeScale = 1;
	}
	void Update(){
		if (Input.GetButtonDown ("Cancel")){
			Pause();}
	}

	public void Pause(){
		canvasPausa.enabled = !canvasPausa.enabled;
		Time.timeScale = Time.timeScale == 0 ? 1: 0;
	}

}
