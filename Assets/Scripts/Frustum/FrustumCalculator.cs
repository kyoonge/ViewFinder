using UnityEngine;

public class FrustumCalculator
{
       public struct FrustumPoints
       {
              public Vector3 leftUp;
              public Vector3 rightUp;
              public Vector3 leftDown;
              public Vector3 rightDown;
              public Vector3 cameraPosition;
       }
       public struct FrustumPlanes
       {
              public Plane left;
              public Plane right;
              public Plane top;
              public Plane bottom;
       }

       // 절두체의 좌표 계산
       public static FrustumPoints CalculateFrustumPoints(Camera camera, Transform capturePoint)
       {
              float frustumHeight = 2.0f * camera.farClipPlane * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
              float frustumWidth = frustumHeight * camera.aspect;

              Vector3 leftUp = new Vector3(-frustumWidth / 2, frustumHeight / 2, camera.farClipPlane);
              Vector3 rightUp = new Vector3(frustumWidth / 2, frustumHeight / 2, camera.farClipPlane);
              Vector3 leftDown = new Vector3(-frustumWidth / 2, -frustumHeight / 2, camera.farClipPlane);
              Vector3 rightDown = new Vector3(frustumWidth / 2, -frustumHeight / 2, camera.farClipPlane);

              return new FrustumPoints
              {
                     leftUp = capturePoint.TransformPoint(leftUp),
                     rightUp = capturePoint.TransformPoint(rightUp),
                     leftDown = capturePoint.TransformPoint(leftDown),
                     rightDown = capturePoint.TransformPoint(rightDown),
                     cameraPosition = capturePoint.position
              };
       }

       // 절두체를 구성하는 4개의 평면 생성 (세 점의 좌표 이용)
       public static FrustumPlanes CalculateFrustumPlanes(FrustumPoints points)
       {
              return new FrustumPlanes
              {
                     left = new Plane(points.cameraPosition, points.leftUp, points.leftDown),
                     right = new Plane(points.cameraPosition, points.rightDown, points.rightUp),
                     top = new Plane(points.cameraPosition, points.rightUp, points.leftUp),
                     bottom = new Plane(points.cameraPosition, points.leftDown, points.rightDown)
              };
       }

       public static FrustumPlanes CreateFrustumMeshes(FrustumPoints points, float offset,
        out Mesh leftMesh, out Mesh rightMesh, out Mesh topMesh, out Mesh bottomMesh)
       {
              var planes = CalculateFrustumPlanes(points);

              Vector3 leftOffset = planes.left.normal * offset;
              Vector3 rightOffset = planes.right.normal * offset;
              Vector3 topOffset = planes.top.normal * offset;
              Vector3 bottomOffset = planes.bottom.normal * offset;

              leftMesh = CreateBoxMesh(points.cameraPosition, points.leftUp,
                  (points.leftUp + points.leftDown) / 2, points.leftDown,
                  points.leftDown + leftOffset, ((points.leftUp + points.leftDown) / 2) + leftOffset,
                  points.leftUp + leftOffset, points.cameraPosition + leftOffset);

              rightMesh = CreateBoxMesh(points.cameraPosition, points.rightDown,
                  (points.rightUp + points.rightDown) / 2, points.rightUp,
                  points.rightUp + rightOffset, ((points.rightUp + points.rightDown) / 2) + rightOffset,
                  points.rightDown + rightOffset, points.cameraPosition + rightOffset);

              topMesh = CreateBoxMesh(points.cameraPosition, points.rightUp,
                  (points.leftUp + points.rightUp) / 2, points.leftUp,
                  points.leftUp + topOffset, ((points.leftUp + points.rightUp) / 2) + topOffset,
                  points.rightUp + topOffset, points.cameraPosition + topOffset);

              bottomMesh = CreateBoxMesh(points.cameraPosition, points.leftDown,
                  (points.leftDown + points.rightDown) / 2, points.rightDown,
                  points.rightDown + bottomOffset, ((points.leftDown + points.rightDown) / 2) + bottomOffset,
                  points.leftDown + bottomOffset, points.cameraPosition + bottomOffset);

              return planes;
       }

       static Mesh CreateBoxMesh(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4,
        Vector3 v5, Vector3 v6, Vector3 v7, Vector3 v8)
       {
              Vector3[] vertices = new[] { v1, v2, v3, v4, v5, v6, v7, v8 };
              int[] triangles = new[] 
              {   //vertices 배열의 인덱스
                   0, 1, 2, 0, 2, 3,
                   3, 2, 5, 3, 5, 4,
                   2, 1, 6, 2, 6, 5,
                   7, 4, 5, 7, 5, 6,
                   0, 1, 6, 0, 6, 7,
                   0, 7, 4, 0, 4, 3
               };

              return new Mesh { vertices = vertices, triangles = triangles };
       }

       public static Mesh CreateFrustumVolume(FrustumPoints points, Vector3 forwardVector, float offset, FrustumPlanes planes)
       {
              // 프러스텀 내부를 향하는 오프셋 적용
              Vector3 cameraPos = points.cameraPosition + (forwardVector * -offset);
              Vector3 rightDownOffset = points.rightDown + (planes.right.normal * -offset);
              Vector3 rightUpOffset = points.rightUp + (planes.right.normal * -offset);
              Vector3 leftUpOffset = points.leftUp + (planes.left.normal * -offset);
              Vector3 leftDownOffset = points.leftDown + (planes.left.normal * -offset);

              Vector3[] vertices = new[] { cameraPos, rightDownOffset, rightUpOffset, leftUpOffset, leftDownOffset };
              int[] triangles = new[] {
                                           0, 2, 1,    // 앞면
                                           4, 1, 2,    // 오른쪽
                                           4, 2, 3,    // 위
                                           0, 4, 3,    // 왼쪽
                                           0, 1, 4,    // 아래
                                           0, 3, 2     // 뒷면
                                       };

              return new Mesh { vertices = vertices, triangles = triangles };
       }
}
