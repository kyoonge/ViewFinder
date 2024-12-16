using UnityEngine;
using System.Collections.Generic;

public class PolaroidFilm
{
       List<GameObject> mPlaceHolders;
       float mCameraFov;
       float mAspectRatio;
       GameObject mBackgroundPrefab;
       Transform mCaptureTransform;
       float mBackgroundOffset;

       public PolaroidFilm(List<GameObject> objects, Transform parentToFollow, float cameraFov, float aspectRatio, GameObject backgroundPrefab, float backgroundOffset)
       {
              mPlaceHolders = new List<GameObject>();
              mCameraFov = cameraFov;
              mAspectRatio = aspectRatio;
              mBackgroundPrefab = backgroundPrefab;
              mCaptureTransform = parentToFollow;
              mBackgroundOffset = backgroundOffset;

              foreach (var obj in objects)
              {
                     var placeholder = GameObject.Instantiate(obj);
                     placeholder.transform.position = obj.transform.position;
                     placeholder.transform.rotation = obj.transform.rotation;
                     placeholder.transform.SetParent(parentToFollow);
                     placeholder.SetActive(false);
                     mPlaceHolders.Add(placeholder);
              }

              if (mBackgroundPrefab != null)
              {
                     CreateBackground();
              }
       }

       void CreateBackground()
       {
              float maxDistance = 0f;
              foreach (var obj in mPlaceHolders)
              {
                     float distance = Vector3.Distance(mCaptureTransform.position, obj.transform.position);
                     maxDistance = Mathf.Max(maxDistance, distance);
              }

              float backgroundDistance = maxDistance + mBackgroundOffset;

              float frustumHeight = 2.0f * backgroundDistance * Mathf.Tan(mCameraFov * 0.5f * Mathf.Deg2Rad);
              float frustumWidth = frustumHeight * mAspectRatio;

              var background = GameObject.Instantiate(mBackgroundPrefab);
              background.transform.position = mCaptureTransform.position + mCaptureTransform.forward * backgroundDistance;
              background.transform.rotation = mCaptureTransform.rotation;
              background.transform.localScale = new Vector3(frustumWidth, frustumHeight, 1);

              background.transform.SetParent(mCaptureTransform);
              background.SetActive(false);
              mPlaceHolders.Add(background);
       }

       public void ActivateFilm()
       {
              for (int i = 0; i < mPlaceHolders.Count; i++)
              {
                     mPlaceHolders[i].transform.SetParent(null);
                     mPlaceHolders[i].SetActive(true);
              }
       }
}