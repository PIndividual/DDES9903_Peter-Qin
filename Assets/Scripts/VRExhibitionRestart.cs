
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class VRExhibitionRestart : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField]
    private string welcomeSceneName = "Welcome VR";

    [Header("Hold Settings")]
    [SerializeField]
    private float promptDelay = 2f;

    [SerializeField]
    private float restartDelay = 5f;

    [Header("UI")]
    [SerializeField]
    private GameObject restartPrompt;

    private InputDevice leftController;
    private float holdTimer = 0f;
    private bool isRestarting = false;
    private bool isPaused = false;
    private bool hasFocus = true;

    private void Start()
    {
        CancelRestart();
    }

    private void Update()
    {
        if (isRestarting || isPaused || !hasFocus)
            return;

        // Find the left XR controller
        if (!leftController.isValid)
        {
            leftController =
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        }

        // Read the X button on Quest Touch controllers
        bool isHoldingX = false;

        if (leftController.isValid)
        {
            leftController.TryGetFeatureValue(
                CommonUsages.primaryButton,
                out isHoldingX
            );
        }

        // Releasing X cancels the entire process
        if (!isHoldingX)
        {
            if (holdTimer > 0f)
                CancelRestart();

            return;
        }

        // Use unscaled time so this works even when
        // the game's timeScale is zero
        holdTimer += Time.unscaledDeltaTime;

        // Show warning after 2 seconds
        if (holdTimer >= promptDelay)
        {
            if (restartPrompt != null &&
                !restartPrompt.activeSelf)
            {
                restartPrompt.SetActive(true);
            }
        }

        // Restart after 2 + 5 = 7 seconds
        if (holdTimer >= promptDelay + restartDelay)
        {
            RestartExperience();
        }
    }

    private void CancelRestart()
    {
        holdTimer = 0f;

        if (restartPrompt != null)
            restartPrompt.SetActive(false);
    }

    private void RestartExperience()
    {
        if (isRestarting)
            return;

        isRestarting = true;

        Debug.Log("[Exhibition] Restarting experience.");

        // Make sure the scene is in the Build Scene List
        if (!Application.CanStreamedLevelBeLoaded(
                welcomeSceneName))
        {
            Debug.LogError(
                "[Exhibition] Welcome scene not found: " +
                welcomeSceneName
            );

            isRestarting = false;
            CancelRestart();
            return;
        }

        CancelRestart();

        // Restore normal game speed
        Time.timeScale = 1f;

        // Load the welcome scene
        SceneManager.LoadScene(welcomeSceneName);
    }

    private void OnApplicationPause(bool paused)
    {
        isPaused = paused;
        CancelRestart();
    }

    private void OnApplicationFocus(bool focused)
    {
        hasFocus = focused;
        CancelRestart();
    }

    private void OnDisable()
    {
        CancelRestart();
    }
}
