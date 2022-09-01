
using UnityEngine;
using Holylib.SoundEffects;

namespace Holylib.UI
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject PauseCanvas;
        public bool isOnPauseMenu;
        [Space]
        // You Can Modify this part
        public CameraLook CameraScript;//
        private void Awake()
        {
            BackToGame();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isOnPauseMenu) { BackToGame(); } else { PauseGame(); }
            }
        }
        public void PauseGame()
        {
            Cursor.lockState = CursorLockMode.None;
            isOnPauseMenu = true;
            PauseCanvas.SetActive(true);
            Time.timeScale = 0;
            SoundEffectController.SFXVolume(-1,0);
            SoundEffectController.MusicVolume(-1, .2f);
        }
        public void BackToGame()
        {
            Cursor.lockState = CursorLockMode.Locked;
            isOnPauseMenu = false;
            PauseCanvas.SetActive(false);
            Time.timeScale = 1;
            SoundEffectController.SFXVolume(-1,1);
            SoundEffectController.MusicVolume(-1,1);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void MusicVolume(float Volume)
        {
            SoundEffectController.MusicVolume(-1, Volume, false);
        }

        public void SFXVolume(float Volume)
        {
            SoundEffectController.SFXVolume(-1, Volume, false);
        }

        public void MouseSensitivity(float Sensitivity)
        {
            // Set Sensitivity
            // You can modify this part
            CameraScript.Sensitivity = Sensitivity;
        }
    }
}
