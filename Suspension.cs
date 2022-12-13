using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class Suspension : MonoBehaviour
{ 
    [Range(0,20)]
    public float naturalFrequency = 10; // (Natural Frequency of the spring) Tendência de oscilação natural, (frequencia ressonante).

    [Range(0, 3)]
    public float dampingRatio = 0.5f; // Amortecimento

    [Range(-1, 1)]
    public float forceShift = 0.03f; // Normal force shift relativa a primeira lei de newton

    public bool setSuspensionDistance = true;    

    void Update()
    {
        foreach(WheelCollider wheelcol in GetComponentsInChildren<WheelCollider>()){
            JointSpring spring = wheelcol.suspensionSpring;

            spring.spring = Mathf.Pow(Math.Sqrt(wheelcol.sprungMass) * naturalFrequency, 2);
            spring.damper = 2 * dampingRatio * Mathf.Sqrt(spring.spring * wheelcol.sprungMass);
            wheelcol.suspensionSpring = spring;
            Vector3 wheelRelativeBody = transform.InverseTransformPoint(wheelcol.transform.position);
            float distance = GetComponent<Rigidbody>().centerOfMass.y - wheelRelativeBody.y + wheelcol.radius;
            wheelcol.forceAppPointDistance = distance - forceShift;

            if(spring.targetPosition > 0 && setSuspensionDistance){
                wheelcol.suspensionDistance = wheelcol.sprungMass * Physics.gravity.magnitude / (spring.targetPosition * spring.spring);
            }
        }
        
    }
}
