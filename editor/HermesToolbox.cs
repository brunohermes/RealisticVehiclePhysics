using UnityEngine;
using UnityEditor;
using System.Collections;

class HermesToolBox : EditorWindow {
	private int axlesCount = 2;
	private float mass = 1000;
	private float axleStep = 2;
	private float axleWidth = 2;
	private float axleShift = -0.5f;
	private string vName;

 	
	[MenuItem ("Hermes Toolbox/Vehicle Physics/New Vehicle")]
	public static void  nVehicleWindow() {
		EditorWindow.GetWindow(typeof(HermesToolBox));
	}

	void OnGUI () {
		vName = EditorGUILayout.TextField ("Vehicle Name: ", vName);
		axlesCount = EditorGUILayout.IntSlider ("Number of Axles: ", axlesCount, 2, 10);
		mass = EditorGUILayout.FloatField ("Mass (kg): ", mass);
		axleStep = EditorGUILayout.FloatField ("Axle Step: ", axleStep);
		axleWidth = EditorGUILayout.FloatField ("Axle Width: ", axleWidth);
		axleShift = EditorGUILayout.FloatField ("Axle Shift (Height): ", axleShift);

		if (GUILayout.Button("Set new Vehicle")) {
			CreateCar ();
		}
	}

	void CreateCar()
	{
		var root = new GameObject (vName);
		var rootBody = root.AddComponent<Rigidbody> ();
		rootBody.mass = mass;
		
		var body = GameObject.CreatePrimitive (PrimitiveType.Cube);
		body.transform.parent = root.transform;

		float length = (axlesCount - 1) * axleStep;
		float firstOffset = length / 2;

		body.transform.localScale = new Vector3(axleWidth, 1, length);

		for (int i = 0; i < axlesCount; ++i) 
		{
			var leftWheel = new GameObject (string.Format("a{0}l", i));
			var rightWheel = new GameObject (string.Format("a{0}r", i));

			leftWheel.AddComponent<WheelCollider> ();
			rightWheel.AddComponent<WheelCollider> ();

			leftWheel.transform.parent = root.transform;
			rightWheel.transform.parent = root.transform;

			leftWheel.transform.localPosition = new Vector3 (-axleWidth / 2, axleShift, firstOffset - axleStep * i);
			rightWheel.transform.localPosition = new Vector3 (axleWidth / 2, axleShift, firstOffset - axleStep * i);
		}

		root.AddComponent<Suspension>();
		root.AddComponent<Vehicle>();
	}

	[MenuItem("Hermes Toolbox/About")]
	
	static void About(MenuCommand command)
    {
		EditorWindow.GetWindow(typeof(About));
	}
}
class About : EditorWindow {
	// private string about = "Ver. 0.1a!";
}