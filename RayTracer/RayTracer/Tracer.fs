module Tracer

open Globals
open Ray
open Hitable

let Background (ray:Ray) =
    let t = 0.5 * (ray.DirectionNorm.Y + 1.0)
    (1.0 - t) * Vec3(1.0,1.0,1.0) + t * Vec3(0.5,0.7,1.0)

let Trace (ray : Ray) (tmin:float) (tmax:float) (objs : IHitable list) : HitRecord option =
    let mutable closest = tmax
    let hited = objs 
                |> List.map (fun obj -> 
                    let record = obj.Hit(ray,tmin,closest,objs)
                    if record.IsSome then
                        closest <- record.Value.RayT
                    record)
                |> List.filter (fun record -> record.IsSome)
                |> List.map (fun record -> record.Value)

    match hited with
    | [] -> None
    | hited ->
        Some(hited
            |> List.reduce (fun x y ->
                if x.RayT <= y.RayT then y else x))
