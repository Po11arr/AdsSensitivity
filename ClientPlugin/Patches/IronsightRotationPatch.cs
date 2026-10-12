using HarmonyLib;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRageMath;

namespace ClientPlugin.Patches;

// Client-only: the server receives the resulting orientation (MyMoveNetCommand carries a
// quaternion with a zero rotation indicator), so scaling the local input is multiplayer safe.
[HarmonyPatch(typeof(MyCharacter), nameof(MyCharacter.MoveAndRotate), typeof(Vector3), typeof(Vector2), typeof(float))]
// ReSharper disable once UnusedType.Global
public static class IronsightRotationPatch
{
    [HarmonyPrefix]
    // ReSharper disable once UnusedMember.Global
    public static void Prefix(MyCharacter __instance, ref Vector2 rotationIndicator)
    {
        if (rotationIndicator.X == 0f && rotationIndicator.Y == 0f)
            return;

        var config = Config.Current;
        if (!config.Enabled || __instance.ZoomMode != MyZoomModeEnum.IronSight)
            return;

        var session = MySession.Static;
        if (session == null || !ReferenceEquals(session.LocalCharacter, __instance))
            return;

        rotationIndicator *= AimScaling.CorrectionFactor(config);
    }
}
