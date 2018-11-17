module Main

open System
open Globals
open Hitable
open Material
open Camera
open Window

let (width,height) = (128*4,72*4)

let light = Lambertian(Vec3(0.8, 0.3, 0.3))
light.SetEmitted(Vec3(0.1,0.1,0.1))

let hitableList : (IHitable*IMaterial) list = [
    (Sphere(Vec3(0.0,0.0,-1.0),0.5) :> IHitable, light :> IMaterial)
    (Sphere(Vec3(-1.0,0.0,-1.0),0.5) :> IHitable,LambertianMetal(Vec3(0.8,0.6,0.2),0.1) :> IMaterial)
    (Sphere(Vec3(1.0,0.0,-1.0),0.5) :> IHitable,Dielectirc(Vec3(1.0,1.0,1.0),1.5) :> IMaterial)
    
    (Sphere(Vec3(0.0,-100.5,-1.),100.0) :> IHitable,Lambertian(Vec3(0.8,0.8,0.0)) :> IMaterial)]

let from = Vec3(0.0, 3.0, 3.0);
let lookat = Vec3(0.0, 0.0, -1.0);
let camera = Camera(from,lookat,Vec3(0.0,1.0,0.0),20.0,(float width/float height),0.0,1.)

let spp = 50000

ParallelRenderer.RenderParallel width height spp hitableList camera
