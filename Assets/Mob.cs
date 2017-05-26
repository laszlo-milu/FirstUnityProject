using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour {

    public float speed;
    public float range;
    public float sightRange;
    public int hp;
    bool alive;
    public CharacterController characterController;
    public Transform player;

    public Animation animation;

    public AnimationClip run;
    public AnimationClip idle;
    public AnimationClip attack;
    public AnimationClip die;



    // Use this for initialization
    void Start()
    {
        animation = GetComponent<Animation>();
        alive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp > 0)
        {
            if (InSight())
            {
                if (InRange())
                {
                    Attack();
                }
                else
                {
                    Chase();
                }
            }
            else
            {
                animation.Play(idle.name);
                characterController.SimpleMove(Vector3.zero);
            }
        }
        else
        {
            if(alive)
            {
                animation.Play(die.name);
                if (animation[die.name].time > 0.95 * animation[die.name].length)
                {
                    alive = false;
                }
            }
        }
        
    }

    bool InRange()
    {
        return Vector3.Distance(transform.position, player.position) < range;
    }

    bool InSight()
    {
        return Vector3.Distance(transform.position, player.position) < sightRange;
    }

    void Chase()
    {
        transform.LookAt(player.position);
        characterController.SimpleMove(transform.forward * speed);
        animation.CrossFade(run.name);
    }

    void Attack()
    {
        transform.LookAt(player.position);
        animation.CrossFade(attack.name);
    }
    public void GetHit(int dmg)
    {
        if (hp > 0)
        {
            hp -= dmg;
        }
        Debug.Log(transform.name + hp);
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            player.GetComponent<Combat>().target = gameObject;

        }
    }
    private void OnMouseDown()
    {
        player.GetComponent<Combat>().target = gameObject;
        player.GetComponent<Combat>().targetSwitchable = false;
    }

    private void OnMouseUp()
    {
        player.GetComponent<Combat>().targetSwitchable = true;
    }
}
