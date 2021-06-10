using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainLogic : MonoBehaviour
{
  public int testMode;
  public float minutes;
  public float danger1Meters;
  public float danger2Meters;
  public float sqrDist = 999;
  public int dangerLevel = -1;
  public float refresh;
  public int vSync;
  public bool won = false;
  Camera mainCam;
  GameObject[] ribbons;
  GameObject[] postVolumes;
  MotionBlur blur;
  PostProcessVolume tempVolume;
  public float freezeNextDrawTime = 0f;
  public float freezeDelay = .083333f;
  int defaultLayerMask;
  Text fpsText;
  float fpsDisplayUpdate = 0;
  dataHolder dHolder;

  // Start is called before the first frame update
  void Start(){
    Cursor.visible = false;
    minutes*=60f;
    dHolder = GameObject.Find("DataHolder").GetComponent<dataHolder>();
    mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
    defaultLayerMask = mainCam.cullingMask;
    ribbons = GameObject.FindGameObjectsWithTag("ribbons");
    postVolumes = GameObject.FindGameObjectsWithTag("postVolume");
    refresh = Screen.currentResolution.refreshRate;
    fpsText = GameObject.Find("FPS Text").GetComponent<Text>();
    danger1Meters *= danger1Meters;
    danger2Meters *= danger2Meters;
    if (testMode==7 || testMode==8 || testMode==10){
      blur = ScriptableObject.CreateInstance<MotionBlur>();
      blur.enabled.Override(true);
      blur.shutterAngle.Override(240);
      tempVolume = PostProcessManager.instance.QuickVolume(0, 5f, blur);
    }
    foreach (GameObject ribbon in ribbons){
      ribbon.GetComponent<Cloth>().clothSolverFrequency = refresh*4f;
    }
    setDangerLevel(0);
    dHolder.lastReset = Time.time;
  }

  // Update is called once per frame
  void Update(){
    dHolder.playTime+=Time.deltaTime;
    if (dHolder.playTime>minutes && dHolder.surveyReady==false) surveyReady();
    if (Time.time > fpsDisplayUpdate){
      fpsDisplayUpdate = Time.time+.5f;
      //fpsText.text = "FPS ("+refresh+") "+(1/Time.smoothDeltaTime)/vSync;
    }
    if (won==true) Cursor.visible = true;
    if (sqrDist>danger1Meters) {
      setDangerLevel(0);
    } else if (sqrDist<danger2Meters){
      setDangerLevel(2);
    } else {
      setDangerLevel(1);
    }
    if (testMode==3) freezeFrameUpdate();
    if (testMode==4 && dangerLevel>0) gradeFPS();
    if (sqrDist<.6f) returnToStart();
  }

  public void setDangerLevel(int newLevel){
    if (newLevel==dangerLevel) return;
    dangerLevel = newLevel;
    if (testMode==1) changeFPS();
    if (testMode==2) changeVSync();
    if (testMode==3) changeFreezeFrame();
    if (testMode==4) changeGradedFPS();
    if (testMode==5) changeProportionalFPS();
    if (testMode==6) rawNoSync();
    if (testMode==7) changeVSyncAndBlur();
    if (testMode==8) changeVSyncAndBlurSharper();
    if (testMode==9) changeFPS2Step();
    if (testMode==10) changeFPS2StepWBlur();
  }

  public void changeFPS(){ //Mode 1
    QualitySettings.vSyncCount = 0;
    int fps = 24; //dangerLevel 0
    if (dangerLevel==1) fps = 48;
    if (dangerLevel==2) fps = 60;
    Application.targetFrameRate = fps;
    foreach (GameObject ribbon in ribbons){
      ribbon.GetComponent<Cloth>().clothSolverFrequency = fps*4f;
    }
  }

  public void changeVSync(){ //Mode 2
    int newSync = 3; //dangerLevel 0
    if (dangerLevel==1) newSync = 2;
    if (dangerLevel==2) newSync = 1;
    QualitySettings.vSyncCount = newSync;
    vSync = newSync;
    foreach (GameObject ribbon in ribbons){
      ribbon.GetComponent<Cloth>().clothSolverFrequency = (refresh/newSync)*4f;
    }
  }

  public void changeFreezeFrame(){ //Mode 3
    QualitySettings.vSyncCount = 1;
    freezeDelay = 1f/24f; //dangerLevel 0
    if (dangerLevel==1) freezeDelay = 1f/60f;
    if (dangerLevel==2) freezeDelay = 1f/60f;
  }

  void freezeFrameUpdate(){
    if (Time.fixedTime>=freezeNextDrawTime){
      mainCam.clearFlags = CameraClearFlags.Skybox;
      mainCam.cullingMask = defaultLayerMask;
      freezeNextDrawTime = Time.fixedTime + freezeDelay;
      foreach (GameObject vol in postVolumes){
        vol.GetComponent<PostProcessVolume>().enabled = true;
      }
    } else {
      mainCam.clearFlags = CameraClearFlags.Nothing;
      mainCam.cullingMask = 0;
      foreach (GameObject vol in postVolumes){
        vol.GetComponent<PostProcessVolume>().enabled = false;
      }
    }
  }

  public void changeGradedFPS(){ //Mode 4
    QualitySettings.vSyncCount = 0;
    if (dangerLevel==0){
      int fps = 24; //dangerLevel 0
      Application.targetFrameRate = fps;
      foreach (GameObject ribbon in ribbons){
        ribbon.GetComponent<Cloth>().clothSolverFrequency = fps*4f;
      }
    }
  }

  void gradeFPS(){
    float gradedFPS = (danger1Meters-sqrDist)/(danger1Meters-danger2Meters);
    int fps = 24 + Mathf.RoundToInt(gradedFPS*36f);
    Application.targetFrameRate = fps;
    foreach (GameObject ribbon in ribbons){
      ribbon.GetComponent<Cloth>().clothSolverFrequency = fps*4f;
    }
  }

  public void changeProportionalFPS(){ //Mode 5
    QualitySettings.vSyncCount = 0;
    int fps = 24;
    if (Mathf.Approximately(refresh, 60f)){
      if (dangerLevel==0) fps = 20; //dangerLevel 0
      if (dangerLevel==1) fps = 40;
      if (dangerLevel==2) fps = 60;
    }
    if (Mathf.Approximately(refresh, 72f)){
      if (dangerLevel==0) fps = 20; //dangerLevel 0
      if (dangerLevel==1) fps = 40;
      if (dangerLevel==2) fps = 60;
    }
    if (Mathf.Approximately(refresh, 90f)){
      if (dangerLevel==0) fps = 30; //dangerLevel 0
      if (dangerLevel==1) fps = 60;
      if (dangerLevel==2) fps = 90;
    }
    if (refresh>119f){
      if (dangerLevel==0) fps = 30; //dangerLevel 0
      if (dangerLevel==1) fps = 60;
      if (dangerLevel==2) fps = 120;
    }
    Application.targetFrameRate = fps;
    foreach (GameObject ribbon in ribbons){
      ribbon.GetComponent<Cloth>().clothSolverFrequency = fps*4f;
    }
  }

  public void rawNoSync(){ //Mode 6
    QualitySettings.vSyncCount = 0;
    Application.targetFrameRate = Mathf.RoundToInt(refresh);
  }

  public void changeVSyncAndBlur(){ //Mode 7
    int newSync = 3; //dangerLevel 0
    int sAngle = 360;
    if (dangerLevel==1) {
      newSync = 2;
      sAngle = 240;
    }
    if (dangerLevel==2) {
      newSync = 1;
      sAngle = 120;
    }
    QualitySettings.vSyncCount = newSync;
    vSync = newSync;
    blur.shutterAngle.value = sAngle;
    foreach (GameObject ribbon in ribbons){
      ribbon.GetComponent<Cloth>().clothSolverFrequency = (refresh/newSync)*4f;
    }
  }

  public void changeVSyncAndBlurSharper(){ //Mode 8
    int newSync = 3; //dangerLevel 0
    int sAngle = 360;
    if (dangerLevel>0) { //dangerlevel 1 & 2
      newSync = 1;
      sAngle = 120;
    }
    QualitySettings.vSyncCount = newSync;
    vSync = newSync;
    blur.shutterAngle.value = sAngle;
    foreach (GameObject ribbon in ribbons){
      ribbon.GetComponent<Cloth>().clothSolverFrequency = (refresh/newSync)*4f;
    }
  }

  public void changeFPS2Step(){ //Mode 9
    QualitySettings.vSyncCount = 0;
    int fps = 24; //dangerLevel 0
    if (dangerLevel>0) fps = Mathf.RoundToInt(refresh);
    Application.targetFrameRate = fps;
    foreach (GameObject ribbon in ribbons){
      ribbon.GetComponent<Cloth>().clothSolverFrequency = fps*8f;
    }
  }

  public void changeFPS2StepWBlur(){ //Mode 10
    QualitySettings.vSyncCount = 0;
    int fps = 24; //dangerLevel 0
    int sAngle = 240;
    if (dangerLevel>0) {
      fps = Mathf.RoundToInt(refresh);
      sAngle*=24/Mathf.RoundToInt(refresh);
    }
    Application.targetFrameRate = fps;
    blur.shutterAngle.value = sAngle;
    foreach (GameObject ribbon in ribbons){
      ribbon.GetComponent<Cloth>().clothSolverFrequency = fps*8f;
    }
  }

  public void surveyReady(){
    dHolder.surveyReady = true;
    string a = "";
    int b = 0;
    for (int c=0; c<6; c++){
      int d = Mathf.FloorToInt(Random.value*9.999f);
      a += d.ToString();
      b += d;
    }
    if (dHolder.fpsCheck==true){
      int d = Mathf.FloorToInt(Random.value*8.999f);
      a += d.ToString();
      b += d;
    } else {
      a += "9";
      b += 9;
    }
    a = (b%10).ToString()+a;
    a = Mathf.FloorToInt(1f+Random.value*8.999f).ToString()+a;
    dHolder.surveyUrl = a;
    gameObject.GetComponent<showSurveyLink>().fireSurveyLinkScript(int.Parse(a));
  }

  public void returnToStart(){
    SceneManager.LoadScene("Gameplay");
  }

}
