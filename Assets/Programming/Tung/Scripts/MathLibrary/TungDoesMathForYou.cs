using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using UnityEngine.UI;

//Tung's math/tools library
public class TungDoesMathForYou : MonoBehaviour
{
    //Returns a list of all child objects
    public List<GameObject> FindAllChilds(Transform TheGameObject)
    {
        List<GameObject> retval = new List<GameObject>();
        foreach (Transform child in TheGameObject)
        {
            retval.Add(child.gameObject);
            retval.AddRange(FindAllChilds(child));
        }
        return retval;
    }

    //Apply layer to all childs of a transform
    public void ApplyLayerToChilds(Transform TheGameObject, string LayerName)
    {
        foreach (Transform child in TheGameObject)
        {
            child.gameObject.layer = LayerMask.NameToLayer(LayerName);
            ApplyLayerToChilds(child, LayerName);
        }
    }

    //Returns the parent gameobject that has the selected component, null if non
    public GameObject FindParentObjectWithComponent(Transform TheGameObject, string TypeName, bool CanReturnSelf)
    {
        //Returns null by default
        GameObject retval = null;
        //If we can search own object for the component
        if(CanReturnSelf)
        {
            Component existingComponent = TheGameObject.GetComponent(TypeName);
            if (existingComponent != null) return existingComponent.gameObject;
        }
        //If this object does have a parent and the child doesn't have the targeted component
        if(TheGameObject.parent != null)
        {
            Component existingComponent = TheGameObject.parent.GetComponent(TypeName);
            //If the component exists or not
            if (existingComponent == null) retval = FindParentObjectWithComponent(TheGameObject.parent, TypeName, false);
            else retval = existingComponent.gameObject;
        }
        return retval;
    }

    //Be sure to have values between 0-255
    public Color Vector4ToColor(Vector4 p)
    {
        Color c = new Color();
        if (p.x > 255 || p.y > 255 || p.z > 255 || p.w > 255)
        {
            Debug.Log("Im not going to do this for you, make your damn values below 255, look up color values if you don't know");
            Debug.Log("Im turning it to red one of my favorite colors because you did it wrong! SHAME!");
            return c = new Vector4(1, 0, 0, 1);
        }
        return c = new Vector4(p.x / 255, p.y / 255, p.z / 255, p.w / 255);
    }

    public List<MeshRenderer> FindAllMeshes(Transform TheGameObject)
    {
        List<MeshRenderer> retval = new List<MeshRenderer>();
        foreach (Transform child in TheGameObject)
        {
            if (child.GetComponent<MeshRenderer>() != null) retval.Add(child.GetComponent<MeshRenderer>());
            retval.AddRange(FindAllMeshes(child));
        }
        return retval;
    }
}
