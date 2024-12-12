using System;
using UnityEngine;
using DG.Tweening;

public class Polaroid : MonoBehaviour
{
    public Vector3 GameModePos;
    public Vector3 CameraModePos;

    private void Start()
    {
          GameModeManager.Instance.OnTakeAPicture.AddListener(GetPicture); 
    }

    private void GetPicture()
    {
        Debug.Log("��Ĭ");

    }

    public bool IsObjectInView(Camera camera, GameObject obj)
    {
        // ī�޶��� Frustum ��� ���
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

        // ������Ʈ�� Bounds ��������
        Bounds bounds = obj.GetComponent<Renderer>().bounds;

        // Frustum �� ���� ���� �˻�: ��ġ�ų� �ȿ� ������ True ��ȯ...
        return GeometryUtility.TestPlanesAABB(planes, bounds);
    }
}
