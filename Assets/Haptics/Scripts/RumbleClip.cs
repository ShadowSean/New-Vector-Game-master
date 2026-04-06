using UnityEngine;
using UnityEngine.Playables;

public class RumbleClip : PlayableAsset
{
    public RumbleProfile profile;
    public bool priority = false;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<RumbleBehaviour>.Create(graph);
        var b = playable.GetBehaviour();
        b.profile = profile;
        b.priority = priority;
        return playable;
    }
}