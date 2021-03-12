using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class LineRendererModule
{
    // RENDER-RAY
    // Render the line as a ray from origin extending in direction for given length
    public static void RenderRay(this LineRenderer renderer, Ray ray, float length)
    {
        renderer.RenderRay(ray.origin, ray.direction, length);
    }
    public static void RenderRay(this LineRenderer renderer, Vector3 origin, Vector3 direction, float length)
    {
        renderer.RenderRay(origin, direction.normalized * length);
    }
    public static void RenderRay(this LineRenderer renderer, Ray ray)
    {
        renderer.RenderRay(ray.origin, ray.direction);
    }
    public static void RenderRay(this LineRenderer renderer, Vector3 origin, Vector3 direction)
    {
        renderer.positionCount = 2;
        renderer.SetPosition(0, origin);
        renderer.SetPosition(1, origin + direction);
    }
}
