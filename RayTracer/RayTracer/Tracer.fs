module Tracer

open Globals
open Ray
open Hitable

let Background (ray:Ray) =
    let t = 0.5 * (ray.DirectionNorm.Y + 1.0)
    (1.0 - t) * Vec3(1.0,1.0,1.0) + t * Vec3(0.5,0.7,1.0)

let Trace (ray : Ray) (objs : IHitable list) =
    let hited = objs 
                |> List.map (fun obj -> obj.Hit(ray))
                |> List.filter (fun record -> record.IsSome)
                |> List.map (fun record -> record.Value)

    match hited with
    | [] -> Background ray
    | hited ->
        (hited
        |> List.reduce (fun x y ->
            if x.RayT <= y.RayT then y else x)).Color

