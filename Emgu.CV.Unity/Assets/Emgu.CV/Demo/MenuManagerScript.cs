using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Emgu.CV.Demo
{
    public class MenuManagerScript : MonoBehaviour
    {

        public void LoadScene(String name)
        {
            SceneManager.LoadScene(name);
        }

        public void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
        }
    }
}