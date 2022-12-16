using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using TMPro;

public class Vehicle : MonoBehaviour
{
    private WheelCollider[] wheels;
 
	public float steeringAngle = 30;
	public float brakeTorque = 1200;
	public GameObject handbrakeDection;
	private bool handBrakeActive;
 
	//Engine Performance Setup
 
	public float maxSpeed = 100;
	public float maxTorque = 750;
 
	public GameObject wheelMesh;
	public GameObject tractionController;
	//Rpm and Km/h System
	private float wRpm, rpmG, speedR;
	private int speedKh;
	//Transmission System
	private bool reverseGear;
 
	//User interface
	public TextMeshProUGUI kphUi;
	public TextMeshProUGUI rpmUi;
	public Slider speedometerUi;
 
	//Particle System for trail and braking
	public ParticleSystem[] handbrakeParticles;
	public ParticleSystem[] movementParticles;

	public ParticleSystem groundImpactParticles;
	public GameObject groundBoundaries;
 
	public float pitchVal = 1f;
	public AudioClip engSound;
	private AudioSource audioSource;
	private AudioSource shiftAudioS;
	public AudioClip shiftSound;
	private float pitchval;
   
	public Renderer []brakeLamps;
	public Renderer []headlamps;
	public Renderer []reverselamps;
	public Renderer []leftBlinkerLamps;
	public Renderer []rightBlinkerLamps;
	 
	public GameObject headlightsLights;
 
	public Material reverseOn, reverseOff;
	public Material brakeLightOn, brakeLightOff;
	public Material headlightOn, headlightOff; //Headlights On/Off Material Slot	
	public bool headlights;	//Headlights Toggler
	public GameObject exhaust;
	//Brake Disks

	public GameObject discoEsq, discoDir;
	public float angulo;
 
	//OPTIONS

	public bool vlights, spdmtr, vaudio, tctrl, impctsys;

	
 
 
	void Awake(){
		//DEFAULT OPTIONS

		impctsys = true; // Ground impact system, shows sparks when the player hits the ground too hard or scraches the vehicle.
	
		shiftAudioS = exhaust.GetComponent<AudioSource>();
		shiftAudioS.clip = shiftSound;
	
		audioSource = GetComponent<AudioSource>();
		audioSource.clip = engSound;
		audioSource.loop = true;	
	}

	//Ground impact detection system
	private void OnTriggerEnter(Collider groundBoundaries){
		if(impctsys){
			if(groundBoundaries.gameObject.CompareTag("Level")){
				groundImpactParticles.Play();
				shiftAudioS.Play();
			}
		}
	}
	
	public void Start()
	{
		
 		// groundImpactParticles
		// groundBoundaries
 
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



		//4x4
		if(Input.GetKeyDown(KeyCode.G)){
			if(!tractionController.activeSelf){
				tractionController.SetActive(true);
			}else if(tractionController.activeSelf){
				tractionController.SetActive(false);
			}
		}
 
		//Headlights
		if(Input.GetKeyDown(KeyCode.L) && headlights == false){
			for(int i = 0; i < headlamps.Length; i++){
				headlamps[i].material = headlightOn;
			}
		 
			headlights = true;
		}else if(Input.GetKeyDown(KeyCode.L) && headlights == true){
			for(int i = 0; i < headlamps.Length; i++){
				headlamps[i].material = headlightOff;
			}
			 
			headlights = false;
		}
		
		if(headlights){
			headlightsLights.SetActive(true);
		}else{
			headlightsLights.SetActive(false);
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
		float torque = maxTorque * Input.GetAxis("Vertical") / 4;
		float torqueT = maxTorque * Input.GetAxis("Vertical");
		bool handbrake = handBrakeActive;

		
		foreach (WheelCollider wheel in wheels)
		{
 
			if(handbrake){
				wheel.brakeTorque = 2750;
			}else{
				wheel.brakeTorque = 0;
			}
			

			/** STEERING AND 4X4 TRACTION SYSTEM **/
			if (wheel.transform.localPosition.z > 0){
				wheel.steerAngle = angle;
				// print(wheel.steerAngle );
				angulo = angle;
				

			if(tractionController.activeSelf){
				wheel.motorTorque = torque;
			} else{
				wheel.motorTorque = 0;
			}
			}
 
			if (wheel.transform.localPosition.z < 0){
				
				//RPM TO KMPH
				wRpm = wheel.rpm;
				var rpt = (wRpm / 4) * 100;
				var multip = 3.6;
				var rpmF = Mathf.RoundToInt(wRpm);
				var realRpm = rpmF * multip;
				var radius = 37;
				var constkph = 0.001885;
 
				var kph = realRpm * radius * constkph;
				var fSpeed = Convert.ToInt32(kph);

				speedKh = Mathf.Abs(fSpeed);
				
				if(Input.GetKeyDown(KeyCode.LeftControl)){
				 	reverseGear = true;
					for(int i = 0; i < reverselamps.Length; i++){
						reverselamps[i].material = reverseOn;
					}
				}else if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey("w")){ 
					reverseGear = false;
					for(int i = 0; i < reverselamps.Length; i++){
						reverselamps[i].material = reverseOff;
					}
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
  

			//Sets the max speed
				if(speedKh < maxSpeed){
					torqueT = maxTorque * Input.GetAxis("Vertical");
				}else{
					torqueT = maxTorque * -(Input.GetAxis("Vertical")) * Time.deltaTime;
				}
				wheel.motorTorque = torqueT;
			}
 
			//Brake SYSTEM And Reverse Actuator
			if(!reverseGear){
				if(Input.GetAxis("Vertical") < 0){
					wheel.brakeTorque = 110;
					for(int i = 0; i < brakeLamps.Length; i++){
					brakeLamps[i].material = brakeLightOn; 

					}
				}else{
					for(int i = 0; i < brakeLamps.Length; i++){
					brakeLamps[i].material = brakeLightOff;

					}
				}
			}else if(reverseGear){
				var reverseSpeed = 30;
						if(speedKh > reverseSpeed){
							wheel.brakeTorque = 5850;
							speedKh = 30;
						}
			}
 
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
 
			discoEsq.transform.localRotation = Quaternion.Euler(0f, angulo, 0f);
			discoDir.transform.localRotation = Quaternion.Euler(0f, angulo, 0f);
			// // disco.transform.localPosition = new Vector3 (transform.localPosition.x , transform.localPosition.y, transform.localPosition.z); 

		}
		

	}
 
}