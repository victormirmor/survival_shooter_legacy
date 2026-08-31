#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptions : MonoBehaviour {
	public Dropdown DropdownResolution, DropdownQuality, DropdownShadows, DropdownVsync, DropdownantiAliasing,DropdownScreen;
	void Start()
	{
		DropdownResolution.value = PlayerPrefs.GetInt("Resolution");
		DropdownantiAliasing.value = PlayerPrefs.GetInt("antiAliasing");		
		DropdownQuality.value = PlayerPrefs.GetInt("Quality");
		DropdownShadows.value = PlayerPrefs.GetInt("Shadows");
		DropdownScreen.value = PlayerPrefs.GetInt("Screen");
		DropdownVsync.value = PlayerPrefs.GetInt("Vsync");
		
	}
	void OnDisable(){

		PlayerPrefs.SetInt("antiAliasing", DropdownantiAliasing.value);
		PlayerPrefs.SetInt("Resolution", DropdownResolution.value);
		PlayerPrefs.SetInt("Quality",DropdownQuality.value);
		PlayerPrefs.SetInt("Shadows", DropdownShadows.value);
		PlayerPrefs.SetInt("Screen",DropdownScreen.value);
		PlayerPrefs.SetInt("Vsync",DropdownVsync.value);
    }
	public void SetResolution (int Level){
		if(Level==0){
			Screen.SetResolution(848, 480, false);
		}
		if(Level==1){
			Screen.SetResolution(1024, 600, false);
		}
		if(Level==2){
			Screen.SetResolution(1280, 720, false);
		}
		if(Level==3){
			Screen.SetResolution(1920, 1080, false);
		}		 
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
	public void ShadowsLevel(int Level)
	{
		if (Level == 0)
		{
			QualitySettings.shadows = ShadowQuality.Disable;
			Debug.Log(QualitySettings.shadows);
		}
		else if (Level == 1)
		{
			QualitySettings.shadows = ShadowQuality.HardOnly;
			Debug.Log(QualitySettings.shadows);
		}
		else if (Level == 2)
		{
			QualitySettings.shadows = ShadowQuality.All;
			Debug.Log(QualitySettings.shadows);
		}

	}
	public void Vsync(int Vsync)
	{

		if (Vsync == 0)
		{
			QualitySettings.vSyncCount = 0;
			Debug.Log(QualitySettings.vSyncCount);
		}
		else if (Vsync == 1)
		{
			QualitySettings.vSyncCount = 1;
			Debug.Log(QualitySettings.vSyncCount);
		}
		else if (Vsync == 2)
		{
			QualitySettings.vSyncCount = 2;
			Debug.Log(QualitySettings.vSyncCount);
		}
	}
	public void Set_antiAliasing(int Level)
	{

		if (Level == 0)
		{
			QualitySettings.antiAliasing = 0;
			Debug.Log(QualitySettings.antiAliasing);

		}
		else if (Level == 1)
		{
			QualitySettings.antiAliasing = 2;
			Debug.Log(QualitySettings.antiAliasing);
		}
		else if (Level == 2)
		{
			QualitySettings.antiAliasing = 4;
			Debug.Log(QualitySettings.antiAliasing);
		}
		else if (Level == 3)
		{
			QualitySettings.antiAliasing = 8;
			Debug.Log(QualitySettings.antiAliasing);
		}
	}
	public void fullScreen(int Level){
		if(Level==0){
			Screen.fullScreen = false;
		}else if(Level==1){
			Screen.fullScreen = true;
		}
	}
	
}
