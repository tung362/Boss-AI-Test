using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    int previousCount;
    List<Actor> localCharacters
    { get { return Player.localPlayers.Select(x => x.character).Where(x => x != null).ToList(); } }

    public Vector3 offset;
    Vector3 averagePosition
    { get {
            Vector3 avg = Vector3.zero;
            foreach (Actor a in localCharacters) avg += a.transform.localPosition;
            return avg / localCharacters.Count;
    }     }

    List<Camera> cameras;

    public GameObject cameraPrefab;

    public float moveSpeed;
    public float rotationSpeed;

    public bool split;
    public bool wideSplit;

    bool follow
    { get { return localCharacters.Count != 0; } }

	// Use this for initialization
	void Start () {
        Init();
	}
	
	// Update is called once per frame
	void Update () {
        Move();
        CharacterCountCheck();
	}

    void Move()
    {
        if(follow)
        {
            if (split)
            {
                for(int i = 0; i < localCharacters.Count; ++i)
                {
                    cameras[i].transform.position = Vector3.Slerp(cameras[i].transform.position, 
                                                                  localCharacters[i].transform.position + offset, 
                                                                  moveSpeed * Time.deltaTime);
                    cameras[i].transform.rotation = Quaternion.Slerp(cameras[i].transform.rotation, 
                                                                     Quaternion.LookRotation(Vector3.Normalize(localCharacters[i].transform.position - cameras[i].transform.position)), 
                                                                     rotationSpeed * Time.deltaTime);
                }
            }
            else
            {
                cameras[0].transform.position = Vector3.Slerp(cameras[0].transform.position,
                                                              averagePosition + offset,
                                                              moveSpeed * Time.deltaTime);
                cameras[0].transform.rotation = Quaternion.Slerp(cameras[0].transform.rotation,
                                                                 Quaternion.LookRotation(Vector3.Normalize(averagePosition - cameras[0].transform.position)),
                                                                 rotationSpeed * Time.deltaTime);
            }
        }
    }

    void Init()
    {
        SetupCameraList();
    }

    void SetupCameraList()
    {
        cameras = new List<Camera>();
        cameras.Add(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>());

        for (int i = 0; i < 3; ++i)
            cameras.Add(Instantiate(cameraPrefab).GetComponent<Camera>());

        for (int i = 1; i < 4; ++i)
            cameras[i].gameObject.SetActive(false);

        AdjustCameraObjects();
    }

    // Check if the number of characters has changed
    void CharacterCountCheck()
    {
        if(previousCount != localCharacters.Count)
        {
            AdjustCameraObjects();
            previousCount = localCharacters.Count;
        }
    }

    // Adjust camera objects when character count changes in split screen
    void AdjustCameraObjects()
    {
        if (split)
        {
            for (int i = 0; i < localCharacters.Count; ++i)
                cameras[i].gameObject.SetActive(true);

            if (localCharacters.Count < 4)
                for (int i = localCharacters.Count; i < 4; ++i)
                    cameras[i].gameObject.SetActive(false);

            if (localCharacters.Count == 1)
                cameras[0].rect = new Rect(0, 0, 1, 1);
            else if (localCharacters.Count == 2)
            {
                if (wideSplit)
                {
                    cameras[0].rect = new Rect(0, 0, 1, .5f);
                    cameras[1].rect = new Rect(0, .5f, 1, 1);
                }
                else
                {
                    cameras[0].rect = new Rect(0, 0, .5f, 1);
                    cameras[1].rect = new Rect(.5f, 0, 1, 1);
                }
            }
            else if (localCharacters.Count == 3)
            {
                if (wideSplit)
                {
                    cameras[0].rect = new Rect(0, 0, 1, .5f);
                    cameras[1].rect = new Rect(0, .5f, .5f, 1);
                    cameras[2].rect = new Rect(.5f, .5f, 1, 1);
                }
                else
                {
                    cameras[0].rect = new Rect(0, 0, .5f, 1);
                    cameras[1].rect = new Rect(.5f, 0, 1, .5f);
                    cameras[2].rect = new Rect(.5f, .5f, 1, 1);
                }
            }
            else if (localCharacters.Count == 4)
            {
                cameras[0].rect = new Rect(0, 0, .5f, .5f);
                cameras[1].rect = new Rect(.5f, 0, 1, .5f);
                cameras[2].rect = new Rect(0, .5f, .5f, 1);
                cameras[3].rect = new Rect(.5f, .5f, 1, 1);
            }

            previousCount = localCharacters.Count;
        }
    }
}
