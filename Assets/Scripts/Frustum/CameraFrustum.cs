using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEngine.Rendering.STP;

public class CameraFrustum : MonoBehaviour
{
       public float xRatio = 16;
       public float yRatio = 9;
       public float customOffset = 0.1f;

       //Camera finder;
       //Transform capturePoint;
       PlayerController controller;

       FrustumPlanes planes;
       CutResult curResult;
       bool isTakingPicture;
       FrustumCalculator.FrustumPoints curPoints;
       FrustumCalculator.FrustumPlanes curPlanes;

       void Start()
       {
              planes = new FrustumPlanes();
              SetupCollisionCheckers();
       }

       void SetupCollisionCheckers()
       {
              SetupChecker(planes.leftPlane, 0);
              SetupChecker(planes.rightPlane, 1);
              SetupChecker(planes.topPlane, 2);
              SetupChecker(planes.bottomPlane, 3);
              SetupChecker(planes.frustumObject, 4);
       }

       void SetupChecker(GameObject plane, int side)
       {
              CollisionChecker checker = plane.AddComponent<CollisionChecker>();
              checker.frustum = this;
              checker.side = side;
       }

       public void AddObjectToCut(GameObject go, int side)
       {
              if (curResult == null)
                     return;

              curResult.AddCut(go, side);
       }

       public void Cut(Camera cam, bool isTakingPic)
       {
              isTakingPicture = isTakingPic;
              curPoints = FrustumCalculator.CalculateFrustumPoints(cam, transform);
              curPlanes = planes.UpdateMeshes(curPoints, customOffset);

              curResult = new CutResult();
              planes.EnableColliders(true);

              StartCoroutine(CutObjectsRoutine());
       }

       IEnumerator CutObjectsRoutine()
       {
              yield return null;
              yield return null;
              yield return null;

              planes.EnableColliders(false);

              if (isTakingPicture)
                     ProcessPictureCut();
              //
              yield return new WaitForSeconds(0.5f);
       }

       void ProcessPictureCut()
       {
              var (allObjects, intactObjects) = CutObjects();
              //activeFilm = new PolaroidFilm(currentCut.objectsInFrustum, capturePoint);

              foreach (var obj in intactObjects)
                     obj.SetActive(true);

              foreach (var obj in allObjects)
                     if (obj != null)
                            Destroy(obj);
       }

       (List<GameObject> allObjects, List<GameObject> intactObjects) CutObjects()
       {
              var allObjects = new List<GameObject>();
              var originalObjects = new List<GameObject>();

              ProcessCutList(
                      curResult.leftCuts,
                      allObjects,
                      originalObjects,
                      (curPoints.leftDown + curPoints.leftUp + curPoints.cameraPosition) / 3,
                      curPlanes.left.normal
              );

              ProcessCutList(
                  curResult.rightCuts,
                  allObjects,
                  originalObjects,
                  (curPoints.rightDown + curPoints.rightUp + curPoints.cameraPosition) / 3,
                  curPlanes.right.normal
              );

              ProcessCutList(
                  curResult.topCuts,
                  allObjects,
                  originalObjects,
                  (curPoints.leftUp + curPoints.rightUp + curPoints.cameraPosition) / 3,
                  curPlanes.top.normal
              );

              ProcessCutList(
                  curResult.bottomCuts,
                  allObjects,
                  originalObjects,
                  (curPoints.leftDown + curPoints.rightDown + curPoints.cameraPosition) / 3,
                  curPlanes.bottom.normal
              );

              return (allObjects, originalObjects);
       }

       void ProcessCutList(List<GameObject> cutList, List<GameObject> allObjects, List<GameObject> originalObjects, Vector3 cutCenter, Vector3 cutNormal)
       {
              foreach (var obj in cutList)
              {
                     if (!allObjects.Contains(obj))
                     {
                            if (isTakingPicture)
                            {
                                   CreateIntactCopy(obj, originalObjects);
                            }
                            allObjects.Add(obj);
                     }

                     ProcessCutPiece(obj, allObjects, cutCenter, cutNormal);
              }
       }

       void CreateIntactCopy(GameObject original, List<GameObject> originalObjects)
       {
              var initialName = original.name;
              original.name = original.name + "/cut";
              var copy = Instantiate(original);
              copy.transform.position = original.transform.position;
              copy.transform.rotation = original.transform.rotation;
              copy.name = initialName;
              copy.SetActive(false);
              originalObjects.Add(copy);
       }

       void ProcessCutPiece(GameObject obj, List<GameObject> allObjects, Vector3 cutCenter, Vector3 cutNormal)
       {
              var cutPiece = obj.GetOrAddComponent<CutPiece>();
              cutPiece.AddPiece(obj);

              int initialCount = cutPiece.pieces.Count;
              for (int i = 0; i < initialCount; i++)
              {
                     var newPiece = Cutter.Cut(cutPiece.pieces[i], cutCenter, cutNormal);
                     if (newPiece != null)
                     {
                            cutPiece.AddPiece(newPiece);
                            allObjects.Add(newPiece);
                     }
              }
       }
}

// 프러스텀 평면들을 관리하는 클래스
public class FrustumPlanes
{
       public readonly GameObject leftPlane;
       public readonly GameObject rightPlane;
       public readonly GameObject topPlane;
       public readonly GameObject bottomPlane;
       public readonly GameObject frustumObject;

       private readonly MeshFilter leftPlaneMF, rightPlaneMF, topPlaneMF, bottomPlaneMF, frustumMF;
       private readonly MeshCollider leftPlaneMC, rightPlaneMC, topPlaneMC, bottomPlaneMC, frustumMC;

       public FrustumPlanes()
       {
              // 평면 초기화
              (leftPlane, leftPlaneMF, leftPlaneMC) = CreatePlane("LeftCameraPlane");
              (rightPlane, rightPlaneMF, rightPlaneMC) = CreatePlane("RightCameraPlane");
              (topPlane, topPlaneMF, topPlaneMC) = CreatePlane("TopCameraPlane");
              (bottomPlane, bottomPlaneMF, bottomPlaneMC) = CreatePlane("BottomCameraPlane");
              (frustumObject, frustumMF, frustumMC) = CreatePlane("FrustumObject");
       }

       (GameObject obj, MeshFilter mf, MeshCollider mc) CreatePlane(string name)
       {
              var obj = GameObject.CreatePrimitive(PrimitiveType.Plane);
              obj.name = name;

              var mf = obj.GetComponent<MeshFilter>();
              var mc = obj.GetComponent<MeshCollider>();
              var renderer = obj.GetComponent<MeshRenderer>();

              mc.convex = true;
              mc.isTrigger = true;
              mc.enabled = false;
              renderer.enabled = false;

              return (obj, mf, mc);
       }

       public void EnableColliders(bool enable)
       {
              leftPlaneMC.enabled = enable;
              rightPlaneMC.enabled = enable;
              topPlaneMC.enabled = enable;
              bottomPlaneMC.enabled = enable;
       }

       public FrustumCalculator.FrustumPlanes UpdateMeshes(FrustumCalculator.FrustumPoints points, float offset)
       {
              Mesh leftMesh, rightMesh, topMesh, bottomMesh;
              var planes = FrustumCalculator.CreateFrustumMeshes(points, offset, out leftMesh, out rightMesh, out topMesh, out bottomMesh);

              leftPlaneMF.mesh = leftMesh;
              leftPlaneMC.sharedMesh = leftMesh;

              rightPlaneMF.mesh = rightMesh;
              rightPlaneMC.sharedMesh = rightMesh;

              topPlaneMF.mesh = topMesh;
              topPlaneMC.sharedMesh = topMesh;

              bottomPlaneMF.mesh = bottomMesh;
              bottomPlaneMC.sharedMesh = bottomMesh;

              return planes;
       }
}

public class CutResult
{
       public List<GameObject> leftCuts = new();
       public List<GameObject> rightCuts = new();
       public List<GameObject> topCuts = new();
       public List<GameObject> bottomCuts = new();
       public List<GameObject> objectsInFrustum = new();
       public GameObject endingObject;

       public void AddCut(GameObject obj, int side)
       {
              switch (side)
              {
                     case 0: if (!leftCuts.Contains(obj)) leftCuts.Add(obj); break;
                     case 1: if (!rightCuts.Contains(obj)) rightCuts.Add(obj); break;
                     case 2: if (!topCuts.Contains(obj)) topCuts.Add(obj); break;
                     case 3: if (!bottomCuts.Contains(obj)) bottomCuts.Add(obj); break;
                     case 4: if (!objectsInFrustum.Contains(obj)) objectsInFrustum.Add(obj); break;
              }
       }
}