using System.Collections.Generic;
using UnityEngine;

public class CutPiece : MonoBehaviour
{
       public List<GameObject> pieces;

       public void AddPiece(GameObject piece)
       {
              if (pieces == null)
              {
                     pieces = new List<GameObject>();
              }
              pieces.Add(piece);
       }
}
