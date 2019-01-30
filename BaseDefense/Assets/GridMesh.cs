using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMesh : MonoBehaviour
{

    public int gridSize = 10;


    private void Awake()
    {
        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        var mesh = new Mesh();
        var verticies = new List<Vector3>();

        var indicies = new List<int>();

        Vector3 offset = new Vector3(-gridSize / 2.0f + 0.5f, 0, -gridSize / 2.0f + 0.5f);

        for (int i = 0; i < gridSize; i++)
        {
            verticies.Add(offset + new Vector3(i, 0, 0));
            verticies.Add(offset + new Vector3(i, 0, gridSize));

            indicies.Add(4 * i + 0);
            indicies.Add(4 * i + 1);

            verticies.Add(offset + new Vector3(0, 0, i));
            verticies.Add(offset + new Vector3(gridSize, 0, i));

            indicies.Add(4 * i + 2);
            indicies.Add(4 * i + 3);
        }

        mesh.vertices = verticies.ToArray();
        mesh.SetIndices(indicies.ToArray(), MeshTopology.Lines, 0);
        filter.mesh = mesh;

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
        meshRenderer.material.color = Color.green;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, GameManager.instance.groundLayer.value))
        {
            Vector3 point = hit.point;
            point.x = Mathf.Floor(point.x);
            point.z = Mathf.Floor(point.z);
            transform.position = point;
        }
    }
}
