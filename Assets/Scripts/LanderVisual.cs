using UnityEngine;

public class LanderVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftThrustParticleSystem;
    [SerializeField] private ParticleSystem middleThrustParticleSystem;
    [SerializeField] private ParticleSystem rightThrustParticleSystem;
    [SerializeField] private GameObject landerExplosionVfx;

    private Lander lander;
    private void Awake()
    {
        lander = GetComponent<Lander>();
        lander.onUpForce += Lander_OnUpForce;
        lander.onLeftForce += Lander_OnLeftForce;
        lander.onRightForce += Lander_OnRightForce;
        lander.onBeforeForce += Lander_OnBeforeForce;


        SetEnableThrusterParticleSystem(leftThrustParticleSystem, false);
        SetEnableThrusterParticleSystem(middleThrustParticleSystem, false);
        SetEnableThrusterParticleSystem(rightThrustParticleSystem, false);
    }

    private void Start()
    {
        lander.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        switch (e.landingType)
        {
            case Lander.LandingType.TooFastLanding:
            case Lander.LandingType.TooSteepAngle:
            case Lander.LandingType.WrongLandingArea:
                //crash
                Instantiate(landerExplosionVfx, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
                break;
        }
    }

    private void Lander_OnBeforeForce(object sender, System.EventArgs e)
    {

        SetEnableThrusterParticleSystem(leftThrustParticleSystem, false);
        SetEnableThrusterParticleSystem(middleThrustParticleSystem, false);
        SetEnableThrusterParticleSystem(rightThrustParticleSystem, false);

    }

    private void Lander_OnUpForce(object sender, System.EventArgs e)
    {
        SetEnableThrusterParticleSystem(leftThrustParticleSystem, true);
        SetEnableThrusterParticleSystem(middleThrustParticleSystem, true);
        SetEnableThrusterParticleSystem(rightThrustParticleSystem, true);
    }

    private void Lander_OnLeftForce(object sender, System.EventArgs e)
    {
        SetEnableThrusterParticleSystem(leftThrustParticleSystem, false);
        SetEnableThrusterParticleSystem(middleThrustParticleSystem, false);
        SetEnableThrusterParticleSystem(rightThrustParticleSystem, true);
    }

    private void Lander_OnRightForce(object sender, System.EventArgs e)
    {
        SetEnableThrusterParticleSystem(leftThrustParticleSystem, true);
        SetEnableThrusterParticleSystem(middleThrustParticleSystem, false);
        SetEnableThrusterParticleSystem(rightThrustParticleSystem, false);
    }


    private void SetEnableThrusterParticleSystem(ParticleSystem particleSystem, bool enabled)
    {
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        emissionModule.enabled = enabled;
    }
}
