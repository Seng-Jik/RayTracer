namespace Material
open Globals
open Hitable
open Ray
open Tracer

type Lambertian(albedo:Vec3) = 
    interface IMaterial with
        member this.Scatter(ray:Ray,record:HitRecord,atten:Vec3) : (Ray option*Vec3) =
            let rand = System.Random(int (ray.Direction.Y * 9999999.0))
            let GetDiffuseDirection () =
                let p = 
                    2.0 * Vec3(rand.NextDouble(),rand.NextDouble(),rand.NextDouble()) - 
                    Vec3.One

                p.Normalized() * rand.NextDouble()
            let target = record.Position + record.Normal + GetDiffuseDirection()
            let ray = Ray(record.Position,target - record.Position)
            (Some(ray),albedo)
