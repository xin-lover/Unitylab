using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Luwu.AR;
using Luwu.UI;
using Luwu.CloudAnchors;

public class CloudAnchorTest : MonoBehaviour {

    public GameObject prefab;
    public CloudAnchorService service;

	// Use this for initialization
	void Start () {
        CloudAnchorServiceWindow wnd = UIManager.instance.Get<CloudAnchorServiceWindow>();
        wnd.Show();
        wnd.SetAnchorCreatedHandler((trans) =>
        {
            Instantiate<GameObject>(prefab,trans);
        });

        wnd.SetAnchorResolvedHandler((trans) =>
        {
            Instantiate<GameObject>(prefab,trans);
        });

        wnd.SetCloudService(service);

	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
