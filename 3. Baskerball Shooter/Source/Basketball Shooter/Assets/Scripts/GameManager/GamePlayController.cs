﻿using UnityEngine;using System.Collections;using UnityEngine.SceneManagement;public class GamePlayController : MonoBehaviour {    public void BackToMainMenu ()    {        SceneManager.LoadScene("MainMenu");    }}