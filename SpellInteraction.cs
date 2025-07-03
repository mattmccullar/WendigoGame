using UnityEngine;

public class SpellInteraction : MonoBehaviour
{
    SpellCaster spellCaster;
    [SerializeField]
    private GameObject hoveredObject;
    private void Start()
    {
        spellCaster = FindObjectOfType<SpellCaster>();
    }

    private void OnMouseEnter()
    {
        hoveredObject = gameObject;
        Debug.Log("mouse entered spell object: " + hoveredObject.name);
    }

    private void OnMouseExit()
    {
        Debug.Log("mouse exited spell object: " + hoveredObject.name);
        hoveredObject = null; // clear hovered object reference
    }
}
