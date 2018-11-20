namespace Material
open Globals
open Hitable
open Ray
open Material.MaterialFuncs

type LambertianMetal(albedo:Vec3,fuzz:float) = 
    let mutable emitted = Vec3(0.0,0.0,0.0)
    let metal = Metal(albedo) :> IMaterial
    member this.SetEmitted (col:Vec3) = emitted <- col
    interface IMaterial with
        member this.Scatter(ray:Ray,record:HitRecord) rand : (Ray option*Vec3) =
            let (met,_) = metal.Scatter(ray,record) rand

            match met with
            | None -> (None,albedo)
            | Some(ray) -> 
                let metalDir = ray.Direction
                let dir = metalDir + GetDiffuseDirection(rand) * fuzz
                (Some(Ray(ray.Orginal,dir)),albedo)

        member this.Emitted with get() = emitted