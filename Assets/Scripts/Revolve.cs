using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolve : MonoBehaviour
{
  public float revolution;
  mainLogic gameLogic;

  void Start(){
    if (GameObject.Find("Game Logic")!=null){
      gameLogic = GameObject.Find("Game Logic").GetComponent<mainLogic>();
    }
  }

  // Update is called once per frame
  void Update(){
    if (gameLogic!=null){
      if (gameLogic.won==true) revolution*=.99f;
    }
    transform.Rotate(new Vector3(0,revolution*Time.deltaTime,0));
  }
}
