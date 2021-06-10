using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ladyController : MonoBehaviour
{
  UnityEngine.AI.NavMeshAgent navMeshController;
  Animator animationController;
  GameObject basilicum;
  float rethinkTimer = 0;
  float sqrMaxDist = 10f;
  mainLogic gameLogic;

  void Start(){
    navMeshController = GetComponent<UnityEngine.AI.NavMeshAgent>();
    animationController = GetComponent<Animator>();
    basilicum = GameObject.Find("Basilicum");
    sqrMaxDist *= sqrMaxDist;
    gameLogic = GameObject.Find("Game Logic").GetComponent<mainLogic>();
  }

  void Update(){
    //won?
    if (gameLogic.won==true){
      animationController.SetInteger("animState",0);
      navMeshController.destination = transform.position;
      return;
    }
    if (navMeshController.velocity.sqrMagnitude>.2){
      animationController.SetInteger("animState",1);
    } else {
      animationController.SetInteger("animState",0);
    }
    if ((basilicum.transform.position-transform.position).sqrMagnitude<2f) {
      animationController.SetInteger("animState",2);
    }
    rethinkTimer-=Time.deltaTime;
    if (rethinkTimer<0){
      rethinkTimer = .5f+(Random.value*.5f);
      if (basilicum.GetComponent<isVisible>().visible==true){
        if ((transform.position-basilicum.transform.position).sqrMagnitude<sqrMaxDist){
          navMeshController.destination = basilicum.transform.position;
        }
      } else if (Random.value<.2f){
        navMeshController.destination = new Vector3(transform.position.x+(3f*(Random.value-.5f)), transform.position.y, transform.position.z+(3f*(Random.value-.5f)));
      }
    }
  }
}
