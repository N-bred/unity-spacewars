using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{

    public int hits = 1;
    public int points = 100;
    public Vector3 rotator;
    public Material hitMaterial;
    Material _originalMaterial;
    Renderer _renderer;

    void Start()
    {
        rotator = new Vector3(45, 0, 0);
        transform.Rotate(rotator * (transform.position.x + transform.position.y) * Random.Range(0.1f, 0.5f));
        _renderer = GetComponent<Renderer>();
        _originalMaterial = _renderer.sharedMaterial;
    }

   
    void Update()
    {
        transform.Rotate(rotator * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        hits--;

        _renderer.sharedMaterial = hitMaterial;

        if (hits == 0)
        {
            // Add points to game score
            GameManager.Instance.Score += points;
            Destroy(gameObject);

        }

        Invoke(nameof(RestoreMaterial), 0.05f);
        
    }

    void RestoreMaterial()
    {
        _renderer.sharedMaterial = _originalMaterial;
    }
}
