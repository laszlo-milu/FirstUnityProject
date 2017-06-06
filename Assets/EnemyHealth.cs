using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    public Combat combat;
    public Texture2D frame;
    public Rect framePosition;
    public Texture2D bar;
    public Rect barPosition;
    public float horizontalDistance;
    public float verticalDistance;
    public float width;
    public float height;
    public Mob target;
    public float hpPercentage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (combat.target != null)
        {
            target = combat.target.GetComponent<Mob>();
            hpPercentage = (float)target.hp / (float)target.maxHp;
        }
        else if (combat.highlighted != null)
        {
            target = combat.highlighted.GetComponent<Mob>();
            hpPercentage = (float)target.hp / (float)target.maxHp;
        }
        else
        {
            hpPercentage = 0;
        }
    }

    private void OnGUI()
    {
        if (combat.target != null || combat.highlighted != null)
        {
            DrawFrame();
            DrawBar();
        }
    }

    void DrawFrame()
    {
        framePosition.x = (Screen.width - framePosition.width) / 2;
        float width = 0.4f;
        framePosition.width = Screen.width * width;
        float height = 0.1f;
        framePosition.height = Screen.height * height;

        GUI.DrawTexture(framePosition, frame);
    }

    void DrawBar()
    {
        barPosition.x = framePosition.x + framePosition.width * horizontalDistance;
        barPosition.y = framePosition.y + framePosition.height * verticalDistance;
        barPosition.width = framePosition.width * width * hpPercentage;
        barPosition.height = framePosition.height * height;

        GUI.DrawTexture(barPosition, bar);
    }
}
