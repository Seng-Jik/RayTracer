namespace Material
open Globals
open Hitable
open Ray
open Material.MaterialFuncs

type Dielectirc(albedo:Vec3,refIdx:float) =
    interface IMaterial with
        member this.Scatter(ray:Ray,record:HitRecord) rand : (Ray option*Vec3) =
            let reflect = lazy (Reflect ray.Direction record.Normal)
            let (normal,niNo,cos) =
                match Vector.Dot(ray.Direction,record.Normal) > 0.0 with
                | true -> (
                            -record.Normal,
                            refIdx,
                            refIdx * Vector.Dot(ray.DirectionNorm,record.Normal))
                | false -> (
                            record.Normal,
                            1.0/refIdx,
                            -Vector.Dot(ray.DirectionNorm, record.Normal))

            let (reflectProb,refracted) = match Refract ray.Direction normal niNo with
                                            | Some(refracted) -> (Schlick cos refIdx,Some(refracted))
                                            | None -> (1.0,None)

            let outRay = match rand.NextDouble() < reflectProb with
                            | true -> Ray(record.Position, reflect.Force())
                            | false -> Ray(record.Position,refracted.Value)
                
            (Some(outRay),albedo)

        member this.Emitted with get() = Vec3(0.0,0.0,0.0)

