module Ray

open Globals

type Ray(org:Vec3,dir:Vec3) = 
    let dirNorm = dir.Normalized()

    member this.GetPoint (t:float) = 
        org + t * dir

    member this.DirectionNorm with get() = dirNorm
    member this.Orginal with get() = org
    member this.Direction with get() = dir

type HitRecord = {
    Normal : Vec3 
    Color : Vec3
    Position : Vec3
    RayT : float }