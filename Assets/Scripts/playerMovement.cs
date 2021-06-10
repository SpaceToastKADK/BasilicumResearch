using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]

public class playerMovement : MonoBehaviour
{
  public float walkingSpeed;
  public float gravity;
  CharacterController characterController;
  Animator animationController;
  Vector3 moveDirection = Vector3.zero;
  Vector3 stylusDefault = new Vector3(5f,5f,5f);
  public Camera mainCam;
  public GameObject[] ladies;
  mainLogic gameLogic;
  Transform stylusHolder;
  Transform stylus;

  void Start(){
    characterController = GetComponent<CharacterController>();
    animationController = GetComponent<Animator>();
    ladies = GameObject.FindGameObjectsWithTag("lady");
    gameLogic = GameObject.Find("Game Logic").GetComponent<mainLogic>();
    stylusHolder = GameObject.Find("StylusHolder").GetComponent<Transform>();
    stylus = GameObject.Find("Stylus").GetComponent<Transform>();
  }

  void Update(){
    //won?
    if (gameLogic.won==true){
      animationController.SetInteger("animState",0);
      return;
    }
    //fell?
    if (transform.position.y<-7f){
      SceneManager.LoadScene("Gameplay");
      return;
    }
    //move
    float vMove = Input.GetAxis("Vertical");
    float hMove = Input.GetAxis("Horizontal");
    moveDirection = (mainCam.transform.forward * vMove) + (mainCam.transform.right * hMove);
    moveDirection *= walkingSpeed;
    characterController.Move(moveDirection * Time.deltaTime);
    if (moveDirection.sqrMagnitude>0){
      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * 5f);
    }
    if (moveDirection.sqrMagnitude>.1){
      animationController.SetInteger("animState",1);
    } else {
      animationController.SetInteger("animState",0);
    }
    if (characterController.isGrounded==false) {
      characterController.Move(new Vector3(0, -gravity, 0) * Time.deltaTime);
    }
    //nearest lady
    GameObject nearest = ladies[0];
    float sqrDist = (ladies[0].transform.position-transform.position).sqrMagnitude;
    foreach(GameObject lady in ladies){
      float newSqr = (lady.transform.position-transform.position).sqrMagnitude;
      if (newSqr<sqrDist){
        sqrDist = newSqr;
        nearest = lady;
      }
    }
    gameLogic.sqrDist = sqrDist;
    //move stylus
    stylusHolder.rotation = Quaternion.LookRotation(nearest.transform.position-transform.position, Vector3.up);
    /*if (gameLogic.dangerLevel==0){
      stylus.localScale = stylusDefault;
    } else {
      stylus.localScale = stylusDefault*Mathf.Clamp(2f-(transform.position.z/20f),1f,2f);
    }*/
  }
}
