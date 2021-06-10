using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeBasilicumInvisible : MonoBehaviour
{
  GameObject basilicum;
  ParticleSystem poof;
  public List<Material> sharedMats = new List<Material>();

  void Start(){
    basilicum = GameObject.Find("Basilicum");
    poof = basilicum.GetComponent<ParticleSystem>();
    foreach (SkinnedMeshRenderer smr in basilicum.GetComponentsInChildren(typeof(SkinnedMeshRenderer))){
      foreach (Material mat in smr.sharedMaterials){
        sharedMats.Add(mat);
      }
    }
  }

  void OnTriggerEnter(Collider body){
    if (body.gameObject.tag=="Player"){
      if (body.gameObject.GetComponent<isVisible>().visible==true) {
        poof.Play();
        foreach (Material mat in sharedMats){
          mat.SetColor("_Color", new Color(mat.color.r, mat.color.g, mat.color.b, .5f));
        }
      }
      body.gameObject.GetComponent<isVisible>().visible = false;
    }
  }

  void OnTriggerExit(Collider body){
    if (body.gameObject.tag=="Player"){
      if (body.gameObject.GetComponent<isVisible>().visible==false) {
        foreach (Material mat in sharedMats){
          mat.SetColor("_Color", new Color(mat.color.r, mat.color.g, mat.color.b, 1f));
        }
      }
      body.gameObject.GetComponent<isVisible>().visible = true;
    }
  }
}
