using UnityEngine;
using UnityEngine.Pool;

public class Smoke : MonoBehaviour
{
    Animator anim;
    private IObjectPool<Smoke> pool;
    public IObjectPool<Smoke> Pool { set => pool = value; }
    public Vector3Int cellCoords;

    private void Start()
    {
        //anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (anim == null) return;

        if (collision.CompareTag("Player"))
        {
            //anim.SetTrigger("Vanish");
            pool.Release(this);
        }
    }

    public void FinishAnim()
    {
        pool.Release(this);
    }
}
