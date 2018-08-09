namespace Hitable

open Ray
open Globals

type HitRecord = {
    Normal : Vec3 
    Position : Vec3
    RayT : float }

type IHitable = 
    abstract member Hit : Ray * float * float * IHitable list -> HitRecord option


