using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class CharacterBehaviour : MonoBehaviour
{
    [HideInInspector] public CharactersPool CharactersPool;
    [SerializeField] private float duration;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            Mine mine;
            if (collision.gameObject.TryGetComponent(out mine))
            {
                mine.Explosion();
            }
            else
            {
                Remove();
                rb.AddForce(new Vector3(Random.Range(-2.5f, 2.5f), 5f, -5f), ForceMode.Impulse);
                rb.AddTorque(new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-2.5f, 2.5f), Random.Range(-2.5f, 2.5f)), ForceMode.Impulse);
            }
        }
        else if(collision.gameObject.tag == "Fallguy")
        {
            Destroy(collision.gameObject);
            CharactersPool.AddCharacter(collision.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Finish")
        {
            GameManager.GameOver?.Invoke(other.transform);
        }
    }

    public void Remove()
    {
        transform.SetParent(null);
        CharactersPool.RemoveCharacter(this);
        rb.freezeRotation = false;
        Destroy(gameObject, 5f);
    }

    public void DoMove(Vector3 point)
    {
        transform.DOMove(point, duration * 5);
    }

    public void DOLocalMove(Vector3 point)
    {
        transform.DOLocalMove(point, duration);
    }
}
