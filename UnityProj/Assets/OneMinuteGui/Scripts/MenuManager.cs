using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace beffio.OneMinuteGUI
{
	public class MenuManager : MonoBehaviour 
    {
	    public string AnimationPropertyName
        {
            get
            {
                return m_animationPropertyName;
            }

            set
            {
                m_animationPropertyName = value;
            }
        }

	    public GameObject InitialScreen
        {
            get
            {
                return m_initialScreen;
            }

            set
            {
                m_initialScreen = value;
            }
        }

	    public List<GameObject> NavigationHistory
        {
            get
            {
                return m_navigationHistory;
            }

            set
            {
                m_navigationHistory = value;
            }
        }
	
	    [SerializeField]
	    private string m_animationPropertyName;

	    [SerializeField]
	    private GameObject m_initialScreen;

	    [SerializeField]
	    private List<GameObject> m_navigationHistory;
        

            public void GoBack()
	        {
		        if (m_navigationHistory.Count > 1)
		        {
                    int index = m_navigationHistory.Count - 1;
			        Animate(m_navigationHistory[index - 1], true);

                    //Slice Of Sky
                    //When controls are showed in the begin of a level the back button goes to the initial menu so the pause must go off
                    if (m_navigationHistory[index].name == "ControlsMenu" && m_navigationHistory[index - 1].name == "InitialMenu")
                    {
                        GameMaster.GM.gamePaused = false;
                    }

                    GameObject target = m_navigationHistory[index];
			        m_navigationHistory.RemoveAt(index);
			        Animate(target, false);
		        }
	        }

	    public void GoToMenu(GameObject target)
	    {
		    if (target == null)
		    {
			    return;
		    }

		    if (m_navigationHistory.Count > 0)
		    {
			    Animate(m_navigationHistory[m_navigationHistory.Count - 1], false);
		    }

		    m_navigationHistory.Add(target);
		    Animate(target, true);
	    }

	    private void Animate(GameObject target, bool direction)
	    {
		    if (target == null)
		    {
			    return;
		    }

		    target.SetActive(true);

		    Canvas canvasComponent = target.GetComponent<Canvas>();
		    if (canvasComponent != null)
		    {
			    canvasComponent.overrideSorting = true;
			    canvasComponent.sortingOrder = m_navigationHistory.Count;
		    }

		    Animator animatorComponent = target.GetComponent<Animator>();
		    if (animatorComponent != null)
		    {
			    animatorComponent.SetBool(m_animationPropertyName, direction);
		    }
	    }
        
	    private void Awake()
	    {
		    m_navigationHistory = new List<GameObject>{m_initialScreen};
	    }


	    //Added by Coubz
        public Toggle invertYAxeToggle;
        public Toggle showControlsWhenGameStarts;

        private void Start()
        {
            //Slice Of Sky
            //Set up the checkbox value depending on the saved data
            if(invertYAxeToggle != null && showControlsWhenGameStarts != null)
            {
                invertYAxeToggle.isOn = PlayerData.PD.invertYAxis;
                showControlsWhenGameStarts.isOn = PlayerData.PD.alwaysShowControls;
            }
        }

	    public void StartGame()
	    {
		    Application.LoadLevel(1);
	    }

	    public void ExitGame()
	    {
		    Application.Quit();
	    }

	    public void invertAxis(bool _invert)
	    {
		    PlayerData.PD.invertYAxis = _invert;
	    }

        public void showControlsWhenGameStart(bool _show)
        {
            PlayerData.PD.alwaysShowControls = _show;
        }

	    public Slider soundSlider;

	    public void MuteSound(bool _isSound)
	    {
		    AudioListener.pause = !_isSound;
		    if (_isSound)
		    {
			    soundSlider.enabled = true;
		    }
		    else
		    {
			    soundSlider.enabled = false;
		    }
	    }

	    public void SetVolume(float _vol)
	    {
		    AudioListener.volume = _vol;
	    }

	    public void PauseGame(bool _setPause)
	    {
		    if (_setPause != GameMaster.GM.gamePaused)
		    {
			    GameMaster.GM.gamePaused = _setPause;
		    }
	    }

	    public void BackToMainMenu()
	    {
		    Application.LoadLevel(0);
	    }

	    public void Replay()
	    {
		    Application.LoadLevel(PlayerData.PD.getLastScore().level);
	    }

        public GameObject moveTuto;
        public GameObject shootTuto;
        public GameObject speedTuto;

        public void setUpControlsTuto(int _tutoLevel)
        {
            if (_tutoLevel >= 1)
            {
                moveTuto.SetActive(true);
            }
            else
            {
                moveTuto.SetActive(false);
            }

            if (_tutoLevel >= 2)
            {
                shootTuto.SetActive(true);
            }
            else
            {
                shootTuto.SetActive(false);
            }

            if (_tutoLevel >= 3)
            {
                speedTuto.SetActive(true);
            }
            else
            {
                speedTuto.SetActive(false);
            }
        }
    }
}
