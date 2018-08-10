namespace Material

open Ray
open Hitable
open Globals

type IMaterial = 
    abstract member Scatter : Ray * HitRecord * Vec3 -> (Ray option*Vec3)