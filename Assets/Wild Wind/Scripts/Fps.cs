using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;

public class Fps : MonoBehaviour 
{

    string label = "";
	float count;
	List<float> times = new List<float>();

	private void Update()
    {

		times.Add(Time.deltaTime);
		float sum = 0;

		if (times.Count > 60)
		{

			sum -= times[0];
			times.RemoveAt(0);

		}

		foreach (float a in times)
			sum += a;

		label = "FPS :" + times.Count / sum;

	}

    void OnGUI ()
	{
		GUI.Label (new Rect (5, 40, 100, 25), label);
	}

}
