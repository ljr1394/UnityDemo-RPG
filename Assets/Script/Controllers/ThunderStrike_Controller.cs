using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{
    private CharacterStats targetStats;
    [SerializeField] private float speed;
    private int damage;


    private bool triggered;

    private Animator anim;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered)
            return;
        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;
        if (Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
            triggered = true;
            transform.localRotation = Quaternion.identity;
            anim.transform.localRotation = Quaternion.identity;
            anim.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            anim.transform.localPosition = new Vector3(0, 0.65f, 0);
            anim.SetBool("Hit", true);


        }
    }



    public void Setup(int _damage, CharacterStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }

    public void StrikeTakeDamege()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);
    }

    public void SelfDestroy() => Destroy(gameObject);


}
