using UnityEngine;
using System.Collections;

public static class ShuffleArray {

	public static void Shuffle (Object[] objsToChange)
	{
		Object[] newObjs = new Object[objsToChange.Length];
		bool[] pulled = new bool[objsToChange.Length];
		for (int j = 0; j < pulled.Length; j++) {
			pulled [j] = true;
		}
		int i = 0;
		while (i < objsToChange.Length) {
			int x = Random.Range (0, objsToChange.Length);
			if (pulled [x] == true) {
				newObjs [i] = objsToChange [x];
				pulled [x] = false;
				i ++;
			}	
		}
		newObjs.CopyTo (objsToChange, 0);
		for (int j = 0; j < pulled.Length; j++) {
			pulled [j] = true;
		}
	}

	public static void Shuffle (Vector3[] objsToChange)
	{
		Vector3[] newObjs = new Vector3[objsToChange.Length];
		bool[] pulled = new bool[objsToChange.Length];
		for (int j = 0; j < pulled.Length; j++) {
			pulled [j] = true;
		}
		int i = 0;
		while (i < objsToChange.Length) {
			int x = Random.Range (0, objsToChange.Length);
			if (pulled [x] == true) {
				newObjs [i] = objsToChange [x];
				pulled [x] = false;
				i ++;
			}	
		}
		newObjs.CopyTo (objsToChange, 0);
		for (int j = 0; j < pulled.Length; j++) {
			pulled [j] = true;
		}
	}
}
