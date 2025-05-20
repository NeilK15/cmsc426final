using UnityEngine;

public class Menu : MonoBehaviour
{

    [SerializeField]
    private LayerMask buttonLayer;
    private GameObject lastHovered;

    void Update()
    {
        // Create a ray from the mouse position into the world
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, buttonLayer))
        {
            // Check if we hit something
            GameObject hovered = hit.collider.gameObject;
            FruitButton fruitButton = hovered.gameObject.GetComponent<FruitButton>();
            fruitButton.SetHover(true);

            if (Input.GetMouseButtonDown(0))
            {
                fruitButton.Interact();
            }

            lastHovered = hovered;
        }
        else
        {
            if (lastHovered != null)
                lastHovered.GetComponent<FruitButton>().SetHover(false);
        }
    }
}
