using System.Collections.Generic;
using UnityEngine;

public class PolaroidFilm
{
       List<GameObject> placeHolders;
       public PolaroidFilm(List<GameObject> obj, Transform parentToFollow)
       {
              placeHolders = new List<GameObject>();
              foreach (var o in obj)
              {
                     var placeholder = GameObject.Instantiate(o);
                     placeholder.transform.position = o.transform.position;
                     placeholder.transform.rotation = o.transform.rotation;
                     placeholder.transform.SetParent(parentToFollow);
                     placeholder.SetActive(false);
                     placeHolders.Add(placeholder);
              }
       }

       public void ActivateFilm()
       {
              for (int i = 0; i < placeHolders.Count; i++)
              {
                     placeHolders[i].transform.SetParent(null);
                     placeHolders[i].SetActive(true);
              }
       }
}