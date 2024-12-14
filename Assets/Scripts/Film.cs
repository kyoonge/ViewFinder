using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Film
{
       List<GameObject> ObjectsInFilm;

       public Film(List<GameObject> objects, Transform parent)
       {
              ObjectsInFilm = new List<GameObject>();

              foreach (GameObject obj in objects)
              {
                     GameObject go = GameObject.Instantiate(obj);
                     go.transform.position = obj.transform.position;
                     go.transform.rotation = obj.transform.rotation;
                     go.transform.SetParent(parent);
                     go.SetActive(false);
                     ObjectsInFilm.Add(go);
              }
       }

       public void ActivateFilm()
       {
              for (int i = 0; i < ObjectsInFilm.Count; i++)
              {
                     ObjectsInFilm[i].transform.SetParent(null);
                     ObjectsInFilm[i].SetActive(true);
              }
       }
}
