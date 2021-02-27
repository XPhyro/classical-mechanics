using MaterialUI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Force
{
    public class Player : MonoBehaviour
    {
        #region Editor Variables
        [Header("Editor Variables")]
        private Rigidbody rb;

        private IEnumerator pauseCoroutine;
        private IEnumerator stopCoroutine;
        #endregion

        #region Simulation Properties
        [Space]
        [Header("Simulation Properties")]
        [SerializeField]
        [ReadOnly]
        private float force;
        [SerializeField]
        [ReadOnly]
        private float speed;
        [SerializeField]
        [ReadOnly]
        private float acceleration;
        [SerializeField]
        [ReadOnly]
        private float kineticEnergy;
        [SerializeField]
        [ReadOnly]
        private float momentum;
        #endregion

        #region UI
        #region Simulation Configuration
        [Space]
        [Header("Simulation Configuration")]
        [SerializeField]
        private Slider forceSlider;
        [SerializeField]
        private Slider massSlider;
        [SerializeField]
        private InputField forceInputField;
        [SerializeField]
        private InputField massInputField;
        [SerializeField]
        private SelectionBoxConfig dirSelectionBox;
        [SerializeField]
        private Toggle runToggle;
        [SerializeField]
        private Slider pauseAfterSlider;
        [SerializeField]
        private InputField pauseAfterInputField;
        [SerializeField]
        private Slider stopAfterSlider;
        [SerializeField]
        private InputField stopAfterInputField;
        [SerializeField]
        private Slider timeScaleSlider;
        [SerializeField]
        private InputField timeScaleInputField;
        #endregion
        #region Hud Configuration
        [Space]
        [Header("Hud Configuration")]
        [SerializeField]
        private Toggle showAllToggle;
        [SerializeField]
        private Toggle showNoneToggle;
        [SerializeField]
        private Toggle[] showToggles;
        #endregion
        #region Hud
        [Space]
        [Header("Hud")]
        [SerializeField]
        private GameObject[] showGos;
        [SerializeField]
        private Text speedText;
        [SerializeField]
        private Text accelerationText;
        [SerializeField]
        private Text kineticEnergyText;
        [SerializeField]
        private Text momentumText;
        #endregion
        #region Viewport
        [Space]
        [Header("Viewport")]
        [SerializeField]
        private Toggle cameraPresetToggle;
        [SerializeField]
        private GameObject cameraSlidersGo;
        [SerializeField]
        private GameObject cameraRadiosGo;
        [SerializeField]
        private Slider[] vectorSliders;
        [SerializeField]
        private InputField[] vectorInputFields;
        [SerializeField]
        private Toggle[] cameraPresetRadios;
        [SerializeField]
        private Vector3[] positionPresets;
        [SerializeField]
        private Vector3[] rotationPresets;
        #endregion
        #region Spotlight Colors
        [Space]
        [Header("Spotlight Colors")]
        [SerializeField]
        private Light[] lights;
        [SerializeField]
        private Slider lightIntensitySlider;
        [SerializeField]
        private InputField lightIntensityInputfield;
        [SerializeField]
        private Slider lightRangeSlider;
        [SerializeField]
        private InputField lightRangeInputField;
        [SerializeField]
        private Slider lightSpotAngleSlider;
        [SerializeField]
        private InputField lightSpotAngleInputField;
        [SerializeField]
        private Toggle colorPresetToggle;
        [SerializeField]
        private GameObject colorSlidersGo;
        [SerializeField]
        private GameObject colorRadiosGo;
        [SerializeField]
        private Slider[] colorSliders;
        [SerializeField]
        private InputField[] colorInputFields;
        [SerializeField]
        private Toggle[] colorPresetRadios;
        [SerializeField]
        private Color[] colorPresets;
        #endregion
        #endregion

        private int lastSelectedCameraPreset;
        private int lastSelectedColorPreset;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();

            SetupListeners();
            SetupVariables();
        }

        private void Update()
        {
            if(Math.Abs(transform.position.z) > 100000)
            {
                transform.position = Vector3.zero;
            }

            if(runToggle.isOn)
            {
                UpdateGenericUI();
            }
        }

        private void FixedUpdate()
        {
            rb.AddForce(Vector3.forward * force);
        }

        private void SetupVariables()
        {
            force = forceSlider.value = 0;
            massSlider.value = rb.mass = 1;
            pauseAfterSlider.value = 0;
            stopAfterSlider.value = 0;
            Time.timeScale = timeScaleSlider.value = 1;

            UpdateStaticUI();
            UpdateGenericUI();
        }

        private void SetupListeners()
        {
            forceSlider.onValueChanged.AddListener(delegate { SliderUpdateForce(); });
            massSlider.onValueChanged.AddListener(delegate { SliderUpdateMass(); });
            forceInputField.onEndEdit.AddListener(delegate { InputUpdate(forceInputField.text, forceSlider); });
            massInputField.onEndEdit.AddListener(delegate { InputUpdate(massInputField.text, massSlider); });
            dirSelectionBox.ItemPicked += UpdateDir;
            runToggle.onValueChanged.AddListener(delegate { UpdateRunSimulation(); });
            pauseAfterSlider.onValueChanged.AddListener(delegate { SliderUpdatePauseAfter(); });
            pauseAfterInputField.onEndEdit.AddListener(delegate { InputUpdate(pauseAfterInputField.text, pauseAfterSlider); });
            stopAfterSlider.onValueChanged.AddListener(delegate { SliderUpdateStopAfter(); });
            stopAfterInputField.onEndEdit.AddListener(delegate { InputUpdate(stopAfterInputField.text, stopAfterSlider); });
            timeScaleSlider.onValueChanged.AddListener(delegate { SliderUpdateTimeScale(); });
            timeScaleInputField.onEndEdit.AddListener(delegate { InputUpdate(timeScaleInputField.text, timeScaleSlider); });

            showAllToggle.onValueChanged.AddListener(delegate { ShowAll(); });
            showNoneToggle.onValueChanged.AddListener(delegate { ShowNone(); });

            //for some reason this doesn't work when in a for loop, find the reason
            showToggles[0].onValueChanged.AddListener(delegate { UpdateShow(0); });
            showToggles[1].onValueChanged.AddListener(delegate { UpdateShow(1); });
            showToggles[2].onValueChanged.AddListener(delegate { UpdateShow(2); });
            showToggles[3].onValueChanged.AddListener(delegate { UpdateShow(3); });

            cameraPresetToggle.onValueChanged.AddListener(delegate { UpdateCameraPresetChoice(); });

            foreach(var v in vectorSliders)
            {
                v.onValueChanged.AddListener(delegate { SliderUpdateCameraPosition(); });
            }

            foreach(var f in vectorInputFields)
            {
                f.onEndEdit.AddListener(delegate { InputUpdateCameraPosition(); });
            }

            //for some reason this doesn't work when in a for loop, find the reason
            cameraPresetRadios[0].onValueChanged.AddListener(delegate { UpdateCameraPresetIndex(0); });
            cameraPresetRadios[1].onValueChanged.AddListener(delegate { UpdateCameraPresetIndex(1); });
            cameraPresetRadios[2].onValueChanged.AddListener(delegate { UpdateCameraPresetIndex(2); });
            cameraPresetRadios[3].onValueChanged.AddListener(delegate { UpdateCameraPresetIndex(3); });
            cameraPresetRadios[4].onValueChanged.AddListener(delegate { UpdateCameraPresetIndex(4); });
            cameraPresetRadios[5].onValueChanged.AddListener(delegate { UpdateCameraPresetIndex(5); });
            cameraPresetRadios[6].onValueChanged.AddListener(delegate { UpdateCameraPresetIndex(6); });
            cameraPresetRadios[7].onValueChanged.AddListener(delegate { UpdateCameraPresetIndex(7); });

            lightIntensitySlider.onValueChanged.AddListener(delegate { SliderUpdateLight(); });
            lightIntensityInputfield.onEndEdit.AddListener(delegate { InputUpdate(lightIntensityInputfield.text, lightIntensitySlider); });
            lightRangeSlider.onValueChanged.AddListener(delegate { SliderUpdateLight(); });
            lightRangeInputField.onEndEdit.AddListener(delegate { InputUpdate(lightRangeInputField.text, lightRangeSlider); });
            lightSpotAngleSlider.onValueChanged.AddListener(delegate { SliderUpdateLight(); });
            lightSpotAngleInputField.onEndEdit.AddListener(delegate { InputUpdate(lightSpotAngleInputField.text, lightSpotAngleSlider); });

            colorPresetToggle.onValueChanged.AddListener(delegate { UpdateColorPresetChoice(); });

            foreach(var c in colorSliders)
            {
                c.onValueChanged.AddListener(delegate { SliderUpdateColor(); });
            }

            foreach(var f in colorInputFields)
            {
                f.onEndEdit.AddListener(delegate { InputUpdateColor(); });
            }

            //for some reason this doesn't work when in a for loop, find the reason
            colorPresetRadios[0].onValueChanged.AddListener(delegate { UpdateColorPresetIndex(0); });
            colorPresetRadios[1].onValueChanged.AddListener(delegate { UpdateColorPresetIndex(1); });
            colorPresetRadios[2].onValueChanged.AddListener(delegate { UpdateColorPresetIndex(2); });
            colorPresetRadios[3].onValueChanged.AddListener(delegate { UpdateColorPresetIndex(3); });
            colorPresetRadios[4].onValueChanged.AddListener(delegate { UpdateColorPresetIndex(4); });
            colorPresetRadios[5].onValueChanged.AddListener(delegate { UpdateColorPresetIndex(5); });
            colorPresetRadios[6].onValueChanged.AddListener(delegate { UpdateColorPresetIndex(6); });
            colorPresetRadios[7].onValueChanged.AddListener(delegate { UpdateColorPresetIndex(7); });
        }

        private void CalculateGenericVariables()
        {
            speed = rb.velocity.z;
            kineticEnergy = rb.mass * speed * speed / 2;
            momentum = rb.mass * speed;
        }

        private void CalculateStaticVariables()
        {
            acceleration = Mathf.Abs(force / rb.mass);
        }

        private void UpdateGenericUI()
        {
            CalculateGenericVariables();

            speedText.text = "Speed(m/s): " + Mathf.Abs((float)Math.Round(speed, 1));
            kineticEnergyText.text = "Kinetic Energy(J): " + Math.Round(kineticEnergy, 1);
            momentumText.text = "Momentum(kgm/s): " + Math.Round(momentum, 1);
        }

        private void UpdateStaticUI()
        {
            CalculateStaticVariables();

            accelerationText.text = "Acceleration(m/s²): " + Math.Round(acceleration, 1);

            if(acceleration != 0)
            {
                accelerationText.text += dirSelectionBox.listItems[dirSelectionBox.currentSelection].Substring(0, 1);
            }
        }

        private void UpdateRunSimulation()
        {
            if(runToggle.isOn)
            {
                rb.isKinematic = false;

                if(pauseCoroutine != null)
                {
                    StopCoroutine(pauseCoroutine);
                }
                if(stopCoroutine != null)
                {
                    StopCoroutine(stopCoroutine);
                }

                if(pauseAfterSlider.value > 0 && dirSelectionBox.currentSelection != 1)
                {
                    pauseAfterSlider.value = (float)Math.Round(pauseAfterSlider.value, 1);

                    pauseCoroutine = PauseAfter();
                    StartCoroutine(pauseCoroutine);
                }
                else if(stopAfterSlider.value > 0)
                {
                    stopAfterSlider.value = (float)Math.Round(stopAfterSlider.value, 1);

                    stopCoroutine = StopAfter();
                    StartCoroutine(stopCoroutine);
                }
            }
            else
            {
                if(stopCoroutine != null)
                {
                    StopCoroutine(stopCoroutine);
                }

                rb.isKinematic = true;
            }
        }

        private void InputUpdate(string text, Slider slider)
        {
            if(text != "")
            {
                float f;
                float.TryParse(text, out f);
                slider.value = f;
            }
        }

        private void SliderUpdateMass()
        {
            massSlider.value = (float)Math.Round(massSlider.value, 1);
            rb.mass = massSlider.value;

            UpdateStaticUI();
        }

        private void SliderUpdateForce()
        {
            forceSlider.value = (float)Math.Round(forceSlider.value, 1);
            force = forceSlider.value * (dirSelectionBox.currentSelection - 1);

            UpdateStaticUI();
        }

        private void UpdateDir(int id)
        {
            if(pauseCoroutine != null)
            {
                StopCoroutine(pauseCoroutine);
            }

            force = forceSlider.value * (id - 1);
            UpdateStaticUI();
        }

        private IEnumerator PauseAfter()
        {
            yield return new WaitForSeconds(pauseAfterSlider.value);
            dirSelectionBox.Select(1);
        }

        private IEnumerator StopAfter()
        {
            yield return new WaitForSeconds(stopAfterSlider.value);
            runToggle.isOn = false;
        }

        private void SliderUpdatePauseAfter()
        {
            stopAfterSlider.value = 0;
        }

        private void SliderUpdateStopAfter()
        {
            pauseAfterSlider.value = 0;
        }

        private void SliderUpdateTimeScale()
        {
            timeScaleSlider.value = (float)Math.Round(timeScaleSlider.value, 1);
            Time.timeScale = timeScaleSlider.value;
        }

        private void ShowAll()
        {
            if(showAllToggle.isOn)
            {
                showNoneToggle.isOn = false;

                foreach(var t in showToggles)
                {
                    t.isOn = true;
                }
            }
        }

        private void ShowNone()
        {
            if(showNoneToggle.isOn)
            {
                showAllToggle.isOn = false;

                foreach(var t in showToggles)
                {
                    t.isOn = false;
                }
            }
        }

        private void UpdateShow(int i)
        {
            if(showGos[i].activeSelf)
            {
                showGos[i].SetActive(false);
                showAllToggle.isOn = false;
            }
            else
            {
                showGos[i].SetActive(true);
                showNoneToggle.isOn = false;
            }

            var areTrue = true;
            var areFalse = false;

            foreach(var t in showToggles)
            {
                if(t.isOn)
                {
                    areFalse = true;
                }
                else
                {
                    areTrue = false;
                }
            }

            if(areTrue)
            {
                showAllToggle.isOn = true;
            }
            else if(!areFalse)
            {
                showNoneToggle.isOn = true;
            }
        }

        private void UpdateCameraPresetChoice()
        {
            cameraRadiosGo.SetActive(cameraPresetToggle.isOn);
            cameraSlidersGo.SetActive(!cameraPresetToggle.isOn);

            if(cameraPresetToggle.isOn)
            {
                UpdateCameraPresetIndex(lastSelectedCameraPreset);
            }
            else
            {
                SliderUpdateCameraPosition();
            }
        }

        private void SliderUpdateCameraPosition()
        {
            Camera.main.transform.localPosition = new Vector3(vectorSliders[0].value, vectorSliders[1].value, vectorSliders[2].value);
            Camera.main.transform.LookAt(transform);
        }

        private void InputUpdateCameraPosition()
        {
            for(int i = 0; i < vectorInputFields.Length; i++)
            {
                if(vectorInputFields[i].text == "" || vectorInputFields[i].text == null)
                {
                    continue;
                }

                float f;
                float.TryParse(vectorInputFields[i].text, out f);
                vectorSliders[i].value = (float)Math.Round(f, 2);
            }
        }

        private void UpdateCameraPresetIndex(int i)
        {
            Camera.main.transform.localPosition = positionPresets[i];
            Camera.main.transform.localEulerAngles = rotationPresets[i];

            lastSelectedCameraPreset = i;
        }

        private void SliderUpdateLight()
        {
            foreach(var l in lights)
            {
                l.intensity = lightIntensitySlider.value;
                l.range = lightRangeSlider.value;
                l.spotAngle = lightSpotAngleSlider.value;
            }
        }

        private void UpdateColorPresetChoice()
        {
            colorRadiosGo.SetActive(colorPresetToggle.isOn);
            colorSlidersGo.SetActive(!colorPresetToggle.isOn);

            if(colorPresetToggle.isOn)
            {
                UpdateColorPresetIndex(lastSelectedColorPreset);
            }
            else
            {
                SliderUpdateColor();
            }
        }

        private void SliderUpdateColor()
        {
            var c = new Color(colorSliders[0].value / 255, colorSliders[1].value / 255, colorSliders[2].value / 255);

            foreach(var l in lights)
            {
                l.color = c;
            }
        }

        private void InputUpdateColor()
        {
            for(int i = 0; i < colorInputFields.Length; i++)
            {
                if(colorInputFields[i].text == "" || colorInputFields[i].text == null)
                {
                    continue;
                }

                float f;
                float.TryParse(colorInputFields[i].text, out f);
                colorSliders[i].value = (float)Math.Round(f, 2);
            }
        }

        private void UpdateColorPresetIndex(int i)
        {
            foreach(var l in lights)
            {
                l.color = colorPresets[i];
            }

            lastSelectedColorPreset = i;
        }
    }
}
