namespace Material
open Globals
open Hitable
open Ray
open Material.MaterialFuncs

type Metal(albedo:Vec3) = 
    interface IMaterial with
        member this.Scatter(ray:Ray,record:HitRecord) : (Ray option*Vec3) =
            let reflected = Reflect ray.DirectionNorm record.Normal
            let scattered = Ray(record.Position,reflected)

            if Vec3.Dot(scattered.Direction,record.Normal) > 0.0 then
                (Some(scattered),albedo)
            else
                (None,albedo)