module Tracer

open Globals
open Ray
open Hitable
open Material

type TraceRecord = { 
    HitRecord : HitRecord
    Material : IMaterial }

let Background (ray:Ray) =
    let t = 0.5 * (ray.DirectionNorm.Y + 1.0)
    (1.0 - t) * Vec3(1.0,1.0,1.0) + t * Vec3(0.5,0.7,1.0)

let Trace (ray : Ray) (tmin:float) (tmax:float) (objs : (IHitable*IMaterial) list) : TraceRecord option =
    let mutable closest = tmax
    let hited = objs 
                |> List.map (fun hitable -> 
                    let (obj,mat) = hitable
                    let record = obj.Hit(ray,tmin,closest)
                    if record.IsSome then
                        closest <- record.Value.RayT
                    (record,mat))
                |> List.filter (fun traceRecord -> 
                    let (record,_) = traceRecord
                    record.IsSome)
                |> List.map (fun traceRecord -> 
                    let (record,mat) = traceRecord
                    (record.Value,mat))

    let closest = match hited with
                    | [] -> None
                    | hited ->
                        Some(hited
                            |> List.reduce (fun xr yr ->
                                let (x,_) = xr
                                let (y,_) = yr
                                if x.RayT <= y.RayT then yr else xr))
    
    match closest with
    | None -> None
    | Some(a,b) -> Some({ HitRecord = a;Material = b })


