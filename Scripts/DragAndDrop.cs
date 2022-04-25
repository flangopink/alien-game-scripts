using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public GameObject selectedObject;
    Vector3 offset;

    void Update()
    {
        if (GameObject.FindGameObjectWithTag("AlienParent").GetComponent<AlienParent>().phase1_done)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
                if (targetObject && targetObject.CompareTag("Alien"))
                {
                    selectedObject = targetObject.transform.gameObject;
                    offset = selectedObject.transform.position - mousePosition;
                }
            }
            if (selectedObject)
            {
                selectedObject.GetComponent<Alien>().isGrabbed = true;
                selectedObject.transform.position = mousePosition + offset;
            }
            if (Input.GetMouseButtonUp(0) && selectedObject)
            {
                selectedObject.GetComponent<Alien>().isGrabbed = false;
                selectedObject = null;
            }
        }
    }
}
