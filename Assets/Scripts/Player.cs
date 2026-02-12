using UnityEngine;

public class Player : MonoBehaviour
{
    private IHoverable currentHovered;
    private Collider currentCollider;

void Update()
{
    if(GameManager.Instance.CurrentState != GameState.Exploring) return;

        HandleRayCast();
        HandleClick();
    }

    private void HandleRayCast()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f))
        {
            currentCollider = hit.collider;
            IHoverable hoverable = hit.collider.GetComponent<IHoverable>(); 

            if (hoverable != currentHovered)
            {
                if (currentHovered != null)
                    currentHovered.OnHoverExit();

                currentHovered = hoverable;

                if (currentHovered != null)
                    currentHovered.OnHoverEnter();
            }
        }
        else
        {
            if (currentHovered != null)
                currentHovered.OnHoverExit();

            currentHovered = null;
        }  
    }

    private void HandleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(currentCollider == null) return;
            
            IClickable clickable = currentCollider.GetComponent<IClickable>();
            clickable?.OnClicked(); //Appelle OnClicked() uniquement si clickable n’est pas null
        }
    }
}
