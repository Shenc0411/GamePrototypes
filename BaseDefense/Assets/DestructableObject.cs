using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{

    public MeshRenderer[] MRs;
    private Dictionary<MeshRenderer, Color> colorMap = new Dictionary<MeshRenderer, Color>();

    void Awake()
    {

    }

    private void Start()
    {
        MRs = GetComponent<Building>().MRs;

        colorMap = GetComponent<Building>().colorMap;
    }

    public void Highlight()
    {
        foreach (MeshRenderer MR in MRs)
        {

            Color color = Color.yellow * colorMap[MR];

            MR.material.color = color;

        }
    }

    public void DeHighlight()
    {
        foreach (MeshRenderer MR in MRs)
        {

            MR.material.color = colorMap[MR];

        }
    }
}
