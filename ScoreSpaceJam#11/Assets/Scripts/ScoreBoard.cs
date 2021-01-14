using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class BoardField
{
    public string PlayerName;
    public int Score;
    public EField boardfield;
    public BoardField(string PlayerName, int Score, EField boardfield)
    {
        this.PlayerName = PlayerName;
        this.Score = Score;
        this.boardfield = boardfield;
    }
}
public class ScoreBoard : MonoBehaviour
{

    public int DesiredNameCount;
    string[] Names =
    {
        "Xxx","Gamer","Killer","Bob","Jake","Hopper","Destroyer","Cod","Terminator","Sebastian","Claude",
        "Zlatan","Genie","Cristiano","Jakoba","Anthony","Melissa","Julius","Daniela","Keith","Roberta","Stephanie",
        "Hacker","German","Mexican","Irish","Italian","German","Russsian","English","brave","fat","creepy",
        "kind","ugly","brown","intelligent","skinny","ruthless","curvy","pretty","fox","puppy","frog",
        "owl","War","Lion","giraffe", "_","-","*"," "," "
    };

    public GameObject emptyfield;

    [HideInInspector]public List<BoardField> AddedBoard;

    public List<BoardField> ListedBoard;

    public List<GameObject> Allfields;


    public Transform ListHolder;

    public Color firstColor;
    public Color PlayerColor;

    private string PlayerName;


    int PlayerHighestScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        PlayerName = PlayerPrefs.GetString("Name");
        if (FindObjectOfType<GameManger>().Score > PlayerPrefs.GetInt("HighScore")) PlayerPrefs.SetInt("HighScore", FindObjectOfType<GameManger>().Score);
        PlayerHighestScore = PlayerPrefs.GetInt("HighScore");
        AddRandomizedNames();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddRandomizedNames()
    {

        for (int i = 0; i < DesiredNameCount-1; i++)
        {
            int stringcountinname = Random.Range(2, 4);
            string Name = "";
            int Score = 0;
            for(int k = 0; k < stringcountinname; k++)
            {
                int RandName = Random.Range(0, Names.Length);
                Name += Names[RandName];
            }
            Score = Random.Range(100, 13000);
            CreateField(Name, Score);
        }
        CreateField(PlayerName, PlayerHighestScore);
        ListBoard();
    }

    void CreateField(string name,int score)
    {
        GameObject newfieldGO = Instantiate(emptyfield);
        newfieldGO.transform.parent = ListHolder;
        newfieldGO.transform.localScale = new Vector3(10f,6f,0f);
        EField newfield = newfieldGO.GetComponent<EmptyScorField>().field;
        newfield.name.text= name;
        newfield.score.text = score.ToString();
        BoardField field = new BoardField(name, score,newfield);
        Allfields.Add(newfieldGO);
        AddedBoard.Add(field);
    }


    void ListBoard()
    {
        int HighestScore = 0;
        int[] AllBoardScore = new int[AddedBoard.Count];
        for (int i = 0; i < DesiredNameCount; i++)
        {
            AllBoardScore[i] = AddedBoard[i].Score;
        }
        for (int k = 0; k < DesiredNameCount; k++)
        {
            HighestScore = Mathf.Max(AllBoardScore);
            foreach (BoardField field in AddedBoard)
            {
                if (field.Score == HighestScore)
                {
                    ListedBoard.Add(field);
                    AddedBoard.Remove(field);
                    for (int j = 0; j < AllBoardScore.Length; j++)
                    {
                        if (field.Score == AllBoardScore[j])
                        {
                            AllBoardScore[j] = 0;
                        }
                    }
                    HighestScore = 0;
                    break;
                }
            }
        }

        for (int x = 0; x < Allfields.Count; x++)
        {
            EField f = Allfields[x].GetComponent<EmptyScorField>().field;
            f.name.text = x+1 +"." + ListedBoard[x].PlayerName;
            f.score.text = ListedBoard[x].Score.ToString();
            bool IsPlayer = PlayerName == ListedBoard[x].PlayerName;
            if (x == 0 && !IsPlayer)
            {
                f.name.color = firstColor;
                f.score.color = firstColor;
            }
            else if (IsPlayer)
            {
                f.name.color = PlayerColor;
                f.score.color = PlayerColor;
            }
        }
    }
}
