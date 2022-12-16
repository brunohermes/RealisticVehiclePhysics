using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Vehicle))]

public class VehicleEditor : Editor
{
   public Texture banner, banner_gameobjs;

 
 public override void OnInspectorGUI(){
   Vehicle vehicleScript = (Vehicle)target;

   EditorGUIUtility.LookLikeControls(300,50);

   GUI.DrawTexture(new Rect(10, 0, 350, 100), banner, ScaleMode.StretchToFill, false, 10.0f);
   GUILayout.Space(125);
     
   
   GUILayout.Label("Vehicle Handling", EditorStyles.boldLabel);
   GUILayout.Space(25);
   
   EditorGUILayout.FloatField("Steering Angle: ", vehicleScript.steeringAngle);
   EditorGUILayout.FloatField("Brake Torque (N): ", vehicleScript.brakeTorque);
   EditorGUILayout.FloatField("Max. Speed (Km/h): ", vehicleScript.maxSpeed);
   EditorGUILayout.FloatField("Max. Torque (N): ", vehicleScript.maxTorque);
   EditorGUILayout.IntField("Number of Gears: ", vehicleScript.gearsNo);
   GUILayout.Space(25);
   GUI.DrawTexture(new Rect(10, 300, 350, 50), banner_gameobjs, ScaleMode.StretchToFill, true, 10.0f);
   GUILayout.Space(100);
   EditorGUILayout.IntField("Number of Gears: ", vehicleScript.gearsNo);

   // EditorGUILayout.HelpBox("Set how much your vehicle will Steer", MessageType.Info);
  
 }   

}
