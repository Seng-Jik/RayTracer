module Main

open System
open Globals
open Hitable
open Material
open Camera
open Window

let (width,height) = (128*12,72*12)

let light = 10.0

let light1 = Lambertian(Vec3(0.0, 0.3, 0.3))
light1.SetEmitted(Vec3(0.0,light,0.0))

let light2 = Metal(Vec3(0.8, 0.3, 0.3))
light2.SetEmitted(Vec3(light,0.0,0.0))

let light3 = Dielectirc(Vec3(0.0,0.5,0.5),2.5)
light3.SetEmitted(Vec3(0.0,0.0,light))

let hitableList : (IHitable*IMaterial) list = [
    (Sphere(Vec3(-2.0,0.0,-2.0),0.5) :> IHitable, light1 :> IMaterial)
    (Sphere(Vec3(2.0,0.0,-2.0),0.5) :> IHitable, light2 :> IMaterial)
    (Sphere(Vec3(0.0,0.0,-2.0),0.5) :> IHitable, light3 :> IMaterial)
    (Sphere(Vec3(-1.0,0.0,-1.0),0.5) :> IHitable,LambertianMetal(Vec3(1.0,1.0,1.0),0.5) :> IMaterial)
    (Sphere(Vec3(1.0,0.0,-1.0),0.5) :> IHitable,Dielectirc(Vec3(1.0,1.0,1.0),1.5) :> IMaterial)
    (Sphere(Vec3(0.0,0.5,-4.0),1.0) :> IHitable, Lambertian(Vec3(1.0,0.0,0.5)) :> IMaterial)
    
    (Sphere(Vec3(0.0,-100.5,-1.),100.0) :> IHitable,Lambertian(Vec3(0.0,0.1,0.2)) :> IMaterial)]

let from = Vec3(0.0, 3.0, 7.0);
let lookat = Vec3(0.0, 0.5, -1.0);
let camera = Camera(from,lookat,Vec3(0.0,1.0,0.0),20.0,(float width/float height),0.0,1.)

let spp = 100000

ParallelRenderer.RenderParallel width height spp hitableList camera
