namespace Hitable

open Ray
open Globals

[<Struct>]
type HitRecord = {
    Normal : Vec3 
    Position : Vec3
    RayT : float }

type IHitable = 
    abstract member Hit : Ray * float * float -> HitRecord option


