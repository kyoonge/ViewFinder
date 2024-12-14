using Unity.VisualScripting;
using UnityEngine;

public class CollisionChecker: MonoBehaviour
{
       [HideInInspector]
       public CameraFrustum frustum;
       [HideInInspector]
       public int side;

       private void OnTriggerEnter(Collider other)
       {
              if (other.gameObject.layer != LayerMask.NameToLayer("Cuttable"))
                     return;

              other.gameObject.name = other.gameObject.name + " have num" + side;
              frustum.AddObjectToCut(other.gameObject, side);
       }

}
