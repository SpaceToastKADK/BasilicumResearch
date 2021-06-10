using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;

public class showSurveyLink : MonoBehaviour {

  [DllImport("__Internal")]
	private static extern void makeSurveyLink(int linkCode);

	public void fireSurveyLinkScript(int linkCode) {
		#if !UNITY_EDITOR
		  makeSurveyLink(linkCode);
		#endif
	}
}
