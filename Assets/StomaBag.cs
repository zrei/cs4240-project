using UnityEngine;

public class StomaBag : MonoBehaviour
{
    [Header("Particle System")]
    [SerializeField] private ParticleSystem liquidParticles;

    [Header("Pouring Parameters")]
    [SerializeField] private float pourThreshold = 0.7f; 
    [SerializeField] private float maxEmissionRate = 100f; 

    private Transform cubeTransform;
    private Vector3 pourFaceNormal = Vector3.forward;

    private void Start()
    {
        cubeTransform = transform;
        if (liquidParticles == null)
        {
            Debug.LogError("Particle System is not assigned!");
        }
        else
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
            {
                liquidParticles.Play();
            }
        }
        else
        {
            if (liquidParticles.isPlaying)
            {
                liquidParticles.Stop();
            }
        }
    }

    private bool IsPouring()
    {

        float dotProduct = Vector3.Dot(cubeTransform.TransformDirection(pourFaceNormal), Vector3.down);
        return dotProduct > pourThreshold;
    }

    private float CalculateTiltAmount()
    {
        float dotProduct = Vector3.Dot(cubeTransform.TransformDirection(pourFaceNormal), Vector3.down);
        return Mathf.InverseLerp(pourThreshold, 1f, dotProduct); // Normalize the tilt amount between 0 and 1
    }
}