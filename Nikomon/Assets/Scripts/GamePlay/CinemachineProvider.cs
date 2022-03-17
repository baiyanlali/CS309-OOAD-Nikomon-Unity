using Cinemachine;

public class CinemachineProvider : CinemachineInputProvider
{
    public static bool InputEnabled = true;

    public override float GetAxisValue(int axis)
    {
        if (!InputEnabled)
            return 0;
        return base.GetAxisValue(axis);
    }
}