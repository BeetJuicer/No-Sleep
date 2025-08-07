using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;

public class ReplaceInteract : MonoBehaviour
{
    [Tooltip("The new sprite that will replace the existing one on 'Interact' objects.")]
    public Sprite newSprite;

    [ContextMenu("Replace Interact Images")]
    public void ReplaceInteractImages()
    {
        if (newSprite == null)
        {
            Debug.LogError("Error: 'New Sprite' has not been assigned. Aborting.", this);
            return;
        }

        List<Image> interactImages = FindObjectsByType<Image>(FindObjectsSortMode.None)
                                     .Where(img => img.gameObject.name == "Interact")
                                     .ToList();

        if (interactImages.Count == 0)
        {
            Debug.LogWarning("No GameObjects named 'Interact' with an Image component were found.");
            return;
        }

        Debug.Log($"Found {interactImages.Count} 'Interact' objects to process.");
        Undo.SetCurrentGroupName("Replace All Interact Images and Children");
        int undoGroup = Undo.GetCurrentGroup();

        foreach (var imageComponent in interactImages)
        {
            GameObject targetObject = imageComponent.gameObject;

            Undo.RecordObject(imageComponent, "Replace Sprite");
            imageComponent.sprite = newSprite;

            // --- THIS IS THE FIX ---
            // Explicitly mark the component as modified to ensure the change is saved.
            EditorUtility.SetDirty(imageComponent);

            if (targetObject.transform.childCount > 0)
            {
                Transform firstChild = targetObject.transform.GetChild(0);
                Undo.DestroyObjectImmediate(firstChild.gameObject);
            }
        }

        Undo.CollapseUndoOperations(undoGroup);
        Debug.Log("Replacement complete! Remember to SAVE YOUR SCENE (Ctrl+S) to make the changes permanent.");
    }
}
