using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class introLogic : MonoBehaviour
{
  public float countdown = 5f;
  public float checkAt = 5f;
  public float checks = 0;
  public float checkRatioTotal;
  GameObject playButton;
  dataHolder dHolder;

  // Start is called before the first frame update
  void Start(){
    dHolder = GameObject.Find("DataHolder").GetComponent<dataHolder>();
    playButton = GameObject.Find("Play");
    playButton.SetActive(false);
  }

  // Update is called once per frame
  void Update(){
    countdown-=Time.deltaTime;
    if (countdown<checkAt){
      checkAt-=.3f;
      checks+=1f;
      if (Screen.currentResolution.refreshRate>=60){
        checkRatioTotal+=1f;
      }
      if (checkRatioTotal/checks>.8f) dHolder.fpsCheck=true;
    }
    if (countdown<0){
      playButton.SetActive(true);
    }
  }
}
