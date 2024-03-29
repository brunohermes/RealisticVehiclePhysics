using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private Transform target;
    private Vector3 previousPosition;

    void start(){
        previousPosition = camera.ScreenToViewportPoint(Input.mousePosition);
    }
    void Update(){
    
        Vector3 direction = previousPosition - camera.ScreenToViewportPoint(Input.mousePosition);
        
        camera.transform.position = target.position;
       
         
        camera.transform.Rotate(new Vector3(1,0,0), direction.y * 180);
        camera.transform.Rotate(new Vector3(0,1,0), -direction.x * 180, Space.World);
        camera.transform.Translate(new Vector3(0,0, -2));
        

    }
    void FixedUpdate(){
     previousPosition = camera.ScreenToViewportPoint(Input.mousePosition);
    }
}
