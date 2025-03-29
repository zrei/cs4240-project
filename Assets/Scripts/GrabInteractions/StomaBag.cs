using UnityEngine;

public class StomaBag : MonoBehaviour
{
    [Header("Particle System")]
    [SerializeField] private ParticleSystem liquidParticles;

    [Header("Pouring Parameters")]
    [SerializeField] private float pourThreshold = 0.7f;
    [SerializeField] private float maxEmissionRate = 50f;

    private Transform _cubeTransform;
    private Vector3 _pourFaceNormal = Vector3.forward;

    private void Awake()
    {
        _cubeTransform = transform;
        
        InitializeParticles();
    }

    private void InitializeParticles()
    {
        if (liquidParticles != null)
        {
            var emission = liquidParticles.emission;
            emission.rateOverTime = 0;
        }
    }

    private void Update()
    {
        if (IsPouring())
        {
            float tiltAmount = CalculateTiltAmount();
            float emissionRate = Mathf.Lerp(0, maxEmissionRate, tiltAmount);

            var emission = liquidParticles.emission;
            emission.rateOverTime = emissionRate;

            if (!liquidParticles.isPlaying)
                liquidParticles.Play();
        }
        else if (liquidParticles.isPlaying)
        {
            liquidParticles.Stop();
        }
    }
    private bool IsPouring()
    {
        float dotProduct = Vector3.Dot(_cubeTransform.TransformDirection(_pourFaceNormal), Vector3.down);
        return dotProduct > pourThreshold;
    }

    private float CalculateTiltAmount()
    {
        float dotProduct = Vector3.Dot(_cubeTransform.TransformDirection(_pourFaceNormal), Vector3.down);
        return Mathf.InverseLerp(pourThreshold, 1f, dotProduct);
    }
}