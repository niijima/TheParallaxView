﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// little scene manager to report the scene names to the UI, and set active scene

public class SceneManager : MonoBehaviour {

	List<GameObject> Scenes;
	public Light headLight;
	public Material TheVoidMaterial;

	// Use this for initialization
	void Awake () { // Awake is called before Start, so we know this has been done when UIManager calls us from its Start()
		Scenes = new List<GameObject>();

		foreach(Transform child in transform)
		{
			SceneInfo si = child.GetComponent<SceneInfo> ();
			if (si.use)
				Scenes.Add (child.gameObject);
			else
				child.gameObject.SetActive (false); // make sure unused scenes are off
		}
	}

	public int GetNoScenes() {
		return Scenes.Count;
	}

	public string GetSceneName(int SceneNo) {
		return Scenes [SceneNo].GetComponent<SceneInfo> ().SceneName;
	}

	public void SetActiveScene(int SceneNo) {
		for (int i = 0; i < Scenes.Count; i++) {
			SceneInfo si = Scenes [i].GetComponent<SceneInfo> ();
			if (i == SceneNo) {
				Scenes [i].SetActive (true);
				RenderSettings.ambientLight = si.ambientLight;
				headLight.gameObject.SetActive (si.headLight);
			} else {
				Scenes [i].SetActive (false);
			}
		}
	}

	// kinda hacky, just sets the brightness of one scene: "the void"
	public void TheVoidSetBrightness( float value ) {
		// 0.3 is default, means full ambient and no emit

		if (value > 0.3f) {
			RenderSettings.ambientLight = Color.white;
			Scenes [1].GetComponent<SceneInfo> ().ambientLight = Color.white; // store in the scene
			float b = (value - 0.3f) / 0.7f;
			Color col = new Color (b, b, b, 1f);
			TheVoidMaterial.SetColor ("_EmissionColor", col);
		} else {
			TheVoidMaterial.SetColor ("_EmissionColor", Color.black);
			float b = value / 0.3f;
			Color col = new Color (b, b, b, 1f);
			RenderSettings.ambientLight = col;
			Scenes [1].GetComponent<SceneInfo> ().ambientLight = col; // store in the scene
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}