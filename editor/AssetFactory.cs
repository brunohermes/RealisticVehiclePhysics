using UnityEngine;
using UnityEditor;
using System.Collections;

class AssetFactory : EditorWindow {
	private int axlesCount = 2;
	private float mass = 1000;
	private float axleStep = 1.67f;
	private float axleWidth = 0.95f;
	private float axleShift = -0.5f;
	private string vName;

 	
	[MenuItem ("Asset Factory/Vehicle Physics/Generate")]
	public static void  nVehicleWindow() {
		EditorWindow.GetWindow(typeof(AssetFactory));
	}

	void OnGUI () {
		vName = EditorGUILayout.TextField ("Vehicle Name: ", vName);
		axlesCount = EditorGUILayout.IntSlider ("Axles (2 Wheels each): ", axlesCount, 1, 10);
		mass = EditorGUILayout.FloatField ("Mass (kg): ", mass);
		axleStep = EditorGUILayout.FloatField ("Axle Step (Vehicle Length): ", axleStep);
		axleWidth = EditorGUILayout.FloatField ("Axle Width (Vehicle Width): ", axleWidth);
		axleShift = EditorGUILayout.FloatField ("Axle Shift (Height): ", axleShift);

		if (GUILayout.Button("Generate vehicle physics")) {
			CreateCar ();
		}
	}

	void CreateCar()
	{
		var root = new GameObject (vName);
		var rootBody = root.AddComponent<Rigidbody> ();
		rootBody.mass = mass;
		
		var body = GameObject.CreatePrimitive (PrimitiveType.Cube);
		body.transform.position = new Vector3(0f, 0f, 0f);
		body.transform.localScale = new Vector3(0.5f, 0.5f, 2.65f);
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
			rightWheel.transform.localScale = new Vector3(0.1f, 0.7f, 0.5f);
			leftWheel.transform.localScale = new Vector3(0.1f, 0.7f, 0.5f);

			leftWheel.transform.parent = root.transform;
			rightWheel.transform.parent = root.transform;
			leftWheel.transform.localPosition = new Vector3 (-axleWidth / 2, axleShift, firstOffset - axleStep * i);
			rightWheel.transform.localPosition = new Vector3 (axleWidth / 2, axleShift, firstOffset - axleStep * i);
		}
		
		root.AddComponent<Suspension>();
		// root.AddComponent<FuelSystem>();
		root.AddComponent<Vehicle>();
	}

	// [MenuItem("Asset Factory/About")]
	
	// static void About(MenuCommand command)
    // {
	// 	EditorWindow.GetWindow(typeof(About));
	// }
}
class About : EditorWindow {
	// private string about = "Ver. 0.1a!";
}