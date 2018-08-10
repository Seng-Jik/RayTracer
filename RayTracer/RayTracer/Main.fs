module Main

open System
open Globals
open Hitable
open Window
open Material


let (width,height) = (128*6,72*5)

let hitableList : (IHitable*IMaterial) list = [
    (Sphere(Vec3(0.0,0.0,-1.0),0.5) :> IHitable,Lambertian(Vec3(0.8, 0.3, 0.3)) :> IMaterial)
    (Sphere(Vec3(0.0,-100.5,-1.),100.0) :> IHitable,Lambertian(Vec3(0.8,0.8,0.0)) :> IMaterial)]

let image = hitableList |> CreateImageForTestRay (Drawing.Size(width,height)) 100

Window.DisplayImage image

