using UnityEngine;
using UnityEngine.Playables;

public class RumbleClip : PlayableAsset
{
    [Tooltip("Assign a RumbleProfile ScriptableObject.")]
    public RumbleProfile profile;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<RumbleBehaviour>.Create(graph);
        playable.GetBehaviour().profile = profile;
        return playable;
    }
}