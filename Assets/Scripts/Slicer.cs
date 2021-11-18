
using UnityEngine;
using EzySlice;

using UnityEngine.UI;
public class Slicer : MonoBehaviour
{
    public Knife knife;
    public Material materialAfterSlice;
    public LayerMask sliceMask;
    public bool isTouched;
    public GameObject knifeObject;
    public Material slicedObjectInnerMaterial;

  
    private void Update()
    {
        if (isTouched == true)
        {
            isTouched = false;
            
            Collider[] objectsToBeSliced = Physics.OverlapBox(transform.position, new Vector3(1, 0.1f, 0.1f), transform.rotation, sliceMask);
            
            foreach (Collider objectToBeSliced in objectsToBeSliced)
            {
                SlicedHull slicedObject = SliceObject(objectToBeSliced.gameObject, materialAfterSlice);

                GameObject upperHullGameobject = slicedObject.CreateUpperHull(objectToBeSliced.gameObject, materialAfterSlice);
                GameObject lowerHullGameobject = slicedObject.CreateLowerHull(objectToBeSliced.gameObject, materialAfterSlice);
                upperHullGameobject.transform.position = objectToBeSliced.transform.position;
                lowerHullGameobject.transform.position = objectToBeSliced.transform.position;
                upperHullGameobject.tag = "Sliceable";
                lowerHullGameobject.tag = "Sliceable";
                knife.Score = PlayerData.Instance.SCORE;
                knife.Score += 1;
                knife.PlayerPrefsScore();
                UIManager.Instance.Score.text = "Score : " + knife.Score.ToString();
                MakeItPhysical(upperHullGameobject);
                MakeItPhysical(lowerHullGameobject);

                objectToBeSliced.gameObject.SetActive(false);
   
            }
        }
    }

    private void MakeItPhysical(GameObject obj)
    {
        obj.AddComponent<MeshCollider>().convex = true;
        obj.AddComponent<Rigidbody>();
    }

    private SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        return obj.Slice(transform.position, transform.up, crossSectionMaterial);
    }

   
}
