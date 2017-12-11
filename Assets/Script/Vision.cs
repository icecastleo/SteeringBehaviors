using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour {

    public Character obj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        obj.OnVisionEnter(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // back of the character
        //if (Vector3.Dot(GetComponentInParent<SpriteRenderer>().flipX ? Vector3.left : Vector3.right, other.transform.position - transform.position) <= Mathf.Cos((Mathf.PI / 180) * 75))
        //    return;

        obj.OnVisionStay(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        obj.OnVisionExit(other);
    }

}
