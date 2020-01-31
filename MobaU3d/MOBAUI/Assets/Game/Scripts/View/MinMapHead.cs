using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMapHead : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
	}
}
