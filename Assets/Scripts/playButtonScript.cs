using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playButtonScript : MonoBehaviour
{
    void Start()
    {
      DontDestroyOnLoad(GameObject.Find("DataHolder"));
      //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
      gameObject.GetComponent<Button>().onClick.AddListener(nextScene);
    }

    void nextScene()
    {
      SceneManager.LoadScene("Gameplay");
    }
}
