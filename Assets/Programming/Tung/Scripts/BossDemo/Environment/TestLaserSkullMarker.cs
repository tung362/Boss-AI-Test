using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLaserSkullMarker : MonoBehaviour
{
    public Vector3 Direction = Vector3.zero;
    public float CurrentValue = 0;
    public Vector3 TargetedPosition = Vector3.zero;
    public List<GameObject> Triangles = new List<GameObject>();

    /*Data*/
    private List<Vector3> TrianglesStartingPositions = new List<Vector3>();

	void Start ()
    {
        for (int i = 0; i < Triangles.Count; i++) TrianglesStartingPositions.Add(Triangles[i].transform.localPosition);
    }
	
	void Update ()
    {
        UpdateRotation();
        UpdateValue();
    }

    void UpdateRotation()
    {
        transform.Rotate(Direction * Time.deltaTime);
    }

    void UpdateValue()
    {
        CurrentValue = Mathf.Clamp(CurrentValue, 0, 1);

        for (int i = 0; i < Triangles.Count; i++) Triangles[i].transform.localPosition = Vector3.Lerp(TrianglesStartingPositions[i], TargetedPosition, CurrentValue);
    }
}
