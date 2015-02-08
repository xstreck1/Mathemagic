using UnityEngine;
using System.Collections;

public class ObjectCollision : MonoBehaviour
{
    public int _toughness = 0;
    public float _SMASH_TIME = 1;

    bool _destroying = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SmashThis()
    {
        _destroying = true;
        Vector3 my_scale = transform.localScale;

        // Lerp in the coroutine for the CAST TIME
        for (float time = 0; time < _SMASH_TIME; time += Time.deltaTime)
        {
            float progress = time / _SMASH_TIME;
            transform.localScale = Vector3.Lerp(my_scale, Vector3.zero, progress);
            yield return null;
        }

        Object.Destroy(gameObject);
    }
        

    void OnCollisionEnter(Collision collision)
    {
        ObjectCollision other = collision.gameObject.GetComponent<ObjectCollision>();
        if (other != null && other._toughness >= _toughness)
        {
            // StartCoroutine(SmashThis());
        }
    }

    public bool isDestroying()
    {
        return _destroying;
    }
}