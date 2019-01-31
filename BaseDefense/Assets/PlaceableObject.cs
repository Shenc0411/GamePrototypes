using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlaceableObject : MonoBehaviour
{

    public MeshRenderer[] MRs;
    private Dictionary<MeshRenderer, Color> colorMap;
    private BoxCollider BC;
    private Vector3 halfExtent;
    private Vector3 center;

    public bool canBePlaced;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        MRs = GetComponent<Building>().MRs;

        colorMap = GetComponent<Building>().colorMap;

        BC = GetComponent<BoxCollider>();

        BC.enabled = false;

        halfExtent = new Vector3(BC.transform.localScale.x * BC.size.x, BC.transform.localScale.y * BC.size.y, BC.transform.localScale.z * BC.size.z);

        halfExtent /= 2.0f;

        SetTransparent();
    }

    // Update is called once per frame
    void Update()
    {
        //follow mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, GameManager.instance.groundLayer.value))
        {
            Vector3 point = hit.point;
            point.x = Mathf.Floor(point.x);
            point.z = Mathf.Floor(point.z);
            transform.position = point;

            UpdateColor();

            if (Input.GetMouseButtonUp(0) && canBePlaced)
            {
                //Place object
                BuildManager.instance.OnPlacingCompleted();
                SetIntransparent();
                BC.enabled = true;
                Destroy(this);

            }

            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                //Cancel placement
                BuildManager.instance.OnPlacingCancelled();
                Destroy(gameObject);
            }

        }
    }

    private void UpdateColor()
    {

        center = BC.center + BC.transform.position;

        if (Physics.OverlapBox(center, halfExtent, BC.transform.rotation, ~GameManager.instance.groundLayer.value).Length > 0)
        {
            canBePlaced = false;
            foreach (MeshRenderer MR in MRs)
            {

                Color color = Color.red * colorMap[MR];

                color.a = 0.5f;

                MR.material.color = color;

            }
        }
        else
        {
            canBePlaced = true;

            foreach (MeshRenderer MR in MRs)
            {

                Color color = Color.green * colorMap[MR];

                color.a = 0.5f;

                MR.material.color = color;

            }

        }

        
    }

    private void SetTransparent()
    {
        foreach(MeshRenderer MR in MRs)
        {
            
            MR.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            MR.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            MR.material.SetInt("_ZWrite", 0);
            MR.material.DisableKeyword("_ALPHATEST_ON");
            MR.material.DisableKeyword("_ALPHABLEND_ON");
            MR.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            MR.material.renderQueue = 3000;

        }
    }

    private void SetIntransparent()
    {
        foreach (MeshRenderer MR in MRs)
        {

            MR.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            MR.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            MR.material.SetInt("_ZWrite", 1);
            MR.material.DisableKeyword("_ALPHATEST_ON");
            MR.material.DisableKeyword("_ALPHABLEND_ON");
            MR.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            MR.material.renderQueue = -1;

            Color color = colorMap[MR];
            MR.material.color = color;
        }
    }

}
