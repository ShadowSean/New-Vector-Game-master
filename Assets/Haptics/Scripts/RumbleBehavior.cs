using UnityEngine;
using UnityEngine.Playables;

public class RumbleBehaviour : PlayableBehaviour
{
    public RumbleProfile profile;
    public bool priority;
    bool _playing;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (!Application.isPlaying || _playing) return;
        _playing = true;
        RumbleManager.Instance?.Play(profile, priority);
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (!Application.isPlaying) return;
        _playing = false;
        RumbleManager.Instance?.StopAll();
    }
}