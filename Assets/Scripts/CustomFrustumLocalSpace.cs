using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomFrustumLocalSpace : MonoBehaviour
{
       [Header("Camera Settings")]
       public Camera finder;
       public float xRatio = 16;
       public float yRatio = 9;
       public float customOffset = 0.1f;
       public Transform capturePoint;
       public PlayerController controller;

       GameObject mLeftPrimitivePlane, mRightPrimitivePlane, mTopPrimitivePlane, mBottomPrimitivePlane, mFrustumObject;
       MeshFilter mLeftPrimitivePlaneMF, mRightPrimitivePlaneMF, mTopPrimitivePlaneMF, mBottomPrimitivePlaneMF, mFrustumObjectMF;
       MeshCollider mLeftPrimitivePlaneMC, mRightPrimitivePlaneMC, mTopPrimitivePlaneMC, mBottomPrimitivePlaneMC, mFrustumObjectMC;

       List<GameObject> mLeftToCut;
       List<GameObject> mRightToCut;
       List<GameObject> mTopToCut;
       List<GameObject> mBottomToCut;
       List<GameObject> mObjectsInFrustum;

       bool mIsTakingPicture;
       GameObject mEnding;
       PolaroidFilm mActiveFilm;
       Frustum mFrustum;

       void Start()
       {
              InitializePrimitivePlanes();
              SetupMeshColliders();
              SetupMeshFilters();
              SetupMeshRenderers();
              SetupCollisionCheckers();
              mFrustum = new Frustum(customOffset);
       }

       void InitializePrimitivePlanes()
       {
              mLeftPrimitivePlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
              mLeftPrimitivePlane.name = "LeftCameraPlane";

              mRightPrimitivePlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
              mRightPrimitivePlane.name = "RightCameraPlane";

              mTopPrimitivePlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
              mTopPrimitivePlane.name = "TopCameraPlane";

              mBottomPrimitivePlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
              mBottomPrimitivePlane.name = "BottomCameraPlane";

              mFrustumObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
              mFrustumObject.name = "FrustumObject";
       }

       void SetupMeshColliders()
       {
              mLeftPrimitivePlaneMC = mLeftPrimitivePlane.GetComponent<MeshCollider>();
              mLeftPrimitivePlaneMC.convex = true;
              mLeftPrimitivePlaneMC.isTrigger = true;
              mLeftPrimitivePlaneMC.enabled = false;

              mRightPrimitivePlaneMC = mRightPrimitivePlane.GetComponent<MeshCollider>();
              mRightPrimitivePlaneMC.convex = true;
              mRightPrimitivePlaneMC.isTrigger = true;
              mRightPrimitivePlaneMC.enabled = false;

              mTopPrimitivePlaneMC = mTopPrimitivePlane.GetComponent<MeshCollider>();
              mTopPrimitivePlaneMC.convex = true;
              mTopPrimitivePlaneMC.isTrigger = true;
              mTopPrimitivePlaneMC.enabled = false;

              mBottomPrimitivePlaneMC = mBottomPrimitivePlane.GetComponent<MeshCollider>();
              mBottomPrimitivePlaneMC.convex = true;
              mBottomPrimitivePlaneMC.isTrigger = true;
              mBottomPrimitivePlaneMC.enabled = false;

              mFrustumObjectMC = mFrustumObject.GetComponent<MeshCollider>();
              mFrustumObjectMC.convex = true;
              mFrustumObjectMC.isTrigger = true;
              mFrustumObjectMC.enabled = false;
       }

       void SetupMeshFilters()
       {
              mLeftPrimitivePlaneMF = mLeftPrimitivePlane.GetComponent<MeshFilter>();
              mRightPrimitivePlaneMF = mRightPrimitivePlane.GetComponent<MeshFilter>();
              mTopPrimitivePlaneMF = mTopPrimitivePlane.GetComponent<MeshFilter>();
              mBottomPrimitivePlaneMF = mBottomPrimitivePlane.GetComponent<MeshFilter>();
              mFrustumObjectMF = mFrustumObject.GetComponent<MeshFilter>();
       }

       void SetupMeshRenderers()
       {
              mLeftPrimitivePlane.GetComponent<MeshRenderer>().enabled = false;
              mRightPrimitivePlane.GetComponent<MeshRenderer>().enabled = false;
              mTopPrimitivePlane.GetComponent<MeshRenderer>().enabled = false;
              mBottomPrimitivePlane.GetComponent<MeshRenderer>().enabled = false;
              mFrustumObjectMF.GetComponent<MeshRenderer>().enabled = false;
       }

       void SetupCollisionCheckers()
       {
              var leftChecker = mLeftPrimitivePlane.AddComponent<CollisionChecker>();
              leftChecker.frustumLocalSpace = this;
              leftChecker.side = 0;

              var rightChecker = mRightPrimitivePlane.AddComponent<CollisionChecker>();
              rightChecker.frustumLocalSpace = this;
              rightChecker.side = 1;

              var topChecker = mTopPrimitivePlane.AddComponent<CollisionChecker>();
              topChecker.frustumLocalSpace = this;
              topChecker.side = 2;

              var bottomChecker = mBottomPrimitivePlane.AddComponent<CollisionChecker>();
              bottomChecker.frustumLocalSpace = this;
              bottomChecker.side = 3;

              var frustumChecker = mFrustumObject.AddComponent<CollisionChecker>();
              frustumChecker.frustumLocalSpace = this;
              frustumChecker.side = 4;
       }

       public void Cut(bool isTakingPic)
       {
              mIsTakingPicture = isTakingPic;
              SetupFrustumGeometry();
              InitializeCutLists();
              EnableColliders();
              StartCoroutine(TestCut(mIsTakingPicture));
       }

       void SetupFrustumGeometry()
       {
              mFrustum.CalculateGeometry(finder, capturePoint);
              CreateFrustumMeshes();
       }

       void CreateFrustumMeshes()
       {
              // Left plane
              var leftOffset = mFrustum.GetOffset(FrustumSide.Left);
              mLeftPrimitivePlaneMF.mesh = CreateBoxMesh(
                  mFrustum.GetCameraPosition(),
                  mFrustum.GetPoint(FrustumPoint.LeftUp),
                  (mFrustum.GetPoint(FrustumPoint.LeftUp) + mFrustum.GetPoint(FrustumPoint.LeftDown)) / 2,
                  mFrustum.GetPoint(FrustumPoint.LeftDown),
                  mFrustum.GetPoint(FrustumPoint.LeftDown) + leftOffset,
                  ((mFrustum.GetPoint(FrustumPoint.LeftUp) + mFrustum.GetPoint(FrustumPoint.LeftDown)) / 2) + leftOffset,
                  mFrustum.GetPoint(FrustumPoint.LeftUp) + leftOffset,
                  mFrustum.GetCameraPosition() + leftOffset
              );
              mLeftPrimitivePlaneMC.sharedMesh = mLeftPrimitivePlaneMF.mesh;

              // Right plane
              var rightOffset = mFrustum.GetOffset(FrustumSide.Right);
              mRightPrimitivePlaneMF.mesh = CreateBoxMesh(
                  mFrustum.GetCameraPosition(),
                  mFrustum.GetPoint(FrustumPoint.RightDown),
                  (mFrustum.GetPoint(FrustumPoint.RightUp) + mFrustum.GetPoint(FrustumPoint.RightDown)) / 2,
                  mFrustum.GetPoint(FrustumPoint.RightUp),
                  mFrustum.GetPoint(FrustumPoint.RightUp) + rightOffset,
                  ((mFrustum.GetPoint(FrustumPoint.RightUp) + mFrustum.GetPoint(FrustumPoint.RightDown)) / 2) + rightOffset,
                  mFrustum.GetPoint(FrustumPoint.RightDown) + rightOffset,
                  mFrustum.GetCameraPosition() + rightOffset
              );
              mRightPrimitivePlaneMC.sharedMesh = mRightPrimitivePlaneMF.mesh;

              // Top plane
              var topOffset = mFrustum.GetOffset(FrustumSide.Top);
              mTopPrimitivePlaneMF.mesh = CreateBoxMesh(
                  mFrustum.GetCameraPosition(),
                  mFrustum.GetPoint(FrustumPoint.RightUp),
                  (mFrustum.GetPoint(FrustumPoint.LeftUp) + mFrustum.GetPoint(FrustumPoint.RightUp)) / 2,
                  mFrustum.GetPoint(FrustumPoint.LeftUp),
                  mFrustum.GetPoint(FrustumPoint.LeftUp) + topOffset,
                  ((mFrustum.GetPoint(FrustumPoint.LeftUp) + mFrustum.GetPoint(FrustumPoint.RightUp)) / 2) + topOffset,
                  mFrustum.GetPoint(FrustumPoint.RightUp) + topOffset,
                  mFrustum.GetCameraPosition() + topOffset
              );
              mTopPrimitivePlaneMC.sharedMesh = mTopPrimitivePlaneMF.mesh;

              // Bottom plane
              var bottomOffset = mFrustum.GetOffset(FrustumSide.Bottom);
              mBottomPrimitivePlaneMF.mesh = CreateBoxMesh(
                  mFrustum.GetCameraPosition(),
                  mFrustum.GetPoint(FrustumPoint.LeftDown),
                  (mFrustum.GetPoint(FrustumPoint.LeftDown) + mFrustum.GetPoint(FrustumPoint.RightDown)) / 2,
                  mFrustum.GetPoint(FrustumPoint.RightDown),
                  mFrustum.GetPoint(FrustumPoint.RightDown) + bottomOffset,
                  ((mFrustum.GetPoint(FrustumPoint.LeftDown) + mFrustum.GetPoint(FrustumPoint.RightDown)) / 2) + bottomOffset,
                  mFrustum.GetPoint(FrustumPoint.LeftDown) + bottomOffset,
                  mFrustum.GetCameraPosition() + bottomOffset
              );
              mBottomPrimitivePlaneMC.sharedMesh = mBottomPrimitivePlaneMF.mesh;
       }

       void InitializeCutLists()
       {
              mLeftToCut = new List<GameObject>();
              mRightToCut = new List<GameObject>();
              mTopToCut = new List<GameObject>();
              mBottomToCut = new List<GameObject>();
              mObjectsInFrustum = new List<GameObject>();
              mEnding = null;
       }

       void EnableColliders()
       {
              mLeftPrimitivePlaneMC.enabled = true;
              mRightPrimitivePlaneMC.enabled = true;
              mTopPrimitivePlaneMC.enabled = true;
              mBottomPrimitivePlaneMC.enabled = true;
       }

       IEnumerator TestCut(bool isTakingPicture)
       {
              yield return null;
              yield return null;
              yield return null;

              DisableColliders();

              List<GameObject> allObjects = new List<GameObject>();
              List<GameObject> intactObjects = new List<GameObject>();

              ProcessLeftSideCuts(isTakingPicture, allObjects, intactObjects);
              ProcessRightSideCuts(isTakingPicture, allObjects, intactObjects);
              ProcessTopSideCuts(isTakingPicture, allObjects, intactObjects);
              ProcessBottomSideCuts(isTakingPicture, allObjects, intactObjects);

              PrepareFrustumObject();

              mFrustumObjectMC.enabled = true;

              yield return null;
              yield return null;
              yield return null;

              mFrustumObjectMC.enabled = false;

              if (mEnding != null)
                     mObjectsInFrustum.Add(mEnding);

              ProcessFinalResult(isTakingPicture, allObjects, intactObjects);

              yield return new WaitForSeconds(0.5f);
       }

       void DisableColliders()
       {
              mLeftPrimitivePlaneMC.enabled = false;
              mRightPrimitivePlaneMC.enabled = false;
              mTopPrimitivePlaneMC.enabled = false;
              mBottomPrimitivePlaneMC.enabled = false;
       }

       void ProcessLeftSideCuts(bool isTakingPicture, List<GameObject> allObjects, List<GameObject> intactObjects)
       {
              foreach (var obj in mLeftToCut)
              {
                     if (isTakingPicture)
                     {
                            CreateIntactObject(obj, intactObjects);
                     }

                     allObjects.Add(obj);

                     var cutPiece = GetOrAddCutPiece(obj);
                     var newPiece = Cutter.Cut(obj, mFrustum.GetCenterPoint(FrustumSide.Left), mFrustum.GetPlane(FrustumSide.Left).normal);
                     cutPiece.AddChunk(newPiece);
                     allObjects.Add(newPiece);
              }
       }

       void ProcessRightSideCuts(bool isTakingPicture, List<GameObject> allObjects, List<GameObject> intactObjects)
       {
              foreach (var obj in mRightToCut)
              {
                     if (isTakingPicture)
                     {
                            var s = obj.name.Split('/');
                            if (s.Length == 1)
                            {
                                   CreateIntactObject(obj, intactObjects);
                            }
                     }

                     if (!allObjects.Contains(obj))
                     {
                            allObjects.Add(obj);
                     }

                     var cutPiece = GetOrAddCutPiece(obj);
                     int initialCount = cutPiece.chunks.Count;
                     for (int i = 0; i < initialCount; i++)
                     {
                            var newPiece = Cutter.Cut(cutPiece.chunks[i], mFrustum.GetCenterPoint(FrustumSide.Right), mFrustum.GetPlane(FrustumSide.Right).normal);
                            cutPiece.AddChunk(newPiece);
                            allObjects.Add(newPiece);
                     }
              }
       }

       void ProcessTopSideCuts(bool isTakingPicture, List<GameObject> allObjects, List<GameObject> intactObjects)
       {
              foreach (var obj in mTopToCut)
              {
                     var s = obj.name.Split('/');
                     if (s.Length == 1)
                     {
                            CreateIntactObject(obj, intactObjects);
                     }

                     if (!allObjects.Contains(obj))
                     {
                            allObjects.Add(obj);
                     }

                     var cutPiece = GetOrAddCutPiece(obj);
                     int initialCount = cutPiece.chunks.Count;
                     for (int i = 0; i < initialCount; i++)
                     {
                            var newPiece = Cutter.Cut(cutPiece.chunks[i], mFrustum.GetCenterPoint(FrustumSide.Top), mFrustum.GetPlane(FrustumSide.Top).normal);
                            cutPiece.AddChunk(newPiece);
                            allObjects.Add(newPiece);
                     }
              }
       }

       void ProcessBottomSideCuts(bool isTakingPicture, List<GameObject> allObjects, List<GameObject> intactObjects)
       {
              foreach (var obj in mBottomToCut)
              {
                     var s = obj.name.Split('/');
                     if (s.Length == 1)
                     {
                            CreateIntactObject(obj, intactObjects);
                     }

                     if (!allObjects.Contains(obj))
                     {
                            allObjects.Add(obj);
                     }

                     var cutPiece = GetOrAddCutPiece(obj);
                     int initialCount = cutPiece.chunks.Count;
                     for (int i = 0; i < initialCount; i++)
                     {
                            var newPiece = Cutter.Cut(cutPiece.chunks[i], mFrustum.GetCenterPoint(FrustumSide.Bottom), mFrustum.GetPlane(FrustumSide.Bottom).normal);
                            cutPiece.AddChunk(newPiece);
                            allObjects.Add(newPiece);
                     }
              }
       }

       void PrepareFrustumObject()
       {
              //need to add a little margin aiming inside
              mFrustumObjectMF.mesh = CreateFrustumObject(
                  mFrustum.GetCameraPosition() + (mFrustum.GetForwardVector() * -customOffset),
                  mFrustum.GetPoint(FrustumPoint.RightDown) + (mFrustum.GetPlane(FrustumSide.Right).normal * -customOffset),
                  mFrustum.GetPoint(FrustumPoint.RightUp) + (mFrustum.GetPlane(FrustumSide.Right).normal * -customOffset),
                  mFrustum.GetPoint(FrustumPoint.LeftUp) + (mFrustum.GetPlane(FrustumSide.Left).normal * -customOffset),
                  mFrustum.GetPoint(FrustumPoint.LeftDown) + (mFrustum.GetPlane(FrustumSide.Left).normal * -customOffset)
              );
              mFrustumObjectMC.sharedMesh = mFrustumObjectMF.mesh;
       }

       void ProcessFinalResult(bool isTakingPicture, List<GameObject> allObjects, List<GameObject> intactObjects)
       {
              if (isTakingPicture)
              {
                     mActiveFilm = new PolaroidFilm(mObjectsInFrustum, capturePoint);

                     foreach (var i in intactObjects)
                            i.SetActive(true);

                     foreach (var obj in allObjects)
                     {
                            if (obj != null)
                                   Destroy(obj);
                     }
              }
              else
              {
                     foreach (var obj in allObjects)
                            Destroy(obj.GetComponent<CutPiece>());

                     foreach (var obj in mObjectsInFrustum)
                            Destroy(obj);

                     mActiveFilm.ActivateFilm();
              }
       }

       CutPiece GetOrAddCutPiece(GameObject obj)
       {
              var cutPiece = obj.GetComponent<CutPiece>();
              if (cutPiece == null)
              {
                     cutPiece = obj.AddComponent<CutPiece>();
                     cutPiece.AddChunk(obj);
              }
              return cutPiece;
       }

       void CreateIntactObject(GameObject obj, List<GameObject> intactObjects)
       {
              var initialName = obj.name;
              obj.name = obj.name + "/cut";
              var original = Instantiate(obj);
              original.transform.position = obj.transform.position;
              original.transform.rotation = obj.transform.rotation;
              original.name = initialName;
              original.SetActive(false);
              intactObjects.Add(original);
       }

       public void AddObjectToCut(GameObject toCut, int side)
       {
              switch (side)
              {
                     case 0:
                            if (!mLeftToCut.Contains(toCut))
                                   mLeftToCut.Add(toCut);
                            break;
                     case 1:
                            if (!mRightToCut.Contains(toCut))
                                   mRightToCut.Add(toCut);
                            break;
                     case 2:
                            if (!mTopToCut.Contains(toCut))
                                   mTopToCut.Add(toCut);
                            break;
                     case 3:
                            if (!mBottomToCut.Contains(toCut))
                                   mBottomToCut.Add(toCut);
                            break;
                     case 4:
                            if (!mObjectsInFrustum.Contains(toCut))
                                   mObjectsInFrustum.Add(toCut);
                            break;
              }
       }

       public void AddEndingObject(GameObject end)
       {
              mEnding = end;
       }

       Mesh CreateBoxMesh(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector3 v5, Vector3 v6, Vector3 v7, Vector3 v8)
       {
              Vector3[] vertices = new Vector3[] {
            v1, v2, v3, v4, v5, v6, v7, v8
        };

              int[] triangles = new int[] {
            0, 1, 2,
            0, 2, 3,
            3, 2, 5,
            3, 5, 4,
            2, 1, 6,
            2, 6, 5,
            7, 4, 5,
            7, 5, 6,
            0, 1, 6,
            0, 6, 7,
            0, 7, 4,
            0, 4, 3
        };

              var mesh = new Mesh
              {
                     vertices = vertices,
                     triangles = triangles,
              };

              return mesh;
       }

       Mesh CreateFrustumObject(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector3 v5)
       {
              Vector3[] vertices = new Vector3[] {
            v1, v2, v3, v4, v5
        };

              int[] triangles = new int[] {
            0, 2, 1,
            4, 1, 2,
            4, 2, 3,
            0, 4, 3,
            0, 1, 4,
            0, 3, 2,
        };

              var mesh = new Mesh
              {
                     vertices = vertices,
                     triangles = triangles,
              };

              return mesh;
       }

       void OnDrawGizmos()
       {
              if (mFrustum == null || finder == null || capturePoint == null) return;

              Gizmos.color = Color.yellow;

              var cameraPosition = capturePoint.position;
              Vector3 leftUpPoint = mFrustum.GetPoint(FrustumPoint.LeftUp);
              Vector3 rightUpPoint = mFrustum.GetPoint(FrustumPoint.RightUp);
              Vector3 leftDownPoint = mFrustum.GetPoint(FrustumPoint.LeftDown);
              Vector3 rightDownPoint = mFrustum.GetPoint(FrustumPoint.RightDown);

              Gizmos.DrawLine(cameraPosition, rightUpPoint);
              Gizmos.DrawLine(cameraPosition, leftUpPoint);
              Gizmos.DrawLine(cameraPosition, rightDownPoint);
              Gizmos.DrawLine(cameraPosition, leftDownPoint);

              Gizmos.DrawLine(leftDownPoint, rightDownPoint);
              Gizmos.DrawLine(leftUpPoint, rightUpPoint);

              Gizmos.DrawLine(leftDownPoint, leftUpPoint);
              Gizmos.DrawLine(rightDownPoint, rightUpPoint);
       }
}