using Stride.BepuPhysics;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using System.Collections.Generic;

namespace RaycastBugRepro;

public class RaycastScript : SyncScript
{
    public override void Update()
    {
        DebugText.Print("""
            Try hovering the mouse over cubes, from Cube1 to Cube48.
            Moving from Cube1 to Cube48, you will notice that raycast will be less accurate the more you progress.
            When you reach Cube48, the raycast will not hit any cubes, instead, the inaccuracy will be so high
            that the raycast detects a hit on Cube48 when the mouse will be upon Cube47 instead.

            Move with WASD keys and look around with right click on mouse. Hold Shift to move faster.
            """.Replace("\r", ""), new Int2(250, 30));

        var camera = Entity.Get<CameraComponent>();

        var backBuffer = GraphicsDevice.Presenter.BackBuffer;
        var viewPort = new Viewport(0, 0, backBuffer.Width, backBuffer.Height);

        var nearPoint = viewPort.Unproject(new Vector3(Input.AbsoluteMousePosition, 0), camera.ProjectionMatrix, camera.ViewMatrix, Matrix.Identity);
        var farPoint = viewPort.Unproject(new Vector3(Input.AbsoluteMousePosition, 1.0f), camera.ProjectionMatrix, camera.ViewMatrix, Matrix.Identity);

        // simple raycast
        var y = 15;
        if (camera.Entity.GetSimulation().RayCast(nearPoint, farPoint, 1000, out var hitInfo))
        {
            DebugText.Print("Direct Hit: " + hitInfo.Collidable.Entity.Name, new Int2(10, y));
        }
        else
        {
            DebugText.Print("No hit", new Int2(10, y));
        }

        // penetrating raycast
        y += 25;
        List<HitInfo> hits = new List<HitInfo>();
        camera.Entity.GetSimulation().RayCastPenetrating(nearPoint, farPoint, 1000, hits);

        for (int i = 0; i < hits.Count; i++)
        {
            HitInfo hit = hits[i];
            DebugText.Print($"Penetrating Hit {i}: {hit.Collidable.Entity.Name}", new Int2(10, y));
            y += 16;
        }
    }
}
