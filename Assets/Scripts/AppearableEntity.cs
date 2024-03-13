//using System.Drawing;
using System.Collections;
using UnityEngine;

public class AppearableEntity:MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private Color color;
    // [SerializeField][Range(1f,3f)]
    float _fadeInTime = 1.7f;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.enabled = false;
    }

    private void Start()
    {
        color = _meshRenderer.material.color;
        color.a = 0f; // Установите прозрачность на 0
        _meshRenderer.material.color = color;
    }

    public void Appear()
    {
        _meshRenderer.enabled = true;
        StartCoroutine(FadeInRoutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Добавился");
        if (other.tag == "Player") {
            PlayerController.Instance.AddAppearableEntity(this);   
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Удалился");
        if (other.tag == "Player")
        {
            PlayerController.Instance.RemoveAppearableEntity(this);
        }
    }

    private IEnumerator FadeInRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _fadeInTime)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / _fadeInTime);
            _meshRenderer.material.color = color;
            yield return null;
        }

        Destroy(this);
    }
}
