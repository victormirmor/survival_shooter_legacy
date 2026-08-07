using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_rotacion : MonoBehaviour{

    public Vector2 rotationSpeed;
    //public float rotationSpeed = 50f;

    private void Update()
    {

        Vector3 Rotation=new Vector3(rotationSpeed.x,0,rotationSpeed.y);

        transform.Rotate(Rotation * Time.deltaTime);
    }

}
