namespace Material

open Ray
open Hitable
open Globals
open System

type IMaterial = 
    abstract member Scatter : Ray * HitRecord -> (Ray option*Vec3)
    abstract Emitted : Vec3 with get

module MaterialFuncs =
    let GetDiffuseDirection (rand:Random) =
        let p = 
            2.0 * Vec3(rand.NextDouble(),rand.NextDouble(),rand.NextDouble()) - 
            Vec3.One
        p.Normalized() * rand.NextDouble()

    let Reflect (vin:Vec3) (normal:Vec3) = 
        vin - 2.0 * Vec3.Dot(vin,normal) * normal

    let Refract (vin:Vec3) (normal:Vec3) (niNo:float) : Vec3 option =
        let uvin = vin.Normalized()
        let dt = Vec3.Dot(uvin,normal)
        let disc = 1.0 - niNo * niNo * (1.0 - dt * dt)
        match disc with
        | disc when disc > 0.0 ->
            Some(niNo * (uvin - normal * dt) - normal * Math.Sqrt(disc))
        | _ -> None

    let Schlick (cos:float) (refIdx:float) =
        let r = (1.0-refIdx) / (1.0+refIdx)
        let r2 = r*r
        r2 + (1.0-r2)*Math.Pow((1.0-cos),5.0)