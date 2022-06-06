using UnityEngine;

public class ScaleBackground : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		Vector3 tempScale = transform.localScale;

		float height = sr.bounds.size.y;
		float width = sr.bounds.size.x;

		float worldHeight = Camera.main.orthographicSize * 2f;//10
		float worldWidth = worldHeight * Screen.width / Screen.height;//10 * 203/339

		tempScale.y = worldHeight / height;
		tempScale.x = worldWidth / width;

		transform.localScale = tempScale;
	}

}
