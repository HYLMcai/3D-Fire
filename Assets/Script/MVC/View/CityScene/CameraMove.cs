using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //玩家
    private GameObject player;
    //相机与玩家距离
    private Vector3 dir = new Vector3(0.36f, 14.85f, -9.63f);

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Init());

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        transform.position = Vector3.Lerp(transform.position, player.transform.position + dir, 5f * Time.deltaTime);
    }


    IEnumerator Init()
    {
        yield return new WaitForSeconds(1f);
        player = GameObject.Find("Player(Clone)").gameObject;
    }
}
