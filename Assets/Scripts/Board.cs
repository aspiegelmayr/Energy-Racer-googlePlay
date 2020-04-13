using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Proyecto26;
using SimpleJSON;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public int offset;
    public GameObject tilePrefab;
    private BackgroundTile[,] allTiles;
    public GameObject[] dots;
    public GameObject[,] allDots;
    public static int curScore;
    public int neededScore;
    public Text ownedCoinsText, coinsText;
    public Text movesText;
    public Text neededScoreText;
    public static int remainingMoves;
    public bool gameOver;
    public bool gameWon;
    public Slider slider;
    public Text playerName;
    public static Car car;
    public static Upgrade upgrade;
    public static int startingMoves;

    public int clouds;
    public string city;
    public double hoursInADay;
    public double Clouds;
    public static int earnedCoins;
    public static int curDistr;
    public int level;
    public Text location;

    public Car activeCar;

    static GameObject gameController;
    static LocationService locationService;

    public Image bonusMoveImg;
    //  public static bool backgroundIsSet;

    public Sprite carImg;

    //multiplayer stuff
    public Sprite localCar, opponentCar;
    public static bool isMultiplayer;
    public static bool isOnlineMultiplayer;
    public static string curPlayer;
    public Text curPlayerText;
    public Slider player2Slider;
    public Text player2Name;
    public static int curPlayer2Score;
    public string winner;
    public static string player1Nickname;
    public static string player2Nickname;
    public Text playerNickname;

    /// <summary>
    /// the game board is initialized, dots and background tiles are created
    /// </summary>
    void Start()
    {
        ownedCoinsText.text = "$" + StartGame.coins;
        if(StartGame.activeUpgrade == null)
        {
            bonusMoveImg.enabled = false;
        } else
        {
            bonusMoveImg.sprite = StartGame.activeUpgrade.upgradeImg;
            bonusMoveImg.enabled = true;
        }
        location.text = LevelSelection.districtName;
        isMultiplayer = LevelSelection.isMultiplayer;
        playerNickname.enabled = false;
        
        if (isMultiplayer)
        {
            ownedCoinsText.enabled = false;
            curPlayerText.text = curPlayer;
            slider.image.sprite = MultiplayerMenu.player1Sprite;
            playerName.text = player1Nickname;
            player2Name.text = player2Nickname;
            player2Slider.gameObject.SetActive(true);
            player2Slider.image.sprite = MultiplayerMenu.player2Sprite;
            coinsText.enabled = false;
            playerNickname.text = curPlayer;
            playerNickname.enabled = true;
            
        }
        else if (isOnlineMultiplayer)
        {
            ownedCoinsText.enabled = false;
            coinsText.enabled = false;
            movesText.enabled = false;
            if(Matchmaking.role == "host")
            {
                slider.image.sprite = localCar;
                player2Slider.image.sprite = opponentCar;
            } else
            {
                curDistr = Matchmaking.level;
                player2Slider.image.sprite = localCar;
                slider.image.sprite = opponentCar;
                curDistr = Matchmaking.level;
            }
            playerName.text = Matchmaking.hostName;
            player2Name.text = Matchmaking.guestName;
            player2Slider.gameObject.SetActive(true);
            RestClient.Get("https://energyracer.firebaseio.com/lobby/" + Matchmaking.matchID + ".json").Then(response => {
                var result = JSON.Parse(response.Text);
                if(result == null)
                {
                    SceneManager.LoadScene("StartScene");
                } else
                {
                    InvokeRepeating("GetOpponentScore", 0.0f, 5f);
                }
            });
        }
        else if(!isMultiplayer && !isOnlineMultiplayer)
        {
            player2Name.enabled = false;
            playerName.enabled = false;
            curPlayerText.text = " ";
            player2Slider.gameObject.SetActive(false);
        }

        gameController = GameObject.Find("GameController");

        earnedCoins = 30;
        coinsText.text = "+" + earnedCoins;

        width = 7;
        height = 7;
        allTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];

        locationService = gameController.GetComponent<LocationService>();
        locationService.SendWeatherRequest();

        movesText.text = remainingMoves.ToString();
        if (!isMultiplayer && !isOnlineMultiplayer)
        {
            carImg = StartGame.activeCar.img;
            slider.image.sprite = carImg;
        }
    }

    /// Update is called once per frame
    /// <summary>
    /// set level difficulty, display remaining moves
    /// </summary>
    public void Update()
    {
        if (level == 0)
        {
            level = GetLevel();
        }
            movesText.text = remainingMoves.ToString();
    }

    public int GetLevel()
    {
        return level;
    }

    /// <summary>
    /// update needed score, moves, and set current score to 0.
    /// check if upgrade is activated
    /// fill the game board with dots
    /// </summary>
    /// <param name="boardHeight"></param> the game board height
    /// <param name="boardWidth"></param> the game board width
    /// <param name="startMoves"></param> the moves 
    /// <param name="scoreToReach"></param> needed score to win
    /// <param name="level"></param> 
    public void Setup(int boardHeight, int boardWidth, int startMoves, int scoreToReach, int _level)
    {
        level = _level;
        /// Setup happens at the end of LocationService Coroutine
        curScore = 0;
        neededScore = scoreToReach;
        startingMoves = startMoves;
        remainingMoves = startMoves;

        slider.maxValue = neededScore;
        slider.value = 0;

        if (isMultiplayer || isOnlineMultiplayer)
        {
            player2Slider.maxValue = neededScore;
            player2Slider.value = 0;
        }

        if (upgrade != null)
        {
            remainingMoves += upgrade.bonusMoves;
        }

        if (car != null)
        {
            neededScore -= car.movesReduction;
        }

        width = boardWidth;
        height = boardHeight;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j + offset);
                GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = transform;
                backgroundTile.name = i + ", " + j;

                int dotToUse = SetDotToUse(level);

                int maxIter = 0;
                while (MatchesAt(i, j, dots[dotToUse]) && maxIter < 100)
                {
                    dotToUse = SetDotToUse(level);
                    maxIter++;
                }

                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity) as GameObject;
                dot.GetComponent<Dot>().col = i;
                dot.GetComponent<Dot>().row = j;

                dot.transform.parent = transform;
                dot.name = i + ", " + j;
                allDots[i, j] = dot;
            }
        }
    } /// SetUp method

    /// <summary>
    /// sets the dot that is used for the game board
    /// </summary>
    /// <param name="_level"></param>
    /// <returns>a random dot</returns>
    private int SetDotToUse(int _level)
    {
        int dotToUse;
        if (_level == 1)
        {
            dotToUse = Random.Range(0, dots.Length - 2);
        }
        else if (_level == 2)
        {
            dotToUse = Random.Range(1, dots.Length - 2);
        }
        else if (_level == 3)
        {
            dotToUse = Random.Range(1, dots.Length - 1);
        }
        else if (_level == 4)
        {
            dotToUse = Random.Range(2, dots.Length - 1);
        }
        else if (_level == 5)
        {
            dotToUse = Random.Range(2, dots.Length);
        }
        else
        {
            /// completely random, when night time or no location
            dotToUse = Random.Range(0, dots.Length);
        }
        return dotToUse;
    } /// SetDotToUse method

    /// <summary>
    ///  checks if a certain piece is matched
    /// </summary>
    /// <param name="col"></param> piece's column
    /// <param name="row"></param> piece's row
    /// <param name="piece"></param> the piece
    /// <returns>true if the dot is matched</returns>
    private bool MatchesAt(int col, int row, GameObject piece)
    {
        if (col > 1 && row > 1)
        {
            if (allDots[col - 1, row].tag == piece.tag && allDots[col - 2, row].tag == piece.tag)
            {
                return true;
            }
            if (allDots[col, row - 1].tag == piece.tag && allDots[col, row - 2].tag == piece.tag)
            {
                return true;
            }
        }
        else if (col <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (allDots[col, row - 1].tag == piece.tag && allDots[col, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }
            if (col > 1)
            {
                if (allDots[col - 1, row].tag == piece.tag && allDots[col - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// makes matched objects disappear
    /// </summary>
    /// <param name="col"></param> the column of matched piece
    /// <param name="row"></param> the row of matched piece
    private void DestroyMatchesAt(int col, int row)
    {
        if (allDots[col, row].GetComponent<Dot>().isMatched)
        {
            if (allDots[col, row].tag == "Battery" || allDots[col, row].tag == "Sun" ||
                allDots[col, row].tag == "Plug")
            {
                if (!isMultiplayer && !isOnlineMultiplayer ||
                    isMultiplayer && curPlayer == player1Nickname ||
                    isOnlineMultiplayer && Matchmaking.role == "host")
                {
                    curScore++;
                }
                else
                {
                    curPlayer2Score++;  
                }
                slider.value = curScore;
                if(isOnlineMultiplayer || isMultiplayer)
                {
                    player2Slider.value = curPlayer2Score;
                }
            }
            else if (allDots[col, row].tag == "Rain" || allDots[col, row].tag == "Cloud")
            {
                if (!isMultiplayer || curPlayer == player1Nickname)
                {
                    curScore--;

                }
                else
                {
                    curPlayer2Score--;

                }
            }

            if (curScore < 0)
            {
                curScore = 0;
            }

            slider.value = curScore;

            if (isMultiplayer)
            {
                if (curPlayer2Score < 0)
                {
                    curPlayer2Score = 0;
                }
                player2Slider.value = curPlayer2Score;
            }
            Destroy(allDots[col, row]);
            allDots[col, row] = null;
        }

    }

    /// <summary>
    /// destroys all matches on board
    /// check for win
    /// </summary>
    public void DestroyMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                    if (remainingMoves == 1)
                    {
                        movesText.text = "1 Move";
                    }
                    else
                    {
                        movesText.text = remainingMoves.ToString();
                    }
                    if (curScore >= neededScore)
                    {
                        SceneManager.LoadScene("GameWon");
                    }
                }
            }
        }
        StartCoroutine(DecreaseRowCo());
    }

    /// <summary>
    /// makes pieces fall down when match below disappears
    /// </summary>
    /// <returns></returns>
    private IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    allDots[i, j].GetComponent<Dot>().row -= nullCount;
                    allDots[i, j] = null;
                }

            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    /// <summary>
    /// spawns new pieces when match disappears
    /// </summary>
    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    Vector2 tempPos = new Vector2(i, j + offset);
                    int dotToUse = SetDotToUse(level);
                    GameObject piece = Instantiate(dots[dotToUse], tempPos, Quaternion.identity);
                    allDots[i, j] = piece;
                    piece.GetComponent<Dot>().col = i;
                    piece.GetComponent<Dot>().row = j;
                }
            }
        }
    }

    /// <summary>
    /// checks if there are matches on board
    /// </summary>
    /// <returns>true if matches on board, false if no matches</returns>
    private bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (allDots[i, j].GetComponent<Dot>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    /// <summary>
    /// destroys all matches that are still on board after refilling
    /// </summary>
    /// <returns></returns>
    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(.4f);

        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(.2f);
            DestroyMatches();
        }
        CheckGameOver();
    }

    public void switchPlayers()
    {
        if (Board.curPlayer == player1Nickname)
        {
            Board.curPlayer = player2Nickname;
        }
        else
        {
            Board.curPlayer = player1Nickname;
        }

        playerNickname.text = curPlayer;
    }

    /// <summary>
    /// checks if user lost
    /// </summary>
    public void CheckGameOver()
    {
        if (isMultiplayer)
        {
            curPlayerText.text = Board.curPlayer;

            if (curScore >= neededScore || remainingMoves == 0 && curScore > curPlayer2Score)
            {
                SceneManager.LoadScene("GameWon");
                winner = " ";
            }


            else if (curPlayer2Score >= neededScore && curScore < curPlayer2Score)
            {
                winner = player2Nickname;
                SceneManager.LoadScene("GameWon");
            }
            else if (remainingMoves == 0 && curScore > neededScore && curPlayer2Score > neededScore)
            {
                SceneManager.LoadScene("GameOver");
            }
            switchPlayers();
        } else if (isOnlineMultiplayer)
        {
             if(Matchmaking.role == "host")
            {
                if(curScore >= neededScore)
                {
                    earnedCoins = 20;
                    SceneManager.LoadScene("GameWon");
                } else if (curPlayer2Score >= neededScore)
                {
                    earnedCoins = -10;
                    SceneManager.LoadScene("GameOver"); 
                } 
            } else
            {
                if (curScore >= neededScore)
                {
                    earnedCoins = -10;
                    SceneManager.LoadScene("GameOver");
                }
                else if (curPlayer2Score >= neededScore)
                {
                    earnedCoins = 20;






                    SceneManager.LoadScene("GameWon");
                }
            }
        }
        else
        {
            if (curScore >= neededScore)
            {
                if (!isMultiplayer)
                {
                    SceneManager.LoadScene("GameWon");
                    DistrictSelection.unlockedDistricts++;
                }
            }
        }
        if (remainingMoves <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    /// <summary>
    /// manages coins
    /// </summary>
    public void updateCoins()
    {
        if (remainingMoves < 5)
        {
            earnedCoins = 10;
        }
        else if (remainingMoves <= 10 && remainingMoves >= 5)
        {
            earnedCoins = 20;
        }

        coinsText.text = "+" + earnedCoins;
    }

    void GetOpponentScore()
    {
        RestClient.Get("https://energyracer.firebaseio.com/lobby/" + Matchmaking.matchID + ".json").Then(response =>
        {
            var result = JSON.Parse(response.Text);

            if (Matchmaking.role == "host")
            {
                curPlayer2Score = result["guestScore"];
                player2Slider.value = curPlayer2Score;

            }
            else
            {
                curScore = result["hostScore"];
                slider.value = curScore;
            }
        });

    }

    void PostScore()
    {
        Match match;
        if(Matchmaking.role == "host")
        {
            match = new Match(Matchmaking.matchID, Matchmaking.hostName, Matchmaking.guestName, curScore, curPlayer2Score, false);
        } else
        {
            match = new Match(Matchmaking.matchID, Matchmaking.hostName, Matchmaking.guestName, curPlayer2Score, curScore, false);
        }
        
        RestClient.Put("https://energyracer.firebaseio.com/lobby/" + Matchmaking.matchID + ".json", match).Then(response =>
        {

        });
    }

    void DeleteDatabaseEntry()
    {
        RestClient.Delete("https://energyracer.firebaseio.com/lobby/" + Matchmaking.matchID + ".json");
    }
}