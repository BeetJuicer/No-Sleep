using System.Collections.Generic;
using Esper.ESave;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    private const string objectPositionDataKey = "ObjectPosition";
    private const string objectColorDataKey = "ObjectColor";

    [SerializeField]
    private SpriteRenderer[] sprites;

    private SaveFileSetup saveFileSetup;
    private SaveFile saveFile;
    private bool isLoaded;

    private void Start()
    {
        // Get save file
        saveFileSetup = GetComponent<SaveFileSetup>();
        saveFile = saveFileSetup.GetSaveFile();

        // Load game when the save file operation is completed
        if (saveFile.isOperationOngoing)
        {
            saveFile.operation.onOperationEnded.AddListener(LoadGame);
        }
        else
        {
            LoadGame();
        }
    }

    /// <summary>
    /// Loads the game.
    /// </summary>
    public void LoadGame()
    {
        //for (int i = 0; i < sprites.Length; i++)
        //{
        //    string positionKey = objectPositionDataKey + i;
        //    string colorKey = objectColorDataKey + i;

        //    if (saveFile.HasData(positionKey))
        //    {
        //        // Get Vector3 from a special method because Vector3 is not savable data
        //        var savableTransform = saveFile.GetTransform(positionKey);
        //        sprites[i].transform.CopyTransformValues(savableTransform);
        //    }

        //    if (saveFile.HasData(colorKey))
        //    {
        //        sprites[i].color = saveFile.GetColor(colorKey);
        //    }
        //}

        //// Populate lists with current state
        //for (int i = 0; i < sprites.Length; i++)
        //{
        //    var sprite = sprites[i];
        //    randomPositions.Add(sprite.transform.position);
        //    randomRotations.Add(sprite.transform.rotation);
        //    randomScales.Add(sprite.transform.localScale);
        //    randomColors.Add(sprite.color);
        //}

        isLoaded = true;

        Debug.Log("Loaded game.");
    }

    /// <summary>
    /// Saves the game.
    /// </summary>
    public void SaveGame()
    {
        //for (int i = 0; i < sprites.Length; i++)
        //{
        //    saveFile.AddOrUpdateData(objectPositionDataKey + i, sprites[i].transform);
        //    saveFile.AddOrUpdateData(objectColorDataKey + i, sprites[i].color);
        //}

        saveFile.Save();

        Debug.Log("Saved game.");
    }
}
