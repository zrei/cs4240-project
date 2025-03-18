using UnityEngine;
using UnityEngine.Events;
using XRInteraction = UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

[RequireComponent(typeof(Rigidbody), typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class StomaBag : MonoBehaviour
{
    [Header("Attachment")]
    [SerializeField] private Transform stomaConnector;

    [Header("Particle System")]
    [SerializeField] private ParticleSystem liquidParticles;

    [Header("Pouring Parameters")]
    [SerializeField] private float pourThreshold = 0.7f;
    [SerializeField] private float maxEmissionRate = 50f;


    private Rigidbody _rb;
    private XRInteraction.Interactables.XRGrabInteractable _grabInteractable;
    private Transform _cubeTransform;
    private Vector3 _pourFaceNormal = Vector3.forward;
    private bool _isAttached = true;

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private bool _isBeingGrabbed = false;

    private FixedJoint _joint;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _cubeTransform = transform;
        
        InitializeParticles();
        AttachToConnector();
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

        if (_isBeingGrabbed && _isAttached)
        {
            float distanceMoved = Vector3.Distance(_cubeTransform.position, stomaConnector.position);
            Debug.Log("Distance moved: " + distanceMoved);
            
            if (distanceMoved > 0.3f)
            {
                DetachFromConnector();
                GlobalEvents.StepsEvents.OnCompleteStep();
            }
        }
    }
    public void OnGrabbed()
    {
        Debug.Log("Stoma bag grabbed");
        _isBeingGrabbed = true;
        
    }

    public void OnReleased()
    {
        Debug.Log("Stoma bag released");
        _isBeingGrabbed = false;
        

    }

    public void AttachToConnector()
    {
        if (stomaConnector != null)
        {
            transform.position = stomaConnector.position;
            
            if (_joint != null)
            {
                Destroy(_joint);
            }
            
            Rigidbody connectorRb = stomaConnector.GetComponent<Rigidbody>();
            if (connectorRb == null)
            {
                connectorRb = stomaConnector.gameObject.AddComponent<Rigidbody>();
                connectorRb.isKinematic = true;
                connectorRb.interpolation = RigidbodyInterpolation.Interpolate;
            }
            
            _joint = gameObject.AddComponent<FixedJoint>();
            _joint.connectedBody = connectorRb;
            _joint.breakForce = 2000f;
            _joint.enableCollision = false;
            
            _rb.isKinematic = false;
            _rb.useGravity = false;
            _rb.mass = 1f;
            _rb.interpolation = RigidbodyInterpolation.Interpolate;
            
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
            
            StartCoroutine(RemoveConstraintsAfterDelay(0.7f));
            
            _isAttached = true;
            
            Debug.Log("Stoma bag firmly attached to connector");
        }
    }

    private IEnumerator RemoveConstraintsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (_isAttached)
        {
            _rb.constraints = RigidbodyConstraints.None;
        }
    }

    public void DetachFromConnector()
    {
        Debug.Log("Detach Called");
        if (_joint != null)
        {
            Destroy(_joint);
        }
        
        _rb.isKinematic = false;
        _rb.useGravity = true;
        
        _rb.linearDamping = 0.05f;        
        _rb.angularDamping = 0.05f;  
        
        _rb.constraints = RigidbodyConstraints.None;
        
        _isAttached = false;
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