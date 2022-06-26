using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AvatarManager : MonoBehaviour
{
	public GameObject playerPrefab;

	private GameObject playerObj;
	private GameObject cameraObj;
	public Vector3 offset;

	void Start()
    {
        GameObject o = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);


        playerObj = o;
        cameraObj = Camera.main.gameObject;
        cameraObj.transform.position = playerObj.transform.position;
    }

    private void FixedUpdate()
    {
        if(playerObj!=null && cameraObj!=null)
        {
            cameraObj.transform.position = playerObj.transform.position - offset;
        }
    }

    public void CameraSet()
    {
		
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
