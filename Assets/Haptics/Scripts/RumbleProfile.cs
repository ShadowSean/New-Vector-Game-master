using UnityEngine;

[CreateAssetMenu(menuName = "Haptics/Rumble Profile", fileName = "NewRumbleProfile")]
public class RumbleProfile : ScriptableObject
{
    [System.Serializable]
    public class Phase
    {
        public string label = "Phase";
        [Range(0f, 5f)] public float duration = 0.3f;
        public AnimationCurve lowMotor = AnimationCurve.Constant(0, 1, 0.5f);
        public AnimationCurve highMotor = AnimationCurve.Constant(0, 1, 0.5f);
    }

    public Phase[] phases;


    public static RumbleProfile DistantFall()
    {
        var p = CreateInstance<RumbleProfile>();
        p.phases = new[]
        {
            new Phase
            {
                label    = "Thud",
                duration = 0.12f,
                lowMotor  = Flat(0.15f),
                highMotor = Flat(0.05f),
            }
        };
        return p;
    }

    public static RumbleProfile FloorImpact()
    {
        var p = CreateInstance<RumbleProfile>();
        p.phases = new[]
        {
            new Phase
            {
                label    = "Impact",
                duration = 0.35f,
                lowMotor  = Decay(1.0f),
                highMotor = Decay(0.6f),
            }
        };
        return p;
    }

    public static RumbleProfile RobotGrab()
    {
        var p = CreateInstance<RumbleProfile>();
        p.phases = new[]
        {
            new Phase { label = "Contact",  duration = 0.08f, lowMotor = Flat(0.9f), highMotor = Flat(0.7f) },
            new Phase { label = "Scrape",   duration = 0.40f, lowMotor = Flat(0.3f), highMotor = Oscillate(0.6f, 0.1f, 8f, 0.40f) },
        };
        return p;
    }

    public static RumbleProfile RobotStep()
    {
        var p = CreateInstance<RumbleProfile>();
        p.phases = new[]
        {
            new Phase
            {
                label    = "Step",
                duration = 0.18f,
                lowMotor  = Decay(0.85f),
                highMotor = Flat(0.1f),
            }
        };
        return p;
    }

    public static RumbleProfile Jumpscare()
    {
        var p = CreateInstance<RumbleProfile>();
        p.phases = new[]
        {
            new Phase { label = "Flinch 1", duration = 0.10f, lowMotor = Flat(1f),   highMotor = Flat(1f) },
            new Phase { label = "Gap",      duration = 0.03f, lowMotor = Flat(0f),   highMotor = Flat(0f) },
            new Phase { label = "Flinch 2", duration = 0.06f, lowMotor = Flat(0.8f), highMotor = Flat(0.8f) },
        };
        return p;
    }

    public static RumbleProfile CryopodOpen()
    {
        var p = CreateInstance<RumbleProfile>();
        p.phases = new[]
        {
        new Phase { label = "Pressure build", duration = 1.5f, lowMotor  = Rise(0.1f, 0.4f),  highMotor = Rise(0.3f, 0.7f) },
        new Phase { label = "Release burst",  duration = 0.2f, lowMotor  = Flat(0.6f),         highMotor = Flat(1.0f) },
        new Phase { label = "Steam exhale",   duration = 0.8f, lowMotor  = Rise(0.2f, 0.9f),   highMotor = Rise(0.4f, 1.0f) },
    };
        return p;
    }


    static AnimationCurve Flat(float v) =>
        AnimationCurve.Constant(0, 1, v);

    static AnimationCurve Decay(float peak) =>
        new AnimationCurve(
            new Keyframe(0f, peak, 0f, -peak * 3f),
            new Keyframe(1f, 0f, -peak * 3f, 0f)
        );

    static AnimationCurve Rise(float from, float to) =>
        new AnimationCurve(
            new Keyframe(0f, from, 0f, (to - from) * 2f),
            new Keyframe(1f, to, (to - from) * 2f, 0f)
        );

    static AnimationCurve Oscillate(float center, float amp, float hz, float duration)
    {
        int samples = Mathf.Max(2, Mathf.RoundToInt(duration * hz * 2));
        var keys = new Keyframe[samples];
        for (int i = 0; i < samples; i++)
        {
            float t = (float)i / (samples - 1);
            float v = center + amp * Mathf.Sin(t * duration * hz * Mathf.PI * 2f);
            keys[i] = new Keyframe(t, Mathf.Clamp01(v));
        }
        return new AnimationCurve(keys);
    }
}