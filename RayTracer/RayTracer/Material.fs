namespace Material

open Ray
open Hitable
open Globals
open System

type IMaterial = 
    abstract member Scatter : Ray * HitRecord -> (Ray option*Vec3)

module MaterialFuncs =
    let GetDiffuseDirection (rand:Random) =
        let p = 
            2.0 * Vec3(rand.NextDouble(),rand.NextDouble(),rand.NextDouble()) - 
            Vec3.One
        p.Normalized() * rand.NextDouble()

    let Reflect (vin:Vec3) (normal:Vec3) = 
        vin - 2.0 * Vec3.Dot(vin,normal) * normal