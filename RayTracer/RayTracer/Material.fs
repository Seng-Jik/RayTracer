namespace Material

open Ray
open Hitable
open Globals

type IMaterial = 
    abstract member Scatter : Ray * HitRecord -> (Ray option*Vec3)