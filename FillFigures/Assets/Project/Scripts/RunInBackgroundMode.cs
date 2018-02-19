using UnityEngine;

public class RunInBackgroundMode : MonoBehaviour {

	void Start ()
	{
		Application.runInBackground = true;
	}
}