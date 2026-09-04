#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PauseOptions : MonoBehaviour {
	public Dropdown DropdownQuality;

	void Start()
	{

		DropdownQuality.value = PlayerPrefs.GetInt("Quality");
	}
	void OnDisable(){
		PlayerPrefs.SetInt("Quality",DropdownQuality.value);
    }

	public void Get_Quality (int Level){
		
		if(Level==0){
			QualitySettings.SetQualityLevel(0,false);
		}else if(Level==1){
			QualitySettings.SetQualityLevel(1,true);
		}else if(Level==2){
			QualitySettings.SetQualityLevel(2,true);
		}else if(Level==3){
			QualitySettings.SetQualityLevel(3,true);
		}
	}
}
