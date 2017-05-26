using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove : MonoBehaviour {

    public static bool attack;
    public float speed;
    public CharacterController characterController;
    public Vector3 position;

    public Animation anim;

    public AnimationClip run;
    public AnimationClip idle;
    public Combat combat;

    // Use this for initialization
    void Start() {
        anim = GetComponent<Animation>();
        position = transform.position;
        combat = transform.GetComponent<Combat>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButton(0))
        {
            LocatePosition();
        }
        if (!attack)
        {
            MoveToPosition();
        }
    }
    void LocatePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000) && hit.collider.tag != "Player" || hit.collider.tag != "Enemy")
        {
            position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            if(combat.targetSwitchable)
            {
                combat.ResetTarget();
            }
        }
    }
    void MoveToPosition()
    {
        
        if (Vector3.Distance(transform.position, position) > 1)
        {
            Quaternion newRotation = Quaternion.LookRotation(position - transform.position, Vector3.forward);
            newRotation.x = 0f; newRotation.z = 0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10);
            characterController.SimpleMove(transform.forward * speed);
            anim.CrossFade(run.name);
        }
        else
        {
            anim.CrossFade(idle.name);
        }
    }
}
