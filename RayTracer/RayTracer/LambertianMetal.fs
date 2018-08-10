namespace Material
open Globals
open Hitable
open Ray

type LambertianMetal(albedo:Vec3,fuzz:float) = 
    let metal = Metal(albedo) :> IMaterial
    interface IMaterial with
        member this.Scatter(ray:Ray,record:HitRecord) : (Ray option*Vec3) =
            let rand = System.Random(int (ray.Direction.Y * 9999999.0))
            let GetDiffuseDirection () =
                let p = 
                    2.0 * Vec3(rand.NextDouble(),rand.NextDouble(),rand.NextDouble()) - 
                    Vec3.One
                p.Normalized() * rand.NextDouble()

            let (met,_) = metal.Scatter(ray,record)

            match met with
            | None -> (None,albedo)
            | Some(ray) -> 
                let metalDir = ray.Direction
                let dir = metalDir + GetDiffuseDirection() * fuzz
                (Some(Ray(ray.Orginal,dir)),albedo)
