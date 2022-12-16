using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Suspension))]

public class SuspensionEditor : Editor
{
   public Texture banner;
   
 
 public override void OnInspectorGUI(){
   Suspension suspensionScript = (Suspension)target;

   EditorGUIUtility.LookLikeControls(300,50);

   GUI.DrawTexture(new Rect(10, 0, 350, 30), banner, ScaleMode.StretchToFill, false, 10.0f);
   GUILayout.Space(30);
   EditorGUILayout.HelpBox("Settings for Spring Stiffness.", MessageType.Info);
   GUILayout.Space(25);
    
   EditorGUILayout.FloatField("Natural Frequency: ", suspensionScript.naturalFrequency);
   GUILayout.Space(5);
   EditorGUILayout.FloatField("Damping Ratio: ", suspensionScript.dampingRatio);
   GUILayout.Space(5);
   EditorGUILayout.FloatField("Force Shift: ", suspensionScript.forceShift);
   GUILayout.Space(25);
 


   // EditorGUILayout.HelpBox("Set how much your vehicle will Steer", MessageType.Info);
  
 }   

}
