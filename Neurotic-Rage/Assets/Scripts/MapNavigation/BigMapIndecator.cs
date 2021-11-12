using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMapIndecator : MonoBehaviour
{
    public float scaleX,scaleY;
    public Transform dot;
    public Camera playerCamera;
    public Vector3 moveDiretion;

    private void Update()
    {
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            moveDiretion.x = 0;
        }
        if (Input.GetAxisRaw("Vertical") == 0)
        {
            moveDiretion.y = 0;
        }


        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            moveDiretion.y += -0.7f;
            moveDiretion.x += 0.7f;
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            moveDiretion.y += 0.7f;
            moveDiretion.x += -0.7f;
        }
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            moveDiretion.y += 0.7f;
            moveDiretion.x += 0.7f;
        }
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            moveDiretion.y += -0.7f;
            moveDiretion.x += -0.7f;
        }
        moveDiretion.x = Mathf.Clamp(moveDiretion.x, -0.7f, 0.7f);
        moveDiretion.y = Mathf.Clamp(moveDiretion.y, -0.7f, 0.7f);
        if (Input.GetAxisRaw("Horizontal") < 0 && Input.GetAxisRaw("Vertical") < 0)
        {
            moveDiretion.x += -1;
            moveDiretion.y = 0;
        }
        if (Input.GetAxisRaw("Horizontal") < 0 && Input.GetAxisRaw("Vertical") > 0)
        {
            moveDiretion.y += 1;
            moveDiretion.x = 0;
        }
        if (Input.GetAxisRaw("Horizontal") > 0 && Input.GetAxisRaw("Vertical") < 0)
        {
            moveDiretion.y += -1;
            moveDiretion.x = 0;
        }
        if (Input.GetAxisRaw("Horizontal") > 0 && Input.GetAxisRaw("Vertical") > 0)
        {
            moveDiretion.x += 1;
            moveDiretion.y = 0;
        }
        moveDiretion.x = Mathf.Clamp(moveDiretion.x, -1, 1);
        moveDiretion.y = Mathf.Clamp(moveDiretion.y, -1, 1);
        moveDiretion.x *= scaleX;
        moveDiretion.y *= scaleY;
        //moveDiretion = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        //moveDiretion = Quaternion.Euler(playerCamera.transform.eulerAngles.y, playerCamera.transform.eulerAngles.y, 0) * moveDiretion;
        //moveDiretion.y = moveDiretion.z;
        //moveDiretion.z = 0;
        //moveDiretion.Normalize();
        dot.position += moveDiretion * Time.deltaTime;
    }
}
