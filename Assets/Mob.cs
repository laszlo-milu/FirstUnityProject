using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour {

    public float hitTime;
    public bool hit;
    public int damage;
    public float speed;
    public float range;
    public float sightRange;
    public int maxHp;
    public int hp;
    bool alive;
    private int stunTime = 0;
    public CharacterController characterController;
    public Transform player;

    public Animation animation;

    public AnimationClip run;
    public AnimationClip idle;
    public AnimationClip attack;
    public AnimationClip die;
    private System.Diagnostics.Stopwatch stopWatch;



    // Use this for initialization
    void Start()
    {
        hit = false;
        animation = GetComponent<Animation>();
        alive = true;
        stopWatch = new System.Diagnostics.Stopwatch();
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(stunTime);
        if (animation[attack.name].time > 0.95 * animation[attack.name].length)
        {
            hit = false;
        }
        if (hp > 0)
        {
            if (stunTime > 0)
            {
                
            }
            else
            {
                if (InSight())
                {
                    if (player.GetComponent<Combat>().hp > 0)
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

                }
                else
                {
                    animation.Play(idle.name);
                    characterController.SimpleMove(Vector3.zero);
                }
            }
            
        }
        else
        {
            if (alive)
            {
                stopWatch.Start();
                animation.Play(die.name);
                if (animation[die.name].time > 0.95 * animation[die.name].length)
                {
                    alive = false;
                }
            }
            else
            {
                if (stopWatch.ElapsedMilliseconds > 5000)
                {
                    stopWatch.Stop();
                    Destroy(gameObject);
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
        animation.Play(attack.name);
        if (InRange())
        {
            Hit();
        }
    }

    void Hit()
    {
        if (InRange() && animation.IsPlaying(attack.name) && !hit)
        {
            if (animation[attack.name].time > animation[attack.name].length * hitTime && animation[attack.name].time < 0.95 * animation[attack.name].length)
            {
                player.GetComponent<Combat>().GetHit(damage);
                hit = true;
            }
        }
    }

    void StunCountdown()
    {
        stunTime = stunTime - 1;
        if (stunTime <= 0)
        {
            CancelInvoke("StunCountdown");
        }
    }

    public void GetStunned(int t)
    {
        stunTime = t;
        InvokeRepeating("StunCountdown", 0f, 1f);
    }

    public void GetHit(int dmg)
    {
        if (hp > 0)
        {
            hp = (hp - dmg > -1) ? hp -= dmg : hp = 0;
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            player.GetComponent<Combat>().target = gameObject;
        }
        player.GetComponent<Combat>().highlighted = gameObject;

    }

    private void OnMouseExit()
    {
        player.GetComponent<Combat>().highlighted = null;

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
