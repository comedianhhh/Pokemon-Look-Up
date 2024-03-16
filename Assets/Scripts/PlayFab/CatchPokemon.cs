using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;



public class CatchPokemon : MonoBehaviour
{

    [System.Serializable]
    public class PokemonData
    {
        public string name;
        public Sprites sprites;
    }

    [System.Serializable]
    public class Sprites
    {
        public string front_default;
    }
    public TMPro.TMP_InputField pokedexInputField;
    public TMPro.TMP_Text pokemonNameDisplay;
    public TMPro.TMP_Text PlayerDisplay;
    public TMPro.TMP_InputField PlayerName;
    public TMPro.TMP_Text PlayerProfile;
    public Image pokemonImageDisplay;


    public string PlayerGUID = "";
    public bool CreateNewPlayer = false;

    public void FetchAndDisplayPokemon()
    {
        int pokedexIndex;
        if (int.TryParse(pokedexInputField.text, out pokedexIndex))
        {
            StartCoroutine(DownloadAndDisplayPokemonSprite(pokedexIndex));
        }
        else
        {
            Debug.LogError("Invalid Pokedex index entered.");
        }
    }

    private IEnumerator DownloadAndDisplayPokemonSprite(int index)
    {
        string url = $"https://pokeapi.co/api/v2/pokemon/{index}/";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string json = webRequest.downloadHandler.text;
                var (spriteUrl, pokemonName) = ExtractSpriteUrlFromJson(json);
                pokemonNameDisplay.text=pokemonName;
                yield return StartCoroutine(DownloadSpriteImage(spriteUrl));
            }
            else
            {
                Debug.LogError($"Error fetching Pokémon data: {webRequest.error}");
            }
        }
    }

    private IEnumerator DownloadSpriteImage(string imageUrl)
    {
        UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return textureRequest.SendWebRequest();

        if (textureRequest.result == UnityWebRequest.Result.Success)
        {
            // Fetch the downloaded byte array
            byte[] imageBytes = textureRequest.downloadHandler.data;

            // Create a new Texture2D (width and height will be set by LoadImage)
            Texture2D texture = new Texture2D(2, 2); // The size here is arbitrary as LoadImage will replace it with the correct size

            // Load the image data into the texture
            if (ImageConversion.LoadImage(texture, imageBytes))
            {
                // Create a sprite from the loaded texture
                Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                pokemonImageDisplay.sprite = sprite;
            }
            else
            {
                Debug.LogError("Failed to load texture from downloaded image bytes.");
            }
        }
        else
        {
            Debug.LogError($"Error downloading sprite: {textureRequest.error}");
        }
    }

    private (string spriteUrl, string name) ExtractSpriteUrlFromJson(string json)
    {
        PokemonData data = JsonUtility.FromJson<PokemonData>(json);
        return (data.sprites.front_default, data.name);
    }
    public void SetPlayerName()
    {
        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = PlayerName.text };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, result =>
        {
            Debug.Log("Display name updated successfully.");
        }, error =>
        {
            Debug.LogError("Failed to update display name: " + error.GenerateErrorReport());
        });
    }

    public void GetPlayerName()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result =>
        {
            var displayName = result.AccountInfo.TitleInfo.DisplayName;
            PlayerDisplay.text = displayName;
            Debug.Log(displayName);
        }, error =>
        {
            Debug.LogError("Failed to get player name: " + error.GenerateErrorReport());
        });
    }

    public void AddCaughtPokemon(int amount = 1)
    {
        PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = "PM",
            Amount = amount
        }, result =>
        {
            Debug.Log("Successfully added caught Pokémon currency.");
        }, error =>
        {
            Debug.LogError("Failed to add caught Pokémon currency: " + error.GenerateErrorReport());
        });
    }
    public void GetCaughtPokemonCount()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
        {
            if (result.VirtualCurrency.ContainsKey("PM"))
            {
                Debug.Log($"Player has caught {result.VirtualCurrency["PM"]} Pokémon.");
                // Update your UI here
            }
            else
            {
                Debug.Log("Player has not caught any Pokémon.");
            }
        }, error =>
        {
            Debug.LogError("Failed to get caught Pokémon count: " + error.GenerateErrorReport());
        });
    }

}

