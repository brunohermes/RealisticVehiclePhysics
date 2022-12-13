using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class Vehicle : MonoBehaviour
{
    private WheelCollider[] wheels;
    
    #region Engine Mechanics
	 
	[Header("Maneuverability and Brakes")]
	[Space(15)]
	public float steeringAngle = 30;
	public float brakeTorque = 1200;
	public GameObject handbrakeDection;
	private bool handBrakeActive;
	[Space(5)]
	[Header("_______________________________________________")]
	[Space(15)]
	//Engine Performance Setup

	[Header("\u03C4" + "              Engine - Performance")]
	[Space(15)]
	public float maxSpeed = 100;
	public float maxTorque = 750;
	public int gearsNo;
	public float []gearBox;
	[Space(5)]
	[Header("_______________________________________________")]
	[Space(15)]

	[Header("\u25EF" + "              Wheels")]
	[Space(15)]
	public GameObject wheelMesh;
	public GameObject tractionController;
	//Rpm and Km/h System
	private float wRpm, rpmG, speedR;
	private int speedKh;
	//Transmission System
	private bool reverseGear;
	[Space(5)]
	[Header("_______________________________________________")]
	[Space(15)]
	#endregion

	#region User interface
	[Header("\u20E3" + "              User Interface")]
	[Space(15)]
	//User interface
	public TextMeshProUGUI kphUi;
	public TextMeshProUGUI rpmUi;
	public Slider speedometerUi;
	[Space(10)]
	#endregion

	#region Effects
	[Header("\u2749" + "              Particles")]
	[Space(5)]
	[Space(15)]
	//Particle System for trail and braking
	public ParticleSystem[] handbrakeParticles;
	public ParticleSystem[] movementParticles;
	[Space(5)]
	[Header("_______________________________________________")]
	[Space(15)]
	#endregion
	
	#region Audio
	[Header("\u266B" + "               Audio")]
	[Space(5)]
	[Space(15)]
	public float pitchVal = 1f;
	public AudioClip engSound;
	private AudioSource audioSource;
	private float idleGear = 1f;
	private float firstGear = 1.18f;
	private float secondGear = 1.36f;
	private float thirdGear = 1.45f;
	private float fourthGear = 1.56f;
	[Space(5)]
	[Header("_______________________________________________")]
	[Space(15)]
	#endregion

	#region Electric
	[Header("\u2600" + "              Electric")]
	[Space(15)]
	
	public Renderer []brakeLamps;
	public Renderer []headlamps;
	public GameObject headlightMesh;
	/** Vehicle Lights **/
	[Space(5)]
	[Header("Material")]
	[Space(5)]
	public Material reverseOn, reverseOff;
	public Material brakeLightOn, brakeLightOff;
	public Material headlightOn, headlightOff; //Headlights On/Off Material Slot	
	public bool headlights;	//Headlights Toggler
	#endregion
 

	#region Awake
	void Awake(){
		audioSource = GetComponent<AudioSource>();
		audioSource.clip = engSound;
		audioSource.loop = true;	
	}
	#endregion

	
	public void Start()
	{
	 
	
		//Wheel creation
		wheels = GetComponentsInChildren<WheelCollider>();
		
		for (int i = 0; i < wheels.Length; ++i) 
		{
			var wheel = wheels [i];
			
			// Creates Wheel Shape
			if (wheelMesh != null)
			{
				var ws = GameObject.Instantiate (wheelMesh);
				ws.transform.parent = wheel.transform;
		
				if(wheel.transform.localPosition.x > 0f){
					ws.transform.localScale = new Vector3(ws.transform.localScale.x * -1f,ws.transform.localScale.y,ws.transform.localScale.z);
				}
			}
		}
		
		
	}
	
	
	
	public void Update()
	{
		
		//Headlights
		if(Input.GetKeyDown(KeyCode.L) && headlights == false){
			for(int i = 0; i < headlamps.Length; i++){
				headlamps[i].material = headlightOn;
			}
			headlightMesh.SetActive(true);
			headlights = true;
		}else if(Input.GetKeyDown(KeyCode.L) && headlights == true){
			for(int i = 0; i < headlamps.Length; i++){
				headlamps[i].material = headlightOff;
			}
			headlightMesh.SetActive(false);
			headlights = false;
		}
		



		
		/** HANDBRAKE DETECTION AND PARTICLE SYSTEM **/
		if(Input.GetKeyDown(KeyCode.Space) && handBrakeActive == false){
			handBrakeActive = true;
			for(int i = 0; i < handbrakeParticles.Length; i++){
				handbrakeParticles[i].time = 1f;
				handbrakeParticles[i].Play();
			}
		}else if(Input.GetKeyDown(KeyCode.Space) && handBrakeActive == true){
			handBrakeActive = false;
		}
		



		float angle = steeringAngle * Input.GetAxis("Horizontal");
		float torque = maxTorque * Input.GetAxis("Vertical") / 2;
		float torqueT = maxTorque * Input.GetAxis("Vertical");
		bool handbrake = handBrakeActive;

		#region General Wheel Collider Structure
		foreach (WheelCollider wheel in wheels)
		{
			if(handbrake){
				wheel.brakeTorque = 750;
			}else{
				wheel.brakeTorque = 0;
			}
			

			/** STEERING AND 4X4 TRACTION SYSTEM **/
			if (wheel.transform.localPosition.z > 0){
				wheel.steerAngle = angle;
			if(tractionController.activeSelf){
				wheel.motorTorque = torque;
			} else{
				wheel.motorTorque = 0;
			}
			}
				

			#region Engine and Rpm
			if (wheel.transform.localPosition.z < 0){
				
				//RPM TO KMPH
				wRpm = wheel.rpm;
				var rpt = (wRpm / 4) * 100;
				var multip = 3.6;
				var rpmF = Mathf.RoundToInt(wRpm);
				var realRpm = rpmF * multip;
				var radius = 37;
				var constkph = 0.001885;

				// rpmG = rpmF;

				// print("KM/H: " + realRpm * radius * constkph + "Wheel RPM: " + realRpm  );

				var kph = realRpm * radius * constkph;
				var fSpeed = Convert.ToInt32(kph);

				speedKh = Mathf.Abs(fSpeed);
				
				if(rpmF < 25f){
					reverseGear = true;
				}else{ 
					reverseGear = false;
				}
				
				if(speedKh >= maxSpeed - 10 || speedKh > maxSpeed){
					speedR = maxSpeed;
				}else{
					speedR = speedKh;
				}
 
				kphUi.text = "Km/h:" + speedKh.ToString();

				speedometerUi.value = speedKh;

				//Particles
				if(speedKh > 3){
					for(int i = 0; i < movementParticles.Length; i++){
					movementParticles[i].Play();
					}
				}else{
					for(int i = 0; i < movementParticles.Length; i++){
					movementParticles[i].Stop();
					}
				}

				//Transmission system
				for(int i = 0;speedKh > gearBox[i] && i < gearsNo; i++){
					if(speedKh > 0){
						switch(i){
						case 0:
						// print("1st gear");
						 
						pitchVal = firstGear;
						audioSource.pitch = pitchVal;

						break;
						case 1:
						// print("2nd gear");
						 
						pitchVal = secondGear;
						audioSource.pitch = pitchVal;

						break;

						case 2:
						// print("3rd gear");
						 
						pitchVal = thirdGear;
						audioSource.pitch = pitchVal;

						break;

						case 3:
						// print("4th gear");
						 
						pitchVal = fourthGear;
						audioSource.pitch = pitchVal;

						break;

						case 4:
						// print("5th gear");
						 
						
						break;
						default: 
							// print("Neutral");
							 
							pitchVal = idleGear;
							audioSource.pitch = pitchVal;
						break;
					}
						
					}else{
						pitchVal = idleGear;
						audioSource.pitch = pitchVal;
					}
				}
			//Sets the max speed
				if(speedKh < maxSpeed){
					torqueT = maxTorque * Input.GetAxis("Vertical");
				}else{
					torqueT = maxTorque * -(Input.GetAxis("Vertical")) * Time.deltaTime;
				}
				wheel.motorTorque = torqueT;
			}
			#endregion

			#region BrakeSystem and Reverse Actuator
			//Brake SYSTEM And Reverse Actuator
			if(!reverseGear){
				if(Input.GetAxis("Vertical") < 0){
					wheel.brakeTorque = 850;
					for(int i = 0; i < brakeLamps.Length; i++){
					brakeLamps[i].material = brakeLightOn;
						 
					}
				}else{
					for(int i = 0; i < brakeLamps.Length; i++){
						brakeLamps[i].material = brakeLightOff;
						 
					}
				}
			}else{ 
				if(Input.GetAxis("Vertical") < 0){
				 
						var reverseSpeed = 30;
						 
						 
						if(speedKh > reverseSpeed){
							wheel.brakeTorque = 850;
							speedKh = 30;
						 
					}
				}else{
					 
				}
			}
			#endregion

			

			#region WheelShapeUpdater 
			//Updates Wheel Shape looks while moving
			if (wheelMesh) 
			{
				Quaternion q;
				Vector3 p;
				wheel.GetWorldPose (out p, out q);

				/** Wheel shape get child check **/
				Transform shapeTransform = wheel.transform.GetChild (0);
				shapeTransform.position = p;
				shapeTransform.rotation = q;
			}
			#endregion

		}
		#endregion


	}
	
}