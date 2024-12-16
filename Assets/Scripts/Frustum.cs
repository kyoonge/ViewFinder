using UnityEngine;

public class Frustum
{
       Vector3 mLeftUpFrustum;
       Vector3 mRightUpFrustum;
       Vector3 mLeftDownFrustum;
       Vector3 mRightDownFrustum;
       Vector3 mCameraPos;
       Vector3 mForwardVector;

       Plane mLeftPlane;
       Plane mRightPlane;
       Plane mTopPlane;
       Plane mBottomPlane;

       float mAspectRatio;
       float mCustomOffset;

       public Frustum(float customOffset)
       {
              mCustomOffset = customOffset;
       }

       public void CalculateGeometry(Camera camera, Transform capturePoint)
       {
              mAspectRatio = camera.aspect;
              float frustumHeight = 2.0f * camera.farClipPlane * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
              float frustumWidth = frustumHeight * mAspectRatio;

              CalculateLocalPoints(frustumWidth, frustumHeight, camera.farClipPlane);
              TransformToWorldSpace(capturePoint);
              CreatePlanes();
       }

       void CalculateLocalPoints(float frustumWidth, float frustumHeight, float farClipPlane)
       {
              mLeftUpFrustum = new Vector3(-frustumWidth / 2, frustumHeight / 2, farClipPlane);
              mRightUpFrustum = new Vector3(frustumWidth / 2, frustumHeight / 2, farClipPlane);
              mLeftDownFrustum = new Vector3(-frustumWidth / 2, -frustumHeight / 2, farClipPlane);
              mRightDownFrustum = new Vector3(frustumWidth / 2, -frustumHeight / 2, farClipPlane);
       }

       void TransformToWorldSpace(Transform capturePoint)
       {
              mLeftUpFrustum = capturePoint.TransformPoint(mLeftUpFrustum);
              mRightUpFrustum = capturePoint.TransformPoint(mRightUpFrustum);
              mLeftDownFrustum = capturePoint.TransformPoint(mLeftDownFrustum);
              mRightDownFrustum = capturePoint.TransformPoint(mRightDownFrustum);

              mCameraPos = capturePoint.position;
              mForwardVector = capturePoint.forward;
       }

       void CreatePlanes()
       {
              mLeftPlane = new Plane(mCameraPos, mLeftUpFrustum, mLeftDownFrustum);
              mRightPlane = new Plane(mCameraPos, mRightDownFrustum, mRightUpFrustum);
              mTopPlane = new Plane(mCameraPos, mRightUpFrustum, mLeftUpFrustum);
              mBottomPlane = new Plane(mCameraPos, mLeftDownFrustum, mRightDownFrustum);
       }

       public Vector3 GetOffset(FrustumSide side)
       {
              switch (side)
              {
                     case FrustumSide.Left:
                            return mLeftPlane.normal * mCustomOffset;
                     case FrustumSide.Right:
                            return mRightPlane.normal * mCustomOffset;
                     case FrustumSide.Top:
                            return mTopPlane.normal * mCustomOffset;
                     case FrustumSide.Bottom:
                            return mBottomPlane.normal * mCustomOffset;
                     default:
                            return Vector3.zero;
              }
       }

       public Vector3 GetCenterPoint(FrustumSide side)
       {
              switch (side)
              {
                     case FrustumSide.Left:
                            return (mLeftDownFrustum + mLeftUpFrustum + mCameraPos) / 3;
                     case FrustumSide.Right:
                            return (mRightDownFrustum + mRightUpFrustum + mCameraPos) / 3;
                     case FrustumSide.Top:
                            return (mLeftUpFrustum + mRightUpFrustum + mCameraPos) / 3;
                     case FrustumSide.Bottom:
                            return (mLeftDownFrustum + mRightDownFrustum + mCameraPos) / 3;
                     default:
                            return Vector3.zero;
              }
       }

       public Plane GetPlane(FrustumSide side)
       {
              switch (side)
              {
                     case FrustumSide.Left:
                            return mLeftPlane;
                     case FrustumSide.Right:
                            return mRightPlane;
                     case FrustumSide.Top:
                            return mTopPlane;
                     case FrustumSide.Bottom:
                            return mBottomPlane;
                     default:
                            return new Plane();
              }
       }

       public Vector3 GetPoint(FrustumPoint point)
       {
              switch (point)
              {
                     case FrustumPoint.LeftUp:
                            return mLeftUpFrustum;
                     case FrustumPoint.RightUp:
                            return mRightUpFrustum;
                     case FrustumPoint.LeftDown:
                            return mLeftDownFrustum;
                     case FrustumPoint.RightDown:
                            return mRightDownFrustum;
                     default:
                            return Vector3.zero;
              }
       }

       public Vector3 GetCameraPosition()
       {
              return mCameraPos;
       }

       public Vector3 GetForwardVector()
       {
              return mForwardVector;
       }
}

public enum FrustumSide
{
       Left,
       Right,
       Top,
       Bottom
}

public enum FrustumPoint
{
       LeftUp,
       RightUp,
       LeftDown,
       RightDown
}