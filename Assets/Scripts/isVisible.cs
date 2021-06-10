using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isVisible : MonoBehaviour
{
  public bool visible;
  public List<SkinnedMeshRenderer> smRenderers = new List<SkinnedMeshRenderer>();
  ParticleSystem poof;

  void Start(){
    foreach (SkinnedMeshRenderer smr in gameObject.GetComponentsInChildren(typeof(SkinnedMeshRenderer))){
      smRenderers.Add(smr);
    }
    poof = GetComponent<ParticleSystem>();
  }

  public void makeVisible(){
    if (visible==true) return;
    visible = true;
    foreach(SkinnedMeshRenderer smr in smRenderers){
      smr.enabled = true;
    }
  }

  public void makeInvisible(){
    if (visible==false) return;
    visible = false;
    foreach(SkinnedMeshRenderer smr in smRenderers){
      smr.enabled = false;
    }
    poof.Play();
  }


}
