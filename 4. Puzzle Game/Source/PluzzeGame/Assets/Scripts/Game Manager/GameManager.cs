﻿using UnityEngine;using System.Collections;using UnityEngine.SceneManagement;public class GameManager : MonoBehaviour {    public static GameManager instance;    private GameObject[] puzzlePieces;    private Sprite[] puzzleImages;    private PuzzlePiece[,] Maxtrix = new PuzzlePiece[GameVariables.MaxRows, GameVariables.MaxColumns];    private Vector3 screenPositionToAnimate;    private PuzzlePiece pieceToAnimate;    private int toAnimateRow, toAnimateColumn;    private float animSpeed = 10f;    private int puzzleIndex;    private GameState gameState;	// Use this for initialization	void Awake () {        MakeSingleton();	}		// Update is called once per frame	void Update () {        if (SceneManager.GetActiveScene().name == "GamePlay")        {            switch (gameState)            {                case GameState.Playing:                    CheckInput();                    break;                case GameState.Animating:                    AnimateMovement(pieceToAnimate, Time.deltaTime);                    CheckIfAnimationEnded();                    break;                case GameState.End:                    print("Game Over");                    break;            }        }		}    void Start ()    {        puzzleIndex = -1;    }    void MakeSingleton ()    {        if (instance == null)        {            instance = this;            DontDestroyOnLoad(gameObject);        }        else        {            Destroy(gameObject);        }    }    public void setIndex (int puzzleIndex)    {        this.puzzleIndex = puzzleIndex;    }    void LoadPuzzle ()    {        puzzleImages = Resources.LoadAll<Sprite>("Sprites/BG " + puzzleIndex);        puzzlePieces = GameObject.Find("Puzzle Holder").GetComponent<PuzzleHolder>().puzzlePieces;        for (int i = 0; i < puzzlePieces.Length; i++)        {            puzzlePieces[i].GetComponent<SpriteRenderer>().sprite = puzzleImages[i];        }    }    void OnLevelWasLoaded ()    {        print("OnLevelWasLoaded");        if (SceneManager.GetActiveScene().name == "GamePlay")        {            print("puzzleIndex =" + puzzleIndex);            if (puzzleIndex > 0)            {                print("LoadPuzzle");                LoadPuzzle();                GameStated();            }        }        else        {                    }    }            void GameStated ()    {        int index = Random.Range(0, GameVariables.MaxSize);        puzzlePieces[index].SetActive(false);        for (int row = 0; row < GameVariables.MaxRows; row++)        {            for (int column = 0; column < GameVariables.MaxColumns; column++)            {                if (puzzlePieces[row * GameVariables.MaxColumns + column].activeInHierarchy)                {                    Vector3 point = GetScreenCoordinatesFromViewPort(row, column);                    puzzlePieces[row * GameVariables.MaxColumns + column].transform.position = point;                    Maxtrix[row, column] = new PuzzlePiece();                    Maxtrix[row, column].gameObject = puzzlePieces[row * GameVariables.MaxColumns + column];                    Maxtrix[row, column].originalRow = row;                    Maxtrix[row, column].originalColumn = column;                }                else                {                    Maxtrix[row, column] = null;                }            }        }        Shuffle();        gameState = GameState.Playing;    }    private Vector3 GetScreenCoordinatesFromViewPort (int row, int column)    {        Vector3 point = Camera.main.ViewportToWorldPoint(new Vector3(0.225f * row, 1 - 0.235f * column, 0));        point.z = 0;        return point;    }    private void Shuffle ()    {        for (int row = 0; row < GameVariables.MaxRows; row++)        {            for (int column = 0; column < GameVariables.MaxColumns; column++)            {                if (Maxtrix[row, column] == null)                    continue;                int random_row = Random.Range(0, GameVariables.MaxRows);                int random_column = Random.Range(0, GameVariables.MaxColumns);                Swap(row, column, random_row, random_column);                                }        }    }    private void Swap (int row, int column, int random_row, int random_column)    {        PuzzlePiece temp = Maxtrix[row, column];        Maxtrix[row, column] = Maxtrix[random_row, random_column];        Maxtrix[random_row, random_column] = temp;        if (Maxtrix[row, column] != null)        {            Maxtrix[row, column].gameObject.transform.position = GetScreenCoordinatesFromViewPort(row, column);            Maxtrix[row, column].currentRow = row;            Maxtrix[row, column].currentColumn = column;        }        Maxtrix[random_row, random_column].gameObject.transform.position = GetScreenCoordinatesFromViewPort(random_row, random_column);        Maxtrix[random_row, random_column].currentRow = random_row;        Maxtrix[random_row, random_column].currentColumn = random_column;    }    private void CheckInput ()    {        if (Input.GetMouseButtonDown(0))        {            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);            if (hit.collider != null)            {                string[] parts = hit.collider.gameObject.name.Split('-');                int rowPath = int.Parse(parts[1]);                int columnPath = int.Parse(parts[2]);                int rowFound = -1;                 int columnFound = -1;                for (int row = 0; row < GameVariables.MaxRows; row++)                {                    if (rowFound != -1)                        break;                    for (int column = 0; column < GameVariables.MaxColumns; column ++)                    {                        if (rowFound != -1)                            break;                        if (Maxtrix[row, column] == null)                            continue;                        if (Maxtrix[row, column].originalRow == rowPath && Maxtrix[row, column].originalColumn == columnPath)                        {                            rowFound = row;                            columnFound = column;                        }                    }                }                bool pieceFound = false;                if (rowFound > 0 && Maxtrix[rowFound - 1, columnFound] == null)                {                    pieceFound = true;                    toAnimateRow = rowFound - 1;                    toAnimateColumn = columnFound;                }                else if (rowFound < GameVariables.MaxRows - 1 && Maxtrix[rowFound + 1, columnFound] == null)                {                                        pieceFound = true;                    toAnimateRow = rowFound + 1;                    toAnimateColumn = columnFound;                }                else if (columnFound > 0 && Maxtrix[rowFound, columnFound - 1] == null)                {                    pieceFound = true;                    toAnimateRow = rowFound;                    toAnimateColumn = columnFound - 1;                }                else if (columnFound < GameVariables.MaxColumns - 1 && Maxtrix[rowFound, columnFound + 1] == null)                {                    pieceFound = true;                    toAnimateRow = rowFound;                    toAnimateColumn = columnFound + 1;                }                if (pieceFound)                {                    screenPositionToAnimate = GetScreenCoordinatesFromViewPort(toAnimateRow, toAnimateColumn);                    pieceToAnimate = Maxtrix[rowFound, columnFound];                    gameState = GameState.Animating;                }            }        }    }    private void AnimateMovement (PuzzlePiece toMove, float time)    {        toMove.gameObject.transform.position = Vector2.MoveTowards(toMove.gameObject.transform.position,                                                                   screenPositionToAnimate,                                                                    animSpeed * time);            }    private void CheckIfAnimationEnded ()     {        if (Vector2.Distance(pieceToAnimate.gameObject.transform.position, screenPositionToAnimate) < 0.1f)        {            Swap(pieceToAnimate.currentRow, pieceToAnimate.currentColumn, toAnimateRow, toAnimateColumn);            gameState = GameState.Playing;            CheckForVictory();        }    }    private void CheckForVictory ()    {        for (int row = 0; row < GameVariables.MaxRows; row++)        {            for (int column = 0; column < GameVariables.MaxColumns; column++)            {                if (Maxtrix[row, column] == null)                {                    continue;                }                if (Maxtrix[row, column].originalRow != Maxtrix[row, column].currentRow                    || Maxtrix[row, column].originalColumn != Maxtrix[row, column].currentColumn)                {                    return;                }                            }        }        gameState = GameState.End;    }} // Game Manager