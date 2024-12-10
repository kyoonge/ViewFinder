using System;
using UnityEngine;

public class Polaroid : MonoBehaviour
{
    private void Start()
    {
          GameModeManager.Instance.OnTakeAPicture.AddListener(GetPicture); 
    }

    private void GetPicture()
    {
        Debug.Log("찰칵");

    }

    public bool IsObjectInView(Camera camera, GameObject obj)
    {
        // 카메라의 Frustum 평면 계산
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

        // 오브젝트의 Bounds 가져오기
        Bounds bounds = obj.GetComponent<Renderer>().bounds;

        // Frustum 내 포함 여부 검사: 겹치거나 안에 있으면 True 반환...
        return GeometryUtility.TestPlanesAABB(planes, bounds);
    }
}
