using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [Range(0, 1)]
    public float smoothTime;

    public Transform playerTranform;

    [HideInInspector]
    public int worldSize;
    private float orthoSize;

    public void Spawn(Vector3 pos)
    {
        GetComponent<Transform>().position = pos;
        orthoSize = GetComponent<Camera>().orthographicSize;
    }

    public void FixedUpdate()
    {
        Vector3 pos = GetComponent<Transform>().position;

        pos.x = Mathf.Lerp(pos.x, playerTranform.position.x,smoothTime);
        pos.y = Mathf.Lerp(pos.y, playerTranform.position.y, smoothTime);

        pos.x = Mathf.Clamp(pos.x, orthoSize * 1.77f , worldSize - (orthoSize * 1.77f)); //cam border

        GetComponent<Transform>().position = pos;
    }
}
