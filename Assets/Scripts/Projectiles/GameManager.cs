using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Projectiles
{
    public class GameManager : MonoBehaviour
    {
        #region Editor Variables
        public static GameManager Singleton;

        /// <summary>
        /// Gravity sphere's rigidbody.
        /// </summary>
        [Header("Editor Variables")]
        [Tooltip("Gravity sphere's rigidbody.")]
        [SerializeField]
        private Rigidbody grb;
        /// <summary>
        /// Throwed sphere's rigidbody.
        /// </summary>
        [Tooltip("Throwed sphere's rigidbody.")]
        [SerializeField]
        private Rigidbody trb;

        private IEnumerator pauseCoroutine;
        private IEnumerator stopCoroutine;

        private int lastSelectedCameraPreset;
        private int lastSelectedColorPreset;

        [SerializeField]
        private float forceSliderAccelerationConstant = 100f;
        [SerializeField]
        private float sliderMinTick = .1f;

        [SerializeField]
        [ReadOnly]
        private Vector3 defaultPos;
        #endregion

        #region Simulation Properties
        [Space]
        [Header("Simulation Properties")]
        [SerializeField]
        [ReadOnly]
        private Vector3 force;
        [SerializeField]
        [ReadOnly]
        private float speed;
        [SerializeField]
        [ReadOnly]
        private Vector3 acceleration;
        [SerializeField]
        [ReadOnly]
        private float kineticEnergy;
        [SerializeField]
        [ReadOnly]
        private float momentum;
        [SerializeField]
        [ReadOnly]
        private float potentialEnergy;
        [SerializeField]
        [ReadOnly]
        private Vector3 initialVelocity;
        [SerializeField]
        [ReadOnly]
        private float expectedMaxHeightTime;
        [SerializeField]
        [ReadOnly]
        private float expectedFlightTime;
        [SerializeField]
        [ReadOnly]
        private float expectedRemainingFlightTime;
        [SerializeField]
        [ReadOnly]
        private float expectedMaxHeight;
        [SerializeField]
        [ReadOnly]
        private Vector3 expectedMaxVelocity;
        [SerializeField]
        [ReadOnly]
        private float expectedMaxKineticEnergy;
        [SerializeField]
        [ReadOnly]
        private float expectedMaxMomentum;
        [SerializeField]
        [ReadOnly]
        private float expectedDistanceX;
        #endregion

        #region UI
        #region Simulation Configuration
        [Space]
        [Header("Simulation Configuration")]
        [SerializeField]
        private Slider forceXSlider;
        [SerializeField]
        private Slider forceYSlider;
        [SerializeField]
        private Slider massSlider;
        [SerializeField]
        private Slider gravitySlider;
        [SerializeField]
        private InputField gravityInputField;
        [SerializeField]
        private Slider heightSlider;
        [SerializeField]
        private InputField heightInputField;
        [SerializeField]
        private InputField forceXInputField;
        [SerializeField]
        private InputField forceYInputField;
        [SerializeField]
        private InputField massInputField;
        [SerializeField]
        private Slider velocityXSlider;
        [SerializeField]
        private Slider velocityYSlider;
        [SerializeField]
        private InputField velocityXInputField;
        [SerializeField]
        private InputField velocityYInputField;
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
        [SerializeField]
        private Toggle expectedToggle;
        #endregion
        #region Hud
        [Space]
        [Header("Hud")]
        [SerializeField]
        private GameObject[] showGos;
        [SerializeField]
        private Text speedText;
        [SerializeField]
        private Text velocityText;
        [SerializeField]
        private Text accelerationText;
        [SerializeField]
        private Text kineticEnergyText;
        [SerializeField]
        private Text momentumText;
        [SerializeField]
        private Text potentialEnergyText;
        [SerializeField]
        private Text heightText;
        [SerializeField]
        private Text distanceText;
        [SerializeField]
        private Text timeText;
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

        private void Start()
        {
            if(Singleton)
            {
                Destroy(this);
            }
            else
            {
                Singleton = this;
            }

            SetupListeners();
            SetupVariables();
        }

        private void Update()
        {
            if(Math.Abs(transform.position.z) > 100000)
            {
                transform.position = Vector3.zero;
            }

            if(runToggle.isOn && timeScaleSlider.value != 0)
            {
                UpdateGenericUI();
            }
        }

        private void FixedUpdate()
        {
            trb.AddForce(force);
        }

        private void SetupVariables()
        {
            forceXSlider.value = forceYSlider.value = 0;
            force = Vector3.zero;
            massSlider.value = 1;
            pauseAfterSlider.value = 0;
            stopAfterSlider.value = 0;

            timeScaleSlider.value = 1;

            defaultPos = trb.position;

            UpdateStaticUI();
            UpdateGenericUI();
            expectedRemainingFlightTime = 0;
            timeText.text = "0";
        }

        private void SetupListeners()
        {
            forceXSlider.onValueChanged.AddListener(delegate { UpdateForce(); });
            forceYSlider.onValueChanged.AddListener(delegate { UpdateForce(); });
            massSlider.onValueChanged.AddListener(delegate { UpdateMass(); });
            gravitySlider.onValueChanged.AddListener(delegate { UpdateGravity(); });
            gravityInputField.onEndEdit.AddListener(delegate { InputUpdate(gravityInputField.text, gravitySlider); });
            heightSlider.onValueChanged.AddListener(delegate { UpdateHeight(); });
            heightInputField.onEndEdit.AddListener(delegate { InputUpdate(heightInputField.text, heightSlider); });
            forceXInputField.onEndEdit.AddListener(delegate { InputUpdate(forceXInputField.text, forceXSlider); });
            forceYInputField.onEndEdit.AddListener(delegate { InputUpdate(forceYInputField.text, forceYSlider); });
            massInputField.onEndEdit.AddListener(delegate { InputUpdate(massInputField.text, massSlider); });
            velocityXSlider.onValueChanged.AddListener(delegate { UpdateVelocity(); });
            velocityYSlider.onValueChanged.AddListener(delegate { UpdateVelocity(); });
            velocityXInputField.onEndEdit.AddListener(delegate { InputUpdate(velocityXInputField.text, velocityXSlider); });
            velocityYInputField.onEndEdit.AddListener(delegate { InputUpdate(velocityYInputField.text, velocityYSlider); });
            runToggle.onValueChanged.AddListener(delegate { UpdateRunSimulation(); });
            pauseAfterSlider.onValueChanged.AddListener(delegate { UpdatePauseAfter(); });
            pauseAfterInputField.onEndEdit.AddListener(delegate { InputUpdate(pauseAfterInputField.text, pauseAfterSlider); });
            stopAfterSlider.onValueChanged.AddListener(delegate { UpdateStopAfter(); });
            stopAfterInputField.onEndEdit.AddListener(delegate { InputUpdate(stopAfterInputField.text, stopAfterSlider); });
            timeScaleSlider.onValueChanged.AddListener(delegate { UpdateTimeScale(); });
            timeScaleInputField.onEndEdit.AddListener(delegate { InputUpdate(timeScaleInputField.text, timeScaleSlider); });

            showAllToggle.onValueChanged.AddListener(delegate { ShowAll(); });
            showNoneToggle.onValueChanged.AddListener(delegate { ShowNone(); });

            //for some reason this doesn't work when in a for loop, find the reason
            showToggles[0].onValueChanged.AddListener(delegate { UpdateShow(0); });
            showToggles[1].onValueChanged.AddListener(delegate { UpdateShow(1); });
            showToggles[2].onValueChanged.AddListener(delegate { UpdateShow(2); });
            showToggles[3].onValueChanged.AddListener(delegate { UpdateShow(3); });
            showToggles[4].onValueChanged.AddListener(delegate { UpdateShow(4); });
            showToggles[5].onValueChanged.AddListener(delegate { UpdateShow(5); });
            showToggles[6].onValueChanged.AddListener(delegate { UpdateShow(6); });
            showToggles[7].onValueChanged.AddListener(delegate { UpdateShow(7); });
            showToggles[8].onValueChanged.AddListener(delegate { UpdateShow(8); });

            expectedToggle.onValueChanged.AddListener(delegate { UpdateExpectedUI(); });

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

            lightIntensitySlider.onValueChanged.AddListener(delegate { UpdateLight(); });
            lightIntensityInputfield.onEndEdit.AddListener(delegate { InputUpdate(lightIntensityInputfield.text, lightIntensitySlider); });
            lightRangeSlider.onValueChanged.AddListener(delegate { UpdateLight(); });
            lightRangeInputField.onEndEdit.AddListener(delegate { InputUpdate(lightRangeInputField.text, lightRangeSlider); });

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
            speed = trb.velocity.magnitude;
            kineticEnergy = trb.mass * speed * speed / 2;
            momentum = trb.mass * speed;
            potentialEnergy = trb.mass * -Physics.gravity.y * trb.position.y;

            expectedRemainingFlightTime -= Time.deltaTime;
        }

        private void CalculateStaticVariables()
        {
            acceleration.z = force.z / trb.mass;
            acceleration.y = force.y / trb.mass + Physics.gravity.y;
            potentialEnergy = trb.mass * -Physics.gravity.y * trb.position.y;
        }

        private void UpdateGenericUI()
        {
            if(!expectedToggle.isOn)
            {
                CalculateGenericVariables();

                velocityText.text = "Velocity(m/s): (" + Math.Round(trb.velocity.z, 1) + ", " + Math.Round(trb.velocity.y, 1) + ")";
                speedText.text = "Speed(m/s): " + Mathf.Abs((float)Math.Round(speed, 1));
                kineticEnergyText.text = "Kinetic Energy(J): " + Math.Round(kineticEnergy, 1);
                momentumText.text = "Momentum(kgm/s): " + Math.Round(momentum, 1);
                potentialEnergyText.text = "Potential Energy(J): " + Math.Round(potentialEnergy, 1);
                distanceText.text = "Distance(m): " + Math.Round(trb.position.z, 1);
                heightText.text = "Height(m): " + Math.Round(trb.position.y, 1);
                timeText.text = Math.Round(expectedRemainingFlightTime, 2).ToString();
            }
        }

        private void UpdateStaticUI()
        {
            CalculateStaticVariables();

            accelerationText.text = "Acceleration(m/s²): (" + Math.Round(acceleration.z, 1) + ", " + Math.Round(acceleration.y, 1) + ")";
            potentialEnergyText.text = "Potential Energy(J): " + Math.Round(trb.position.y, 1);
        }

        public void OnTCollision()
        {
            expectedRemainingFlightTime = 0;

            potentialEnergyText.text = "Potential Energy(J): 0";
            accelerationText.text = "Acceleration(m/s²): (" + Math.Round(acceleration.z, 1) + ", " + Math.Round(acceleration.y, 1) + ")";
            velocityText.text = "Velocity(m/s): (" + expectedMaxVelocity.z + ", " + expectedMaxVelocity.y + ")";
            var speed = expectedMaxVelocity.magnitude;
            speedText.text = "Speed(m/s): " + speed;
            kineticEnergyText.text = "Kinetic Energy(J): " + trb.mass * speed * speed / 2;
            momentumText.text = "Momentum(kgm/s): " + trb.mass * speed;
            distanceText.text = "Distance(m): " + expectedDistanceX;
            heightText.text = "Height(m): 0";
            timeText.text = "0";

            runToggle.isOn = false;
        }

        private void UpdateRunSimulation()
        {
            if(runToggle.isOn)
            {
                expectedMaxHeightTime = initialVelocity.y / Mathf.Abs(acceleration.y);
                expectedMaxHeight = initialVelocity.y * expectedMaxHeightTime / 2 + defaultPos.y;
                expectedFlightTime = (initialVelocity.y + Mathf.Sqrt(initialVelocity.y * initialVelocity.y + 2 * -acceleration.y * defaultPos.y)) / -acceleration.y;
                expectedRemainingFlightTime = expectedFlightTime;
                expectedDistanceX = expectedFlightTime * (initialVelocity.z + acceleration.z * expectedFlightTime / 2);
                expectedMaxVelocity.y = (expectedFlightTime - expectedMaxHeightTime) * acceleration.y;
                expectedMaxVelocity.z = initialVelocity.z + acceleration.z * expectedFlightTime;
                var expSpeed = expectedMaxVelocity.magnitude;
                expectedMaxMomentum = expSpeed * trb.mass;
                expectedMaxKineticEnergy = expectedMaxMomentum * expSpeed / 2;

                grb.isKinematic = false;
                trb.isKinematic = false;

                grb.position = new Vector3(0, defaultPos.y, defaultPos.z - 3);
                trb.position = defaultPos;

                trb.velocity = initialVelocity;

                Time.timeScale = timeScaleSlider.value;

                UpdateExpectedUI();

                if(pauseCoroutine != null)
                {
                    StopCoroutine(pauseCoroutine);
                }
                if(stopCoroutine != null)
                {
                    StopCoroutine(stopCoroutine);
                }

                if(pauseAfterSlider.value > 0)
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

                trb.position = new Vector3(0, 0, trb.position.z);
                grb.position = new Vector3(0, 0, defaultPos.z - 3);

                grb.isKinematic = true;
                trb.isKinematic = true;

                trb.mass = massSlider.value;
                Physics.gravity = new Vector3(0, -gravitySlider.value, 0);
                force.y = forceYSlider.value;
                force.z = forceXSlider.value;

                UpdateStaticUI();
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

        private void UpdateMass()
        {
            massSlider.value = (float)Math.Round(massSlider.value, 1);

            forceXSlider.maxValue = massSlider.value * forceSliderAccelerationConstant;
            forceYSlider.maxValue = massSlider.value * gravitySlider.value - .1f;

            if(!runToggle.isOn)
            {
                trb.mass = massSlider.value;
                UpdateStaticUI();
            }
        }

        private void UpdateGravity()
        {
            gravitySlider.value = (float)Math.Round(gravitySlider.value, 1);

            forceYSlider.maxValue = massSlider.value * gravitySlider.value - .1f;

            if(!runToggle.isOn)
            {
                Physics.gravity = new Vector3(0, -gravitySlider.value, 0);
                UpdateStaticUI();
            }
        }

        private void UpdateHeight()
        {
            heightSlider.value = (float)Math.Round(heightSlider.value, 1);
            defaultPos.y = heightSlider.value;

            if(!runToggle.isOn)
            {
                trb.position = defaultPos;
                grb.position = new Vector3(0, defaultPos.y, defaultPos.z - 3);
            }
        }

        private void UpdateForce()
        {
            forceXSlider.value = (float)Math.Round(forceXSlider.value, 1);
            forceYSlider.value = (float)Math.Round(forceYSlider.value, 1);

            if(!runToggle.isOn)
            {
                force.y = forceYSlider.value;
                force.z = forceXSlider.value;
                UpdateStaticUI();
            }
        }

        private void UpdateVelocity()
        {
            velocityXSlider.value = (float)Math.Round(velocityXSlider.value, 1);
            velocityYSlider.value = (float)Math.Round(velocityYSlider.value, 1);

            initialVelocity.y = velocityYSlider.value;
            initialVelocity.z = velocityXSlider.value;
        }

        private IEnumerator PauseAfter()
        {
            yield return new WaitForSeconds(pauseAfterSlider.value);
            timeScaleSlider.value = 0;
        }

        private IEnumerator StopAfter()
        {
            yield return new WaitForSeconds(stopAfterSlider.value);
            runToggle.isOn = false;
        }

        private void UpdatePauseAfter()
        {
            stopAfterSlider.value = 0;
        }

        private void UpdateStopAfter()
        {
            pauseAfterSlider.value = 0;
        }

        private void UpdateTimeScale()
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

        private void UpdateExpectedUI()
        {
            if(!expectedToggle.isOn)
            {
                timeText.text = expectedRemainingFlightTime.ToString();

                return;
            }

            velocityText.text = "Velocity(m/s): (" + expectedMaxVelocity.z + ", " + expectedMaxVelocity.y + ")";
            speedText.text = "Speed(m/s): " + expectedMaxVelocity.magnitude;
            kineticEnergyText.text = "Kinetic Energy(J): " + expectedMaxKineticEnergy;
            momentumText.text = "Momentum(kgm/s): " + expectedMaxMomentum;
            potentialEnergyText.text = "Potential Energy(J): " + 0;
            distanceText.text = "Distance(m): " + expectedDistanceX;
            heightText.text = "Height(m): " + 0;
            timeText.text = expectedFlightTime.ToString();
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
            Camera.main.transform.LookAt(trb.transform);
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
                vectorSliders[i].value = f;
            }
        }

        private void UpdateCameraPresetIndex(int i)
        {
            Camera.main.transform.localPosition = positionPresets[i];
            Camera.main.transform.localEulerAngles = rotationPresets[i];

            lastSelectedCameraPreset = i;
        }

        private void UpdateLight()
        {
            foreach(var l in lights)
            {
                l.intensity = lightIntensitySlider.value;
                l.range = lightRangeSlider.value;
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

                int j;
                int.TryParse(colorInputFields[i].text, out j);
                colorSliders[i].value = j;
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
