using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Vehicle : MonoBehaviour
{
    #region Primitives
    private WheelCollider[] wheels;  
    [Header("Vehicle and Trailer Management")]
	[Space(10)]
    public bool engineOn;
    public bool trailerAttached;
    #endregion

    #region Maneuverability
    [Header("Maneuverability and Brakes")]
	[Space(5)]
    public float steeringAngle = 30;
    public float brakeTorque = 1200;
    public bool handBrakeActive;
    public GameObject handbrakeDection;
    #endregion
  
    #region Engine Performance
    [Header("Maneuverability and Brakes")]
	[Space(5)]
    public float maxSpeed = 100;
	public float maxTorque = 750;
	public int gearsNo;
	public float []gearBox;
    #endregion

    #region Wheels
    [Header("Wheels")]
	[Space(5)]
    public GameObject wheelMesh;
	public GameObject tractionController;
    private float wRpm, rpmG, speedR;
	private int speedKh;
    private bool reverseGear;
    #endregion

    void OnTriggerStay(Collider other){
        if(other.tag == "Trailer"){
            trailerAttached = true;
        }
    }

    void Awake(){

    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
