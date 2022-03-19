using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axel
{
    Front,
    Rear
}
[System.Serializable]
public struct Wheel
{
    public WheelCollider collider;
    public GameObject model;
    public Axel aks;
}
public class CarController : MonoBehaviour
{
    [SerializeField] float maxAcceleration = 20.0f;
    [SerializeField] float turnSensivity = 1.0f;
    [SerializeField] float maxSteerAngle = 45.0f;
    [SerializeField] List<Wheel> wheels;

    float inputX, inputY;
    [SerializeField] Vector3 centerOfMass;

    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
    }
    private void Update()
    {
        GetInputs();
        AnimateWheels();
    }
    void FixedUpdate()
    {

    }
    private void LateUpdate()
    {
        Move();
        Turn();
    }
    void GetInputs()
    {
        inputY = Input.GetAxis("Vertical");
        inputX = Input.GetAxis("Horizontal");
    }
    void Move()
    {
        foreach (var wheel in wheels)
        {
            if(wheel.aks == Axel.Rear)
            {
                wheel.collider.motorTorque = inputY * maxAcceleration * 500 * Time.deltaTime;
            }
        }
    }
    void Turn()
    {
        foreach(var wheel in wheels)
        {
            if(wheel.aks == Axel.Front)
            {
                float steerAngle = inputX * turnSensivity * maxSteerAngle;
                wheel.collider.steerAngle = Mathf.Lerp(wheel.collider.steerAngle,steerAngle,0.5f);
            }
        }
    }
    void AnimateWheels()
    {
        foreach(Wheel wheel in wheels)
        {
            Vector3 pos;
            Quaternion rot;
            wheel.collider.GetWorldPose(out pos, out rot);
            wheel.model.transform.position = pos;
            wheel.model.transform.rotation = rot;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + transform.rotation * centerOfMass, 0.25f);
    }
}
