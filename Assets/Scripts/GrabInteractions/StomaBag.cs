using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StomaBag : GrabAndFollow
{

    [Header("Attachment")]
    [SerializeField] private Transform stomaConnector;

    private Rigidbody rb;
    private bool isAttached = true;

    [Header("Particle System")]
    [SerializeField] private ParticleSystem liquidParticles;

    [Header("Pouring Parameters")]
    [SerializeField] private float pourThreshold = 0.7f; 
    [SerializeField] private float maxEmissionRate = 50f; 

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

        rb = GetComponent<Rigidbody>();
        AttachToConnector();
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

    public override GrabState OnGrab(Transform grabbedTransform)
    {
        DetachFromConnector();
        return base.OnGrab(grabbedTransform);
    } 


    public void AttachToConnector()
    {
        if (stomaConnector != null)
        {
            
            transform.parent = stomaConnector;
            rb.isKinematic = true; 
            isAttached = true;
        }
    }

    public void DetachFromConnector()
    {
        if (isAttached)
        {
            
            transform.parent = null;
            rb.isKinematic = false; 
            isAttached = false;
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
        return Mathf.InverseLerp(pourThreshold, 1f, dotProduct); 
    }
}