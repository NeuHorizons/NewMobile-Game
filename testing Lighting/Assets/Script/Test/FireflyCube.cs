using UnityEngine;

public class FireflyCube : MonoBehaviour
{
    private Vector3 _target;
    private float _speed;
    private float _lifetime;
    private float _age;
    private Material _mat;
    private float _glowSpeed;

    void Start()
    {
        _target = transform.position + Random.insideUnitSphere * 2f;
        _speed = Random.Range(0.2f, 0.5f);
        _lifetime = Random.Range(3f, 6f);
        _glowSpeed = Random.Range(1f, 2f);

        _mat = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        _age += Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, _target, Time.deltaTime * _speed);

        if (Vector3.Distance(transform.position, _target) < 0.1f)
        {
            _target = transform.position + Random.insideUnitSphere * 2f;
        }

        float glow = Mathf.PingPong(Time.time * _glowSpeed, 1f);
        Color color = _mat.GetColor("_EmissionColor");
        _mat.SetColor("_EmissionColor", color * glow);

        if (_age > _lifetime)
        {
            Destroy(gameObject);
        }
    }
}
