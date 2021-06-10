using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeLadiesInvisible : MonoBehaviour
{

  void OnTriggerEnter(Collider body){
    if (body.gameObject.tag=="lady"){
      body.gameObject.GetComponent<isVisible>().makeInvisible();
    }
  }

  void OnTriggerExit(Collider body){
    if (body.gameObject.tag=="lady"){
      body.gameObject.GetComponent<isVisible>().makeVisible();
    }
  }
}
