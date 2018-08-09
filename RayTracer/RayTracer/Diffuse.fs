module Diffuse
open Globals
open Hitable
open Ray
open Tracer

let rec GetDiffuseColor (ray:Ray) (objs:IHitable list) : Vec3 =
    let rand = System.Random(int (ray.Direction.Y * 9999999.0))
    let GetDiffuseDirection () =
        let p = 
            2.0 * Vec3(rand.NextDouble(),rand.NextDouble(),rand.NextDouble()) - 
            Vec3.One

        p.Normalized() * rand.NextDouble()
    
    match Trace ray 0.0000001 System.Double.MaxValue objs with
    | Some(record) -> 
        let target = record.Position + record.Normal + GetDiffuseDirection()
        0.5 * GetDiffuseColor (Ray(record.Position,target - record.Position)) objs

    | None -> Background ray
