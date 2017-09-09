using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// This is the class that administrates everything inside the game.
/// This Class contains:
/// - Init and storage of all objects inside the world - at any time
/// - calls updates for every entity (keep in mind, we only have 1 MonoBehaviour in the entire gamelogic)
/// - provides Methodes for saving and loading "Frames"
/// - Manages Time
/// - generic create entity methodes
/// - search for gameobject
/// </summary>
public class WorldController : MonoBehaviour
{
	
	#region Data Storage

    /// <summary>
    /// current time in seconds
    /// </summary>
	public float currTime = 0;

    /// <summary>
    /// all entities saved with their Id's
    /// </summary>
	Dictionary<int,Entity> entities = new Dictionary<int, Entity> ();

    int _frameid = 0;
    /// <summary>
    /// Id of current last frame
    /// </summary>
	int frameid { 
		get { return _frameid; } 
		set {
			_frameid = value;
		}
	}
	//double time= 0;
	//the word 'Break' refers to pause
    /// <summary>
    /// screen that is shown if you pause time
    /// </summary>
	public GameObject BreakScreen;

    /// <summary>
    /// Border around near object
    /// </summary>
	public GameObject Highlight;

    /// <summary>
    /// Sprites of all characters. 
    /// This is temporary and will later be replaced
    /// </summary>
	public Sprite[] CharacterSprites;

	/// <summary>
	/// Here all Enitiy Updates will be added
	/// </summary>
	public event NoNoDel UpdateTick;

    /// <summary>
    /// provides methodes for Frameformatting
    /// </summary>
	FrameManager FManager = new FrameManager ("History.dat");

    /// <summary>
    /// manages all the Instructions for the Entities
    /// </summary>
	InstructionManager IManager;

    /// <summary>
    /// Time between saving frames
    /// </summary>
    public float TimeToSave;

	#endregion

	#region State Properties
    /// <summary>
    /// If time is running
    /// </summary>
	bool TimeRunning = true;
	bool _MenuPause;

    /// <summary>
    /// The menu is shown
    /// </summary>
	bool MenuPause {
		get{ return _MenuPause; }
		set {
			_MenuPause = value;
			if (_MenuPause) {				
				BreakScreen.SetActive (true);
			} else {
				BreakScreen.SetActive (false);
			}
		}
	}

	#endregion

    /// <summary>
    /// this is used for safing FRAMES
    /// </summary>
	float timer = 0;
    //Assets\stuff\Assets\anzug_glatze_sprite.png

    #region monobehaviour Stuff
    
    /// <summary>
    /// setting up the instructionManager, the user, spawns all entities and set the camera to follow player
    /// </summary>
    void Start ()
	{
		currTime = 0;
		#region IntstructionManager
		IManager = new InstructionManager ();
		//IManager.SaveInstructions (new InstructionMovement (2f, 0, new Vector2 (4f, 4f), 3));

		//InstructionData[] idat = new InstructionData[1];
		//idat [0] = new InstructionMovement (2f, 4, new Vector2 (3, 21), 5f);
		//IManager.SaveInstructions (idat);
		IManager.LoadInstructions ();
		#endregion
		
        
        #region user

		List<InputMovement> input = new List<InputMovement> ();
		input.Add (new InputMovement (KeyCode.W, new Vector2 (0f, 1f), PressTypes.KeyDownKey));
		input.Add (new InputMovement (KeyCode.S, new Vector2 (0f, -1f), PressTypes.KeyDownKey));
		input.Add (new InputMovement (KeyCode.D, new Vector2 (1f, 0f), PressTypes.KeyDownKey));
		input.Add (new InputMovement (KeyCode.A, new Vector2 (-1f, 0f), PressTypes.KeyDownKey));

		GameObject player = new GameObject ("Player", typeof(SpriteRenderer), typeof(CircleCollider2D), typeof(Rigidbody2D));

		player.GetComponent<Rigidbody2D> ().isKinematic = true;
		player.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;
		player.GetComponent<Rigidbody2D> ().mass = 1;
		player.GetComponent<Rigidbody2D> ().drag = 1000;
		player.GetComponent<Rigidbody2D> ().angularDrag = 10;
		player.GetComponent<Rigidbody2D> ().gravityScale = 0;

		player.GetComponent<CircleCollider2D> ().radius = 1.5f;
		player.GetComponent<SpriteRenderer> ().sprite = CharacterSprites [0];

		AddNewPlayer (player, input);
		//Instantiate(player);
		Debug.Log ("object created");
		#endregion 

		new Spawner (this);
        		
		//kann benutzt werden um slowmos zu machen und so
		//einfach Zeitlauf verlangsamen
		//Debug.Log (Time.timeScale);
		Camera.main.gameObject.GetComponent<Camera_Behaivour> ().target = entities [0].entity.transform;
	}

    /// <summary>
    /// Called once per frame
    /// checks for "quit" command
    /// checks for button presses
    /// executes all updates on entities
    /// keeps the clock ticking
    /// Exports Frame if needed
    /// </summary>
    void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
        
        // maybe obsolete
		Buttons ();
		//running all updates
		if (TimeRunning && UpdateTick != null) {
			//I call my own Update <3
			if (UpdateTick != null) {
				UpdateTick ();
			}
			
			timer = timer + Time.deltaTime;
			currTime = currTime + Time.deltaTime;
			if (timer >= TimeToSave) {
				FManager.ExportByEntityData (entities);
				timer = 0;
			}
		}
		//Debug.Log ("Log");
	}

    /// <summary>
    /// currently no use
    /// </summary>
	void FixedUpdate ()
	{
		
	}

	#endregion

	#region Methodes

    /// <summary>
    /// Mainly checks for space for pausetoggle
    /// obsolete feature: load frame on [return]
    /// </summary>
	void Buttons ()
	{
		if (MenuPause) {
			if (Input.GetKeyDown (KeyCode.Return)) {
				float ident = BreakScreen.transform.GetComponentInChildren<Slider> ().value;

				//wir wollen ja nichts immer laden
				Frame f = FManager.GetFrameByID ((int)ident);
				LoadFrame (f);
			}
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			//Stop and start time
			TogglePause ();

			//Serialize ();
		}

	}

    /// <summary>
    /// toggles the Time-Pausescreen overlay
    /// </summary>
	void TogglePause ()
	{
		Slider slider = BreakScreen.transform.GetComponentInChildren<Slider> ();
		if (MenuPause) {
			Highlight.SetActive (true);
			FManager.StopPause ();
			MenuPause = false;
			TimeRunning = true;
			FManager.RevertToFrameID ((int)slider.value);
		} else {
			Highlight.SetActive (false);
			FManager.StartPause ();
			slider.maxValue = FManager.GetHighestID ();
			slider.value = slider.maxValue;
			MenuPause = true;
			TimeRunning = false;
		}
		Debug.Log ("TogglePause");
	}

    /// <summary>
    /// Currently only updates the time near slider in Time-Pausescreen
    /// </summary>
	public void UpdateUI ()
	{
		Text step = BreakScreen.transform.GetComponentInChildren<Text> ();
		Slider slide = BreakScreen.transform.GetComponentInChildren<Slider> ();


		step.text = (slide.value * TimeToSave).ToString ();
	}

	/// <summary>
	/// Loads the frame f.
	/// This pastes the information of the Frame
	/// into the coresponding Entities
	/// </summary>
	/// <param name="f">Frame that should be loaded</param>
	//ToDo make it more efficient
	void LoadFrame (Frame f)
	{
		//this reverts the current time
		currTime = f.FrameID * TimeToSave;
		Debug.Log ("Load Frame Initialized");
		foreach (Data data in f.Info) {
			try {
				//Debug.Log("entwered Try at Load Frame");
				Entity c = entities [data.id];
				//Debug.Log("entity snached");
				c.Load (data, f.FrameID * TimeToSave);
				//Debug.Log("Entity succesfully Loaded Data");
			} catch (Exception ex) {
				Debug.LogError (ex.ToString ());
				//Debug.Log ("Stuff happened");
			}
		}
		Debug.Log ("Loaded Frame " + f.FrameID);
	}

	/// <summary>
	/// Adds the new entity.
	/// </summary>
	/// <param name="go">GameObject</param>
	public void AddNewEntity (GameObject go)
	{        
		Entity c = new Entity (go, this, entities.Count);
		entities.Add (entities.Count, c);
	}

	/// <summary>
	/// Adds the new character.
	/// </summary>
	/// <param name="go">GameObject</param>
	public void AddNewCharacter (GameObject go)
	{        
		Entity c = new Character (go, this, entities.Count, IManager);

		entities.Add (entities.Count, c);
	}

	/// <summary>
	/// Adds the new player.
	/// </summary>
	/// <param name="go">GameObject</param>
	/// <param name="move">Movement Inputs</param>
	public void AddNewPlayer (GameObject go, List<InputMovement> move)
	{
		Entity c = new Player (go, this, entities.Count, move);
		entities.Add (entities.Count, c);
	}

	/// <summary>
	/// Raises the slide change event.
	/// </summary>
	public void OnSlideChange ()
	{
		float ident = BreakScreen.transform.GetComponentInChildren<Slider> ().value;
		if (MenuPause) {
			Frame f = FManager.GetFrameByID ((int)ident);
			LoadFrame (f);
		}
	}

	/// <summary>
	/// Gets the entity by game object.
	/// </summary>
	/// <returns>The entity by game object.</returns>
	/// <param name="Go">GameObject</param>
	public Entity GetEntityByGameObject (GameObject Go)
	{
		foreach (KeyValuePair<int,Entity> item in entities) {
			if (item.Value.entity == Go) {
				return item.Value;
			}
		}
		return null;
	}

	#endregion
}