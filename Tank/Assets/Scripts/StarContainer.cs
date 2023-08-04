using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StarContainer : MonoBehaviour
{
    [SerializeField] GameObject[] stars;
    public GameObject[] GetStar()
    {
        return stars;
    }
}
