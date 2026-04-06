using UnityEngine;
using UnityEngine.Timeline;

[TrackColor(0.9f, 0.45f, 0.1f)]
[TrackClipType(typeof(RumbleClip))]
[TrackBindingType(typeof(RumbleManager))]
public class RumbleTrack : TrackAsset { }