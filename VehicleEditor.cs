using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Vehicle))]
public class VehicleEditor : Editor
{
 public override void OnInspectorGUI(){
    Vehicle vehicleScript = (Vehicle)target;
    EditorGUILayout.FloatField("Steering Angle", vehicleScript.steeringAngle);
    EditorGUILayout.HelpBox("Set how much your vehicle will Steer", MessageType.Info);
 
 
 }   
}
