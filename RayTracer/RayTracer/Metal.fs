namespace Material
open Globals
open Hitable
open Ray

type Metal(albedo:Vec3) = 
    interface IMaterial with
        member this.Scatter(ray:Ray,record:HitRecord) : (Ray option*Vec3) =
            let Reflect (vin:Vec3) (normal:Vec3) = 
                vin - 2.0 * Vec3.Dot(vin,normal) * normal

            let reflected = Reflect ray.DirectionNorm record.Normal
            let scattered = Ray(record.Position,reflected)

            if Vec3.Dot(scattered.Direction,record.Normal) > 0.0 then
                (Some(scattered),albedo)
            else
                (None,albedo)