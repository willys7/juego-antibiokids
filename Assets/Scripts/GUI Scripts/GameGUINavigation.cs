using System;
using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class GameGUINavigation : MonoBehaviour {

	//------------------------------------------------------------------
	// Variable declarations
	
	private bool _paused;
    private bool quit;
    private bool correct_answer = true;
    private string _errorMsg;
    private int preguntanum = 0;
    private string[] preguntas = new string[] {
        "Los antibioticos se utilizan para matar los virus.","Los antibioticos fueron descubiertos por primera vez por Alexander Fleming","La penicilina se obtiene de un hongo",
        "Las bacterias resistentes a los antibioticos son un problema en los hospitales","Los antibioticos se utilizan para matar las bacterias","Nunca debes usar los antibioticos de otras personas.",
        "Un tercio de los antibioticos existentes ya no funcionan contra las bacterias.","Los antibioticos se utilizaron para matar muchos soldados en la Segunda Guerra Mundial",
        "Se deben tomar antibioticos para la gripe.","Los antibioticos pueden curar cualquier enfermedad","La penicilina es el unico antibiotico que podemos usar","Solo hay que tomar antibioticos hasta sentirse mejor",
        "Los antibioticos no matan bacterias utiles.","Las personas llegan a una edad mas adulta hoy en dia que hace 50 años porque los antibioticos ayudan a controlar muchas enfermedades",
        "Al lavarse las manos se puede utilizar agua unicamente. No es necesario utilizar jabon.","Despues de estar con alguien enfermo no es necesario lavarse las manos.",
        "Al estornudar nos debemos tapar la boca.","Tenemos que lavarnos las manos despues de estornudar.","Los virus son parasitos","Todos los microbios son malos",
        "Es considerada infeccion nosocomial si aparece 48 horas despues de entrar en las instalaciones del hospital.","Las infecciones nosocomiales se pueden transmitir por un buen procedimiento quirurgico.",
        "Las infecciones nosocomiales estan relacionadas con la atencion sanitaria.","Una forma de prevenir las infecciones nosocomiales es lavandose muy bien las manos y cumpliendo con todas las normas de limpieza.",
        "Todas las personas estan propensas a contraer infecciones nosocomiales."};
    private bool[] respuestas = new bool[] { false, true, true, true, true, true, true, false, false, false, false, false, false, true, false, false, true, true, true, false, true, false, true, true, true };
    private string[] aprendizaje = new string[] {
        "Los antibioticos no se pueden utilizar para tratar enfermedades provocadas por virus.","Fleming descubrio las propiedades antibioticos de los hongos cuando un grupo de ellos empezo a crecer por accidente en su placa de agar de bacterias. Las bacterias no crecian cerca de los hongos.",
        "La penicilina se obtiene del hongo Penicillium","Los antibioticos se utilizan mucho en los hospitales, asi que las bacterias han desarrollado resistencia a ellos. El SARM es un ejemplo de bacteria resistente a los antibioticos.",
        "Los antibioticos destruyen las bacterias de varias formas, aunque no todos ellos los matan: algunas bacterias son resistentes a ciertos antibioticos.","Se receta un antibiotico especifico a cada persona por diferentes razones. Tomar los antibioticos de otras personas es peligroso y puede que no sea eficaz.",
        "Las bacterias suelen multiplicarse rapidamente y se vuelven resistentes a antibioticos.","Esto significa que se necesita crear nuevos antibioticos y utilizar adecuadamente los que tenemos.",
        "Durante la Segunda Guerra Mundial murieron varios solados pero debido a las infecciones bacterianas que se introducian en sus heridas. Se utilizaron antibioticos para salvar las vidas de los soldados.",
        "No se deben tomar antibioticos para la gripe. La gripe es un virus y los antibioticos no funcionan contra ellos. La gripe suele mejorar con tomar suficientes liquidos y descansar.",
        "Los antibioticos funcionan unicamente contra bacterias. Las enfermedades infecciosas pueden ser causadas por bacterias, virus y hongos.","La penicilina es conocida por haber sido el primer antibiotico descubierto y de uso comun.",
        "Los antibioticos se tomar hasta el final del tratamiento que indique el doctor pues no finalizarlo provocaria una recaida. Al no terminar el tratamiento puede provocar resistencia en las bacterias.",
        "Algunos antibioticos matan todas las bacterias, ya sean buenas o malas.","Antes de que se descubrieran los antibioticos, muchas personas morian a causa de enfermedades que no tenian cura.",
        "El agua no elimina los microbios en nuestras manos. El jabon pueden ayudar a eliminar los microbios. Se debe utilizar siempre jabon al lavarse las manos.","Al estar con personas enfermas se pueden pegar los microbios. Si nos lavamos las manos despues de estar con personas enfermas se eliminaran microbios.",
        "Cuando estornudemos debemos taparnos la boca con el antebrazo para evitar contaminar bacterias.","Al estornudar sobre las manos se contaminan de virus de la gripe y se puede transmitir a las personas que tocamos. Lavarse las manos con agua y jabon se eliminan estos microbios.",
        "Los virus no pueden sobrevivir fuera de una celula huesped.","Existen mas microbios buenos que malos. Los microbios nos ayudan en el dia con la produccion de alimentos y crecimiento de plantas.",
        "La OMS (Organizacion Mundial de la Salud) dice que las Infecciones Nosocomiales son una infeccion contraida en un hospital despues de 48 horas de estar alli. Se puede extender hasta 30 dias o 1 año.",
        "Las infecciones nosocomiales se transmiten al no realizar un procedimiento quirurgico limpio, al no lavarse bien las manos antes de una operacion.","La higiene hospitalaria es por medio de enfermeria, personal de limpieza, doctores, todos. Lavarse las manos de forma apropiada es la clave.",
        "Al lavarse las manos se recomienda cantar la cancion “Happy birthday”. Se deben fregar las manos, muñecas, entre los dedos y debajo de las uñas con agua y jabon.",
        "Prevenir las infecciones nosocomiales es responsabilidad de los doctores, enfermeras, pacientes y visitas." };
    //public bool initialWaitOver = false;

    public float initialDelay;

    // canvas
    public Canvas PauseCanvas;
    public Canvas PreguntaCanvas;
    public Canvas QuitCanvas;
    public Canvas ReadyCanvas;
    public Canvas RespuestaCanvas;
    public Canvas ScoreCanvas;
    public Canvas ErrorCanvas;
    public Canvas GameOverCanvas;

    // buttons
    public Button MenuButton;
    public Button TrueButton;
    public Button FalseButton;
    public Button OkButton;

    //Texts
    public Text QuestionText;
    public Text AprendizajeText;

    //------------------------------------------------------------------
    // Function Definitions

    // Use this for initialization
    void Start () 
	{
		StartCoroutine("ShowReadyScreen", initialDelay);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			// if scores are show, go back to main menu
			if(GameManager.gameState == GameManager.GameState.Scores)
				Menu();

			// if in game, toggle pause or quit dialogue
			else
			{
				if(quit == true)
					ToggleQuit();
				else
					TogglePause();
			}
		}
	}

	// public handle to show ready screen coroutine call
	public void H_ShowReadyScreen()
	{
		StartCoroutine("ShowReadyScreen", initialDelay);
	}

    public void H_ShowGameOverScreen()
    {
        StartCoroutine("ShowGameOverScreen");
    }

	IEnumerator ShowReadyScreen(float seconds)
	{
		//initialWaitOver = false;
		GameManager.gameState = GameManager.GameState.Init;
		ReadyCanvas.enabled = true;
		yield return new WaitForSeconds(seconds);
		ReadyCanvas.enabled = false;
		GameManager.gameState = GameManager.GameState.Game;
		//initialWaitOver = true;
	}

    IEnumerator ShowGameOverScreen()
    {
        Debug.Log("Showing GAME OVER Screen");
        GameOverCanvas.enabled = true;
        yield return new WaitForSeconds(2);
        Menu();
    }

	public void getScoresMenu()
	{
		Time.timeScale = 0f;		// stop the animations
		GameManager.gameState = GameManager.GameState.Scores;
		MenuButton.enabled = false;
		ScoreCanvas.enabled = true;
	}

	//------------------------------------------------------------------
	// Button functions

	public void TogglePause()
	{
		// if paused before key stroke, unpause the game
		if(_paused)
		{
			Time.timeScale = 1;
			PauseCanvas.enabled = false;
			_paused = false;
			MenuButton.enabled = true;
		}
		
		// if not paused before key stroke, pause the game
		else
		{
			PauseCanvas.enabled = true;
			Time.timeScale = 0.0f;
			_paused = true;
			MenuButton.enabled = false;
		}


        Debug.Log("PauseCanvas enabled: " + PauseCanvas.enabled);
	}
	
	public void ToggleQuit()
	{
		if(quit)
        {
            PauseCanvas.enabled = true;
            QuitCanvas.enabled = false;
			quit = false;
		}
		
		else
        {
            QuitCanvas.enabled = true;
			PauseCanvas.enabled = false;
			quit = true;
		}
	}

    public void ToggleQuestion()
    {
        QuestionText.fontSize = 20;
        PreguntaCanvas.enabled = true;
        Time.timeScale = 0.0f;
        System.Random rnd = new System.Random();
        preguntanum = rnd.Next(0, 25);
        QuestionText.text = preguntas[preguntanum];//GameManager.lives = GameManager.lives - 1;
    }
    public void ToggleTrue()
    {
        bool is_right = respuestas[preguntanum];
        if (is_right)
        {
            QuestionText.text = "CORRECTO!!!";
        }
        else
        {
            QuestionText.text = "INCORRECTO!!!";
        }
        QuestionText.fontSize = 30;
        PreguntaCanvas.enabled = false;
        RespuestaCanvas.enabled = true;
        correct_answer = correct_answer && is_right;
        AprendizajeText.text = aprendizaje[preguntanum];
    }

    public void ToggleFalse()
    {
        bool is_right = respuestas[preguntanum];
        if (is_right)
        {
            QuestionText.text = "CORRECTO!!!";
        }
        else
        {
            QuestionText.text = "INCORRECTO!!!";
        }
        QuestionText.fontSize = 30;
        StartCoroutine("ShowAnswerScreen", initialDelay);
        PreguntaCanvas.enabled = false;
        AprendizajeText.fontSize = 20;
        AprendizajeText.text = aprendizaje[preguntanum];
        RespuestaCanvas.enabled = true;
        correct_answer = correct_answer && is_right;
    }

    public void LearningAnswer()
    {
        UIScript ui = GameObject.FindObjectOfType<UIScript>();
        if (correct_answer)
        {
            Time.timeScale = 1;
            PauseCanvas.enabled = false;
            RespuestaCanvas.enabled = false;
            PauseCanvas.enabled = false;
            _paused = false;
            MenuButton.enabled = true;
            GameManager.gameState = GameManager.GameState.Init;
            H_ShowReadyScreen();
            //GameManager.gameState = GameManager.GameState.Game;
        }
        else
        {
            GameManager.lives--;
            GameManager.gameState = GameManager.GameState.Dead;

            // update UI too
            Destroy(ui.lives[ui.lives.Count - 1]);
            ui.lives.RemoveAt(ui.lives.Count - 1);
            RespuestaCanvas.enabled = false;
            Time.timeScale = 1;
            MenuButton.enabled = true;
            if (GameManager.lives > 0)
            {
                GameManager.gameState = GameManager.GameState.Init;
                H_ShowReadyScreen();
            }
            else
            {
                H_ShowGameOverScreen();
            }
        }
    }

    public void Menu()
	{
		Application.LoadLevel("menu");
		Time.timeScale = 1.0f;

        // take care of game manager
	    GameManager.DestroySelf();
	}

    IEnumerator AddScore(string name, int score)
    {
        string privateKey = "pKey";
        string AddScoreURL = "http://ilbeyli.byethost18.com/addscore.php?";
        string hash = Md5Sum(name + score + privateKey);

        Debug.Log("Name: " + name + " Escape: " + WWW.EscapeURL(name));

        WWW ScorePost = new WWW(AddScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash );
        yield return ScorePost;

        if (ScorePost.error == null)
        {
            Debug.Log("SCORE POSTED!");

            // take care of game manager
            Destroy(GameObject.Find("Game Manager"));
            GameManager.score = 0;
            GameManager.Level = 0;

            Application.LoadLevel("scores");
            Time.timeScale = 1.0f;
        }
        else
        {
            Debug.Log("Error posting results: " + ScorePost.error);
        }

        yield return new WaitForSeconds(2);
    }

    public string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }

	public void SubmitScores()
	{
		// Check username, post to database if its good to go
	    int highscore = GameManager.score;
        string username = ScoreCanvas.GetComponentInChildren<InputField>().GetComponentsInChildren<Text>()[1].text;
        Regex regex = new Regex("^[a-zA-Z0-9]*$");

	    if (username == "")                 ToggleErrorMsg("Username cannot be empty");
        else if (!regex.IsMatch(username))  ToggleErrorMsg("Username can only consist alpha-numberic characters");
        else if (username.Length > 10)      ToggleErrorMsg("Username cannot be longer than 10 characters");
        else                                StartCoroutine(AddScore(username, highscore));
	    
	}

    public void LoadLevel()
    {
        GameManager.Level++;
        Application.LoadLevel("game");
    }

    public void ToggleErrorMsg(string errorMsg)
    {
        if (ErrorCanvas.enabled)
        {
            ScoreCanvas.enabled = true;
            ErrorCanvas.enabled = false;

        }
        else
        {
            ScoreCanvas.enabled = false;
            ErrorCanvas.enabled = true;
            ErrorCanvas.GetComponentsInChildren<Text>()[1].text = errorMsg;

        }
    }
}
