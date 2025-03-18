using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable _grabInteractable;
    private Transform _cubeTransform;
    private Vector3 _pourFaceNormal = Vector3.forward;
    private bool _isAttached = true;

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private bool _isBeingGrabbed = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        _cubeTransform = transform;

        ConfigureGrabInteractable();
        InitializeParticles();
        AttachToConnector();
    }

    private void ConfigureGrabInteractable()
    {
        _grabInteractable.selectEntered.AddListener(OnGrabbed);
        _grabInteractable.selectExited.AddListener(OnReleased);
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

        if (_isAttached && _isBeingGrabbed)
        {
            float distanceMoved = Vector3.Distance(_cubeTransform.position, _initialPosition);
            if (distanceMoved > 0.05f)
            {
                GlobalEvents.StepsEvents.OnCompleteStep();
                _isAttached = false;
            }
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        _initialPosition = _cubeTransform.position;
        _initialRotation = _cubeTransform.rotation;
        _isBeingGrabbed = true;
        _rb.isKinematic = false;

 
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        _isBeingGrabbed = false;

        if (_isAttached)
        {
            _cubeTransform.position = _initialPosition;
            _cubeTransform.rotation = _initialRotation;
        }
        else
            DetachFromConnector();
    }

    public void AttachToConnector()
    {
        if (stomaConnector != null)
        {
            transform.position = stomaConnector.position + new Vector3(0, 0, 0.11f);
            transform.parent = stomaConnector;
            _rb.isKinematic = true;
            _isAttached = true;
        }
    }

    public void DetachFromConnector()
    {
        if (_isAttached)
        {
            transform.parent = null;
            _rb.isKinematic = false;
            _isAttached = false;
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