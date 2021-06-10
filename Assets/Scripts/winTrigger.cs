using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class winTrigger : MonoBehaviour
{
  mainLogic gameLogic;
  Canvas winCanvas;
  Text winText;
  ParticleSystem treePoof;
  Light sun;
  float wonTime;
  float fadeToTime;
  Quaternion sunAngle;
  dataHolder dHolder;

  void Start(){
    gameLogic = GameObject.Find("Game Logic").GetComponent<mainLogic>();
    winCanvas = GameObject.Find("Win Canvas").GetComponent<Canvas>();
    winText = GameObject.Find("Win Text").GetComponent<Text>();
    treePoof = GameObject.Find("Tree").GetComponent<ParticleSystem>();
    sun = GameObject.Find("Sun").GetComponent<Light>();
    winCanvas.enabled = false;
    dHolder = GameObject.Find("DataHolder").GetComponent<dataHolder>();
  }

  void Update(){
    if (gameLogic.won == true){
      float fade = (Time.time-wonTime)/(fadeToTime-wonTime);
      sun.transform.rotation = Quaternion.Slerp(sunAngle, Quaternion.Euler(190, 0, 0), fade);
    }
  }

  void OnTriggerEnter(Collider body){
    if (body.gameObject.tag=="Player"){
      gameLogic.won = true;
      winCanvas.enabled = true;
      wonTime = Time.time;
      fadeToTime = wonTime+10f;
      sunAngle = sun.transform.rotation;
      float playTime = Time.time-dHolder.lastReset;
      float minutes = Mathf.Floor(playTime/60f);
      float seconds = Mathf.Floor(playTime - (minutes*60f));
      winText.text = minutes+":"+seconds;
      treePoof.Play();
    }
  }
}
