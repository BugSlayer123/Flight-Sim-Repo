using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlightSim
{
    public class FlightControl : MonoBehaviour
    {
        float speed;
        float glide;
        Rigidbody rb;

        [SerializeField] AerodynamicProperties aerodynamicProperties;

        Camera mainCamera;
        IController controller;


        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            mainCamera = Camera.main;
            speed = aerodynamicProperties.Speed;

            RegisterControlInput();
        }

        void RegisterControlInput()
        {
            controller = GetComponent<IController>();
            controller.OnFlightControlInput += ApplyFlightControlInput;
            controller.OnThrottleInput += ApplyThrottleInput;
            controller.OnBrakeInput += ApplyBrakeInput;
        }

        void FixedUpdate()
        {
            AdjustSpeedBasedOnAttitude();
        }

        void AdjustSpeedBasedOnAttitude()
        {
            speed -= transform.forward.y * Time.deltaTime * 10.0f;

            if (speed < 25.0f)
            {
                speed = 25.0f;
            }
        }

        void ApplyFlightControlInput(Vector3 flightControlInput)
        {
            float roll = flightControlInput.z * 5.5f;
            float pitch = flightControlInput.x * 4.0f;
            float rotation = flightControlInput.y * aerodynamicProperties.Yaw;

            rotation *= Time.deltaTime;

            //transform.Rotate(0, (rotation + flightControlInput.y * -1.5f), 0);
            rb.AddRelativeTorque(Vector3.up * aerodynamicProperties.Torque * rotation);
            rb.AddRelativeTorque(Vector3.back * aerodynamicProperties.Torque * roll);
            rb.AddRelativeTorque(Vector3.right * aerodynamicProperties.Torque * pitch);
        }

        void ApplyThrottleInput(bool throttle)
        {


            if (!throttle) return;

            rb.AddRelativeForce(Vector3.forward * aerodynamicProperties.Thrust);
            glide = aerodynamicProperties.Thrust;

        }

        void ApplyBrakeInput(bool brake)
        {
            Vector3 opposite = -rb.velocity;
            int brakePower = 500;
            Vector3 brakeForce = opposite.normalized * brakePower;

            if (brake)
            {
                rb.AddForce(opposite * Time.deltaTime);
                rb.AddForce(brakeForce * Time.deltaTime);
                //rb.AddRelativeForce(-Vector3.forward * reverse);
                glide = aerodynamicProperties.Reverse;
            }
            else
            {
                rb.AddRelativeForce(Vector3.forward * glide);
                glide *= 0.995f;
            }
        }
    }
}