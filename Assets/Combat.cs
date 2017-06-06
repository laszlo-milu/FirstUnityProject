using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour {

    public GameObject target;
    public GameObject highlighted;

    public float range;
    public int minDmg;
    public int maxDmg;
    public float hitTime;
    public bool hit;
    public bool targetSwitchable = true;
    public int hp;
    public bool alive = true;
    public int stunTime;

    Animation animation;
    Vector3 position;
    Vector3 defaultPosition;
    Quaternion defaultRotation;

    private System.Diagnostics.Stopwatch stopWatch;

    public AnimationClip attack;
    public AnimationClip die;

	// Use this for initialization
	void Start () {
        animation = GetComponent<Animation>();
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
        position = transform.position;
        hp = 500;
        stopWatch = new System.Diagnostics.Stopwatch();



    }

    // Update is called once per frame
    void Update() {
        if (hp < 1)
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
                    stopWatch.Reset();
                    Respawn();
                }
            }
        }
        else
        {
            if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && (Input.GetKey(KeyCode.LeftShift) || (target != null && InRange())))
            {
                if (target != null)
                {
                    transform.LookAt(target.transform.position);
                }
                else
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 1000) && hit.collider.tag != "Player" || hit.collider.tag != "Enemy")
                    {
                        position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    }
                    Quaternion newRotation = Quaternion.LookRotation(position - transform.position, Vector3.zero);
                    newRotation.x = 0f; newRotation.z = 0f;
                    transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10);
                }
                
                if (Input.GetMouseButton(0))
                {
                    animation.Play(attack.name);
                    Hit();
                    ClickToMove.attack = true;
                }
                if (Input.GetMouseButton(1))
                {
                    animation.Play(attack.name);
                    StunTarget(stunTime);
                    ClickToMove.attack = true;
                }
            }
            if (animation[attack.name].time > 0.95 * animation[attack.name].length)
            {
                hit = false;
            }
            if (!animation.IsPlaying(attack.name))
            {
                ClickToMove.attack = false;
            }
        }

    }

    void Respawn()
    {
        alive = true;
        transform.SetPositionAndRotation(defaultPosition, defaultRotation);
        hp = 500;
        ClickToMove.position = defaultPosition;
    }

    void Hit()
    {
        if (target != null && InRange() && animation.IsPlaying(attack.name) && !hit)
        {
            if (animation[attack.name].time > animation[attack.name].length*hitTime && animation[attack.name].time < 0.95 * animation[attack.name].length)
            {
                target.GetComponent<Mob>().GetHit(Random.Range(minDmg, maxDmg+1));
                hit = true;
            }
        }
    }

    void StunTarget(int t)
    {
        if (target != null && InRange() && animation.IsPlaying(attack.name) && !hit)
        {
            if (animation[attack.name].time > animation[attack.name].length * hitTime && animation[attack.name].time < 0.95 * animation[attack.name].length)
            {
                target.GetComponent<Mob>().GetStunned(t);
                hit = true;
            }
        }
    }

    public void GetHit(int dmg)
    {
        if (hp > 0)
        {
            hp -= dmg;
        }
        Debug.Log(transform.name + hp);
    }

    public void ResetTarget()
    {
        target = null;
    }

    public bool InRange()
    {
        return Vector3.Distance(transform.position, target.transform.position) < range;
    }
}
