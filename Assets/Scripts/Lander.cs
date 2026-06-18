using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{

    public static Lander Instance { get; private set; }

    public event EventHandler onUpForce;
    public event EventHandler onRightForce;
    public event EventHandler onLeftForce;
    public event EventHandler onBeforeForce;
    public event EventHandler OnCoinPickup;
    public event EventHandler<OnLandedEventArgs> OnLanded;
    public class OnLandedEventArgs : EventArgs
    {
        public int score;
        public LandingType landingType;
        public float dotVector;
        public float landingSpeed;
        public float scoreMultiplier;
    }

    public enum LandingType
    {
        Success,
        WrongLandingArea,
        TooSteepAngle,
        TooFastLanding
    }
    private Rigidbody2D landerRigidBody2D;
    private float fuelAmount;
    private float fuelAmountMax = 10f;
    private void Awake()
    {
        Instance = this;
        fuelAmount = fuelAmountMax;
        landerRigidBody2D = GetComponent<Rigidbody2D>();
        // Vector2.Dot(new Vector2(0, 1), new Vector2(0, 1));
        // Vector2.Dot(new Vector2(0, 1), new Vector2(.5f, .5f));
        // Vector2.Dot(new Vector2(0, 1), new Vector2(1, 0));
        // Vector2.Dot(new Vector2(0, 1), new Vector2(0, -1));
    }

    private void FixedUpdate()
    {
        onBeforeForce.Invoke(this, EventArgs.Empty);

        if (fuelAmount <= 0f)
        {
            return;
        }

        if (Keyboard.current.upArrowKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            ConsumeFuel();
        }
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
            OnLanded.Invoke(this, new OnLandedEventArgs
            {
                score = 0,
                landingType = LandingType.WrongLandingArea,
                dotVector = 0f,
                landingSpeed = 0f,
                scoreMultiplier = 0
            });
            return;
        }
        float sofLandingVelocityMagnitude = 4f;
        float relativeVelocityMAagnitude = collision2D.relativeVelocity.magnitude;
        if (relativeVelocityMAagnitude > sofLandingVelocityMagnitude)
        {
            Debug.Log("too hard");
            OnLanded.Invoke(this, new OnLandedEventArgs
            {
                score = 0,
                landingType = LandingType.TooFastLanding,
                dotVector = 0f,
                landingSpeed = relativeVelocityMAagnitude,
                scoreMultiplier = 0
            });
            return;
        }
        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = .90f;
        if (dotVector < minDotVector)
        {
            Debug.Log("landed on steep angle");
            OnLanded.Invoke(this, new OnLandedEventArgs
            {
                score = 0,
                landingType = LandingType.TooSteepAngle,
                dotVector = dotVector,
                landingSpeed = relativeVelocityMAagnitude,
                scoreMultiplier = 0
            });
            return;
        }

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

        OnLanded.Invoke(this, new OnLandedEventArgs
        {
            score = score,
            landingType = LandingType.Success,
            dotVector = dotVector,
            landingSpeed = relativeVelocityMAagnitude,
            scoreMultiplier = landingPad.GetScoreMultiplier()
        });

    }

    private void OnTriggerEnter2D(Collider2D collision2D)
    {
        if (collision2D.gameObject.TryGetComponent(out FuelPickup fuelPickup))
        {
            float addFuelAmount = 10f;
            fuelAmount += addFuelAmount;
            if (fuelAmount >= fuelAmountMax)
            {
                fuelAmount = fuelAmountMax;
            }
            fuelPickup.DestroySelf();
        }

        if (collision2D.gameObject.TryGetComponent(out CoinPickup coinPickup))
        {
            OnCoinPickup.Invoke(this, EventArgs.Empty);
            coinPickup.DestroySelf();
        }
    }

    private void ConsumeFuel()
    {
        float fuelConsumptionAmount = 1f;
        fuelAmount -= fuelConsumptionAmount * Time.deltaTime;
    }

    public float GetSpeedX()
    {
        return landerRigidBody2D.linearVelocityX;
    }

    public float GetSpeedY()
    {
        return landerRigidBody2D.linearVelocityY;
    }

    public float GetFuel()
    {
        return fuelAmount;
    }

    public float GetFuelAmountNormalized()
    {
        return fuelAmount / fuelAmountMax;
    }

}
