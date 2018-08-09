module Main

open System
open Globals
open Hitable
open Window


let (width,height) = (128*6,72*5)

let hitableList : IHitable list = [
    Sphere(Vec3(0.0,0.0,-1.0),0.5)
    Sphere(Vec3(0.0,-100.5,-1.),100.0)]

let image = hitableList |> CreateImageForTestRay (Drawing.Size(width,height)) 100

Window.DisplayImage image

