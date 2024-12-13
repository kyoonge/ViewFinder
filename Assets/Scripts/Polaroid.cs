using UnityEngine;

public class Polaroid : MonoBehaviour
{
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private GameObject film;
    [SerializeField] private Camera mainCamera;

    public void ActivateCamera(bool active)
    {
        if (cameraObject != null)
        {
            cameraObject.SetActive(active);
        }
    }

    public void ShowFilm()
    {
        if (film != null)
        {
            film.SetActive(true);
        }
    }

    public void HideFilm()
    {
        if (film != null)
        {
            film.SetActive(false);
        }
    }

    public void TakePicture()
    {
        // 사진 찍기 로직
        //activeFilm = Instantiate(filmPrefab); // 필름 생성
        //activeFilm.SetActive(false); // 일단 숨겨둠
    }

    public void PlaceFilm(Vector3 position)
    {
        Debug.Log("필름 배치");
        //if (activeFilm != null)
        //{
        //    activeFilm.transform.position = position;
        //    activeFilm.SetActive(true);
        //}
    }

    public bool IsTargetInView(GameObject target)
    {
        if (mainCamera == null || target == null) return false;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        Renderer renderer = target.GetComponent<Renderer>();
        if (renderer == null) return false;

        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }
}