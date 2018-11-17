namespace Material
open Globals
open Hitable
open Ray
open Material.MaterialFuncs

type Lambertian(albedo:Vec3) = 
    let mutable emitted = Vec3(0.0,0.0,0.0)
    member this.SetEmitted (col:Vec3) = emitted <- col

    interface IMaterial with
        member this.Scatter(ray:Ray,record:HitRecord) rand : (Ray option*Vec3) =
            let target = record.Position + record.Normal + GetDiffuseDirection rand
            let ray = Ray(record.Position,target - record.Position)
            (Some(ray),albedo)

        member this.Emitted = emitted

   