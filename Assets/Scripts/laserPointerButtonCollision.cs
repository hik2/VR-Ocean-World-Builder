using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class laserPointerButtonCollision : MonoBehaviour {

    private BoxCollider bCollider;
    private RectTransform rTransform;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        rTransform = GetComponent<RectTransform>();
        bCollider = GetComponent<BoxCollider>();
        if (bCollider == null)
        {
            bCollider = gameObject.AddComponent<BoxCollider>();
        }
        bCollider.size = rTransform.sizeDelta;
    }
}