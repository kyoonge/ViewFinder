using UnityEngine;

public class Polaroid : MonoBehaviour
{
       [SerializeField] GameObject cameraObject;
       [SerializeField] GameObject film;
       [SerializeField] Camera mainCamera;
       CameraFrustum frustum;
       Camera cam;

       void Start()
       {
              TryGetComponent(out frustum);
              cam = GetComponentInChildren<Camera>(true);
       }

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
              frustum.Cut(cam, true);
              //activeFilm = Instantiate(filmPrefab); // 필름 생성
              //activeFilm.SetActive(false); // 일단 숨겨둠
       }

       public void PlaceFilm()
       {
              Debug.Log("필름 배치");
              frustum.Cut(cam, false);
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