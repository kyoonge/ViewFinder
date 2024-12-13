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
        // ���� ��� ����
        //activeFilm = Instantiate(filmPrefab); // �ʸ� ����
        //activeFilm.SetActive(false); // �ϴ� ���ܵ�
    }

    public void PlaceFilm(Vector3 position)
    {
        Debug.Log("�ʸ� ��ġ");
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