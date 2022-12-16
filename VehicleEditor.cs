using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Vehicle))]
public class VehicleEditor : Editor
{
   public Texture logo;
 
 public override void OnInspectorGUI(){
  
   GUI.DrawTexture(new Rect(10, 0, 100, 100), logo, ScaleMode.StretchToFill, true, 10.0f);
   GUILayout.Space(120);
   EditorGUILayout.LabelField("Movement Settings", EditorStyles.boldLabel);
   GUILayout.Space(50);
   Vehicle vehicleScript = (Vehicle)target;
   EditorGUILayout.FloatField("Steering Angle", vehicleScript.steeringAngle);
   EditorGUILayout.HelpBox("Set how much your vehicle will Steer", MessageType.Info);
  
 }   

}
