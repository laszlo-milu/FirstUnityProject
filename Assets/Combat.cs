using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour {

    public GameObject target;

    public float range;
    public int damage;
    public float hitTime;
    public bool hit;
    public bool targetSwitchable = true;

    Animation animation;
    Vector3 position;

    public AnimationClip attack;

	// Use this for initialization
	void Start () {
        animation = GetComponent<Animation>();
        position = transform.position;

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButton(0) && (Input.GetKey(KeyCode.LeftShift) || (target != null && InRange())))
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
            animation.Play(attack.name);
            Hit();
            ClickToMove.attack = true;
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

    void Hit()
    {
        if (target != null && InRange() && animation.IsPlaying(attack.name) && !hit)
        {
            if (animation[attack.name].time > animation[attack.name].length*hitTime && animation[attack.name].time < 0.95 * animation[attack.name].length)
            {
                target.GetComponent<Mob>().GetHit(damage);
                hit = true;
            }
        }
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
