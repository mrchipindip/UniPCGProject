using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRandom : MonoBehaviour {

    public GameObject[] PlaceableItems;
    public GameObject[] pairObject;
    private int ItemNum;
    private int randNumGend;

    void Start () {
        ItemNum = PlaceableItems.Length;

        randNumGend = Random.Range(0, (ItemNum));

        PlaceableItems[randNumGend].SetActive(true);

        if (pairObject[0] != null)
        {
            pairObject[randNumGend].SetActive(true);
        }
	}
	
	// Update is called once per frame
	void Update () {

    }
}
