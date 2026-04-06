using UnityEngine;

/// Attach to your PlayableDirector GameObject.
/// Wire each Timeline Signal to the matching public method below.
public class RumbleSignalReceiver : MonoBehaviour
{
    [Header("Profiles — assign in Inspector")]
    public RumbleProfile distantFall;
    public RumbleProfile floorImpact;
    public RumbleProfile robotGrab;
    public RumbleProfile robotStep;
    public RumbleProfile jumpscare;
    public RumbleProfile cryopodOpen;

    // Called by Signal Emitter markers on the Timeline.
    public void OnDistantFall() => Play(distantFall);
    public void OnFloorImpact() => Play(floorImpact);
    public void OnRobotGrab() => Play(robotGrab);
    public void OnRobotStep() => Play(robotStep);
    public void OnJumpscare() => Play(jumpscare);
    public void OnCryopodOpen() => Play(cryopodOpen);

    void Play(RumbleProfile p) => RumbleManager.Instance?.Play(p);
}