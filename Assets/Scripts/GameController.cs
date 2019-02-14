using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject levelCompletePanel;   
    public GameObject theEndPanel;
    public GameObject caldron;

    bool gamePaused = false;

    public AudioSource audioController;

    public AudioClip wordCorrectSound;
    public AudioClip wordWrongSound;

    public AudioClip level1Music;
    public AudioClip level2Music;
    public AudioClip level3Music;

    public List<AudioClip> musicLevels;

    List<Word> wordsListEasy = new List<Word>();
    List<Word> wordsListNormal = new List<Word>();
    List<Word> wordsListHard = new List<Word>();
    Word wordSelected;

    public Text currentWordText;
    public Text currentTipText;
    public Text answer;

    public Object ingredient;

    int numberOfLives = 4;
    int wordsCorrect = 0;

    public static int level = 1;

    void Start () {
        //Debug.Log("Level: " + level);
        setWordsList();
        wordSelected = ChooseRandomWord();
        currentTipText.text = wordSelected.tip.ToUpper();
        HideWord();
        audioController.loop = true;
        AddMusicsToList();
        PlayMusic(musicLevels[level - 1]);
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !gamePaused;
            pausePanel.SetActive(gamePaused);
            turnObjectVisibility(!gamePaused);
        }
    }

    /**
     * Retorna ao jogo
     */
    public void ChangePause()
    {
        gamePaused = !gamePaused;
        pausePanel.SetActive(gamePaused);
        turnObjectVisibility(!gamePaused);
    }

    /**
     * Adiciona a lista de palavras nos três niveis de dificuldade.
     */
    void setWordsList()
    {
        wordsListEasy.Add(new Word("fireplace", "warm"));
        wordsListEasy.Add(new Word("monkey", "animal"));
        //wordsListEasy.Add(new Word("philadelphia", "city"));
        wordsListEasy.Add(new Word("august", "month"));
        wordsListEasy.Add(new Word("egypt", "country"));
        wordsListEasy.Add(new Word("vodka", "drink"));
        wordsListEasy.Add(new Word("zombie", "dead"));
        wordsListEasy.Add(new Word("subway", "underground"));
        wordsListEasy.Add(new Word("shake", "vibrate"));
        wordsListEasy.Add(new Word("fruit", "food"));

        wordsListNormal.Add(new Word("strength", "power"));
        wordsListNormal.Add(new Word("positive", "confident"));
        wordsListNormal.Add(new Word("university", "college"));
        wordsListNormal.Add(new Word("mission", "assignment "));
        wordsListNormal.Add(new Word("relationship", "connection"));
        wordsListNormal.Add(new Word("explanation", "reason"));
        wordsListNormal.Add(new Word("grandmother", "parent"));
        wordsListNormal.Add(new Word("january", "month"));
        wordsListNormal.Add(new Word("advice", "guidance"));
        wordsListNormal.Add(new Word("memento", "reminder"));

        wordsListHard.Add(new Word("pennsylvania", "state"));
        wordsListHard.Add(new Word("fervid", "ardent"));
        wordsListHard.Add(new Word("oxygen", "air"));
        wordsListHard.Add(new Word("rogue", "dishonest "));
        wordsListHard.Add(new Word("mystify", "puzzle"));
        wordsListHard.Add(new Word("wagon", "train"));
        wordsListHard.Add(new Word("jukebox", "music"));
        wordsListHard.Add(new Word("shallow", "superficial"));
        wordsListHard.Add(new Word("thumb", "finger"));
        wordsListHard.Add(new Word("blizzard", "cold"));
    }

    /**
     * Escolhe uma palabra aleatoria para ser selecionada no jogo.
     */
    Word ChooseRandomWord()
    {
        List<Word> currentWordsList = new List<Word>();
        switch (level)
        {
            case 1:
                currentWordsList = wordsListEasy;
                break;
            case 2:
                currentWordsList = wordsListNormal;
                break;
            case 3:
                currentWordsList = wordsListHard;
                break;

        }
        int wordPosition = Random.Range(0, currentWordsList.Count);
        Word word = currentWordsList[wordPosition];
        //Debug.Log(word.word);
        return word;
    }

    /**
     * Valida se a resposta digitada esta correta.
     */
    public void ValidateAnswer()
    {
        if (answer.text.ToUpper().Equals(wordSelected.word.ToUpper()))
        {
            wordsCorrect++;
            currentWordText.text = wordSelected.word.ToUpper();
            InstantiateIngredient();
        } else
        {
            PlaySoundEffect(wordWrongSound);
            numberOfLives--;
            if (numberOfLives < 0)
            {
                GameOver();
            }
            else
            {
                RevealALetter();
            }
        }
        answer.text = " ";
    }

    /**
     * Mostra a tela de game over.
     */

    void GameOver()
    {
        gameOverPanel.SetActive(true);
        turnObjectVisibility(false);
    }

    /*
     * Joga o ingrediente no caldeirão.
     */
    void InstantiateIngredient()
    {
        Instantiate(ingredient);
        PlaySoundEffect(wordCorrectSound);
    }

    /**
     * Esconde a palavra que deve ser adivinhada.
     */
    void HideWord()
    {
        currentWordText.text = "";
        for (int i = 0; i < wordSelected.word.Length; i++)
        {
            currentWordText.text += "_ ";
        }
    }

    /**
     * Revela uma letra aleatoria como dica para o jogador.
     */
    void RevealALetter()
    {
        StringBuilder word = new StringBuilder(currentWordText.text);
        word.Replace(" ", string.Empty);

        int numberOfTries = 0;
        int randomLetterPosition;
        do
        {
            numberOfTries++;
            randomLetterPosition = Random.Range(0, word.Length);
        } while (numberOfTries < word.Length && word[randomLetterPosition] != '_');

        word[randomLetterPosition] = wordSelected.word[randomLetterPosition];
        currentWordText.text = word.ToString();
        currentWordText.text = AddSpaceBetweenLetters(currentWordText.text);
    }

    /**
     * Adiciona um espaço entre as letras
     */
    string AddSpaceBetweenLetters(string word)
    {
        //Debug.Log(word);
        StringBuilder wordWithSpaces = new StringBuilder();
        for (int i = 0; i < word.Length; i++)
        {
            wordWithSpaces.Append(word[i].ToString() + " ");
        }

        return wordWithSpaces.ToString();
    }

    /**
     * Remove a palavra descoberta e passa para a proxima fase.
     */
    public void NextWord()
    {

        switch (level)
        {
            case 1:
                wordsListEasy.Remove(wordSelected);
                break;
            case 2:
                wordsListNormal.Remove(wordSelected);
                break;
            case 3:
                wordsListHard.Remove(wordSelected);
                break;

        }

        if(wordsCorrect == 3)
        {
            levelCompletePanel.SetActive(true);
            turnObjectVisibility(false);
        } else
        {
            wordSelected = ChooseRandomWord();
            currentTipText.text = wordSelected.tip.ToUpper();
            HideWord();
        }
    }

    /**
     * Passa para a próxima fase.
     */
    public void NextLevel()
    {
        level++;
        SceneManager.LoadScene(level);
    }

    /**
     * Tela de conclusão do jogo. 
     */
    public void EndGame()
    {
        theEndPanel.SetActive(true);
        turnObjectVisibility(false);
    }

    /**
     * Reinicia a fase atual.
     */
    public void restartLevel()
    {
        SceneManager.LoadScene(level);
    }

    /**
     * Toca o efeito sonoro
     */
    public void PlaySoundEffect(AudioClip audio)
    {
        audioController.PlayOneShot(audio);
    }

    /**
     * Adiciona todas as musicas em uma lista para serem tocadas de acordo com a fase atual.
     */
    public void AddMusicsToList()
    {
        musicLevels.Add(level1Music);
        musicLevels.Add(level2Music);
        musicLevels.Add(level3Music);
    }

    /**
     * Toca a musica de background do jogo.
     */
    public void PlayMusic(AudioClip music)
    {
        audioController.clip = music;
        audioController.Play();
    }

    public void turnObjectVisibility(bool visible)
    {
        caldron.SetActive(visible);
    }
}
