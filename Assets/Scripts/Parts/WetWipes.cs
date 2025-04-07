using UnityEngine;

public class WetWipes : MonoBehaviour
{
    [SerializeField] private Material _dirtyMaterial;

    private MeshRenderer _meshRenderer;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void DirtyWetWipes()
    {
        _meshRenderer.material = _dirtyMaterial;
    }
}