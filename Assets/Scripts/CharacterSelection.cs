using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
	[SerializeField] CameraMovement camera;

	// 2-D array structure that stores all the outfits
	private GameObject[][] outfits = new GameObject[2][];
	private int selectedSection = 0;

	// Character model
	[SerializeField] private GameObject[] characters;
	[SerializeField] private GameObject[] deco;
	private int selectedItem = 0;

	private int[] PlayerData = new int[2];

	// Random outfits will be generated after startup
	void Start()
	{
		for (int i = 0; i < PlayerData.Length; i++)
		{
			PlayerData[i] = 0;
		}

		outfits[0] = characters;
		outfits[1] = deco;
		Randomize();
	}

	float speed = 1f;
	float delta = 0.33f;  //delta is the difference between min y to max y.

	// Fancy Stuff
	void Update() 
	{
		float y = Mathf.Sin(speed * Time.time) * delta;
		Vector3 pos = new Vector3(transform.position.x, y, transform.position.z);
		transform.position = pos;
	}

	/*
	 * The following two functions changes the item in a specific outfit category
	 */
	public void NextItem()
	{
		selectedItem = PlayerData[selectedSection];
		outfits[selectedSection][selectedItem].SetActive(false);
		selectedItem++;
		if (selectedItem >= outfits[selectedSection].Length)
		{
			selectedItem = 0;
		}
		outfits[selectedSection][selectedItem].SetActive(true);

		// Remeber the player's choice in section
		PlayerData[selectedSection] = selectedItem;
	}

	public void PreviousItem()
	{
		selectedItem = PlayerData[selectedSection];
		outfits[selectedSection][selectedItem].SetActive(false);
		selectedItem--;
		if (selectedItem < 0)
		{
			selectedItem += outfits[selectedSection].Length;
		}
		outfits[selectedSection][selectedItem].SetActive(true);

		// Remeber the player's choice in section
		PlayerData[selectedSection] = selectedItem;
	}

	/*
	 * The following two functions changes the outfit category
	 */
	public void NextSection()
	{
		selectedSection = selectedSection + 1;
		if (selectedSection >= outfits.Length) 
		{
			selectedSection = 0;
		}

		camera.SetCurView(selectedSection);
	}

	public void PreviousSection()
	{
		selectedSection--;
		if (selectedSection < 0)
		{
			selectedSection += outfits.Length;
		}

		camera.SetCurView(selectedSection);
	}

	public void StartGame()
	{
		PlayerPrefs.SetInt("selectedCharacter", PlayerData[0]);
		PlayerPrefs.SetInt("selectedDeco", PlayerData[1]);
		SceneManager.LoadScene(1, LoadSceneMode.Single);
	}

	public void BackToTitle()
	{
		#if UNITY_EDITOR
			// Application.Quit() does not work in the editor so
			// UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}

	public void Randomize()
	{
		outfits[0][PlayerData[0]].SetActive(false);
		selectedItem = Random.Range(0, characters.Length);
		outfits[0][selectedItem].SetActive(true);
		PlayerData[0] = selectedItem;
		//Debug.Log("Data: " + selectedItem);

		outfits[1][PlayerData[1]].SetActive(false);
		selectedItem = Random.Range(0, deco.Length);
		outfits[1][selectedItem].SetActive(true);
		PlayerData[1] = selectedItem;
		//Debug.Log("Data: " + selectedItem);
	}
}
