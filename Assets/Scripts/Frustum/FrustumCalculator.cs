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

       // ����ü�� ������ ��ǥ ���
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


       public static FrustumPlanes CreateFrustumMeshes(FrustumPoints points, float offset,
        out Mesh leftMesh, out Mesh rightMesh, out Mesh topMesh, out Mesh bottomMesh)
       {
              FrustumPlanes planes = CreateFrustumPlanes(points);

              Vector3 leftOffset = planes.left.normal * offset;
              Vector3 rightOffset = planes.right.normal * offset;
              Vector3 topOffset = planes.top.normal * offset;
              Vector3 bottomOffset = planes.bottom.normal * offset;

              leftMesh = CreateBoxMesh(points.cameraPosition, points.leftUp, (points.leftUp + points.leftDown) / 2, points.leftDown, leftOffset);
              rightMesh = CreateBoxMesh(points.cameraPosition, points.rightDown, (points.rightUp + points.rightDown) / 2, points.rightUp, rightOffset);
              topMesh = CreateBoxMesh(points.cameraPosition, points.rightUp, (points.leftUp + points.rightUp) / 2, points.leftUp, topOffset);
              bottomMesh = CreateBoxMesh(points.cameraPosition, points.leftDown, (points.leftDown + points.rightDown) / 2, points.rightDown, bottomOffset);

              return planes;
       }

       // ����ü�� �����ϴ� 4���� ��� ���� (�� ���� ��ǥ ������ ���)
       public static FrustumPlanes CreateFrustumPlanes(FrustumPoints points)
       {
              return new FrustumPlanes
              {
                     left = new Plane(points.cameraPosition, points.leftUp, points.leftDown),
                     right = new Plane(points.cameraPosition, points.rightDown, points.rightUp),
                     top = new Plane(points.cameraPosition, points.rightUp, points.leftUp),
                     bottom = new Plane(points.cameraPosition, points.leftDown, points.rightDown)
              };
       }

       static Mesh CreateBoxMesh(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector3 offset)
       {
              Vector3[] vertices = new[] { v1, v2, v3, v4, v1 + offset, v2 + offset, v3 + offset, v4 + offset };
              int[] triangles = new[] 
              {   //vertices �迭�� �ε���
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
              // �������� ���θ� ���ϴ� ������ ����
              Vector3 cameraPos = points.cameraPosition + (forwardVector * -offset);
              Vector3 rightDownOffset = points.rightDown + (planes.right.normal * -offset);
              Vector3 rightUpOffset = points.rightUp + (planes.right.normal * -offset);
              Vector3 leftUpOffset = points.leftUp + (planes.left.normal * -offset);
              Vector3 leftDownOffset = points.leftDown + (planes.left.normal * -offset);

              Vector3[] vertices = new[] { cameraPos, rightDownOffset, rightUpOffset, leftUpOffset, leftDownOffset };
              int[] triangles = new[] {
                                           0, 2, 1,    // �ո�
                                           4, 1, 2,    // ������
                                           4, 2, 3,    // ��
                                           0, 4, 3,    // ����
                                           0, 1, 4,    // �Ʒ�
                                           0, 3, 2     // �޸�
                                       };



              return new Mesh { vertices = vertices, triangles = triangles };
       }

       public static void DrawFrustum(FrustumPoints points, Vector3 forwardVector, float offset, FrustumPlanes planes)
       {
              // ������ ����
              Vector3 cameraPos = points.cameraPosition + (forwardVector * -offset);
              Vector3 rightDownOffset = points.rightDown + (planes.right.normal * -offset);
              Vector3 rightUpOffset = points.rightUp + (planes.right.normal * -offset);
              Vector3 leftUpOffset = points.leftUp + (planes.left.normal * -offset);
              Vector3 leftDownOffset = points.leftDown + (planes.left.normal * -offset);

              // �� ���� ����
              Color lineColor = Color.green;

              // ����ü�� �𼭸��� ������ ����
              Debug.DrawLine(cameraPos, rightDownOffset, lineColor, 10f);  // ī�޶� -> ������ �Ʒ�
              Debug.DrawLine(cameraPos, rightUpOffset, lineColor, 10f);   // ī�޶� -> ������ ��
              Debug.DrawLine(cameraPos, leftUpOffset, lineColor, 10f);    // ī�޶� -> ���� ��
              Debug.DrawLine(cameraPos, leftDownOffset, lineColor, 10f);  // ī�޶� -> ���� �Ʒ�

              Debug.DrawLine(rightDownOffset, rightUpOffset, lineColor, 10f);  // ������ �Ʒ� -> ������ ��
              Debug.DrawLine(rightUpOffset, leftUpOffset, lineColor, 10f);     // ������ �� -> ���� ��
              Debug.DrawLine(leftUpOffset, leftDownOffset, lineColor, 10f);    // ���� �� -> ���� �Ʒ�
              Debug.DrawLine(leftDownOffset, rightDownOffset, lineColor, 10f); // ���� �Ʒ� -> ������ �Ʒ�
       }
}
