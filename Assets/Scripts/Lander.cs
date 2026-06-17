using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    public event EventHandler onUpForce;
    public event EventHandler onRightForce;
    public event EventHandler onLeftForce;
    public event EventHandler onBeforeForce;
    private Rigidbody2D landerRigidBody2D;
    private void Awake()
    {
        landerRigidBody2D = GetComponent<Rigidbody2D>();
        // Vector2.Dot(new Vector2(0, 1), new Vector2(0, 1));
        // Vector2.Dot(new Vector2(0, 1), new Vector2(.5f, .5f));
        // Vector2.Dot(new Vector2(0, 1), new Vector2(1, 0));
        // Vector2.Dot(new Vector2(0, 1), new Vector2(0, -1));
    }

    private void FixedUpdate()
    {
        onBeforeForce.Invoke(this, EventArgs.Empty);
        if (Keyboard.current.upArrowKey.isPressed)
        {
            float force = 700f;
            landerRigidBody2D.AddForce(force * transform.up * Time.deltaTime);
            onUpForce?.Invoke(this, EventArgs.Empty);
        }

        if (Keyboard.current.leftArrowKey.isPressed)
        {
            float turnSpeed = +100f;
            landerRigidBody2D.AddTorque(turnSpeed * Time.deltaTime);
            onLeftForce?.Invoke(this, EventArgs.Empty);

        }

        if (Keyboard.current.rightArrowKey.isPressed)
        {
            float turnSpeed = -100f;
            landerRigidBody2D.AddTorque(turnSpeed * Time.deltaTime);
            onRightForce?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            Debug.Log("crashed on the terrain");
            return;
        }
        float sofLandingVelocityMagnitude = 4f;
        float relativeVelocityMAagnitude = collision2D.relativeVelocity.magnitude;
        if (relativeVelocityMAagnitude > sofLandingVelocityMagnitude)
        {
            Debug.Log("too hard");
            return;
        }
        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = .90f;
        if (dotVector < minDotVector)
        {
            Debug.Log("landed on steep angle");
            return;
        }
        Debug.Log(dotVector);
        Debug.Log("soft landing");

        float maxScoreAmountLandingAngle = 100;
        float scoreDotVectorMultiplier = 10f;

        float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreAmountLandingAngle;


        float maxScoreAmountLandingSpeed = 100;

        float landingSpedScore = (sofLandingVelocityMagnitude - relativeVelocityMAagnitude) * maxScoreAmountLandingSpeed;

        Debug.Log("landing Angle score: " + landingAngleScore);
        Debug.Log("landing speed score: " + landingSpedScore);

        int score = Mathf.RoundToInt((landingAngleScore + landingSpedScore) * landingPad.GetScoreMultiplier());


        Debug.Log("score: " + score);

    }

}
