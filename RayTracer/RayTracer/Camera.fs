module Camera

open System
open Globals
open Ray

type Camera(position:Vec3,lookAt:Vec3,up:Vec3,vfov:float,aspect:float) = 
    let unitAngle = Math.PI / 180.0 * vfov
    let halfHeight = Math.Tan(unitAngle * 0.5)
    let halfWidth = aspect * halfHeight

    let w = (lookAt - position).Normalized()
    let u = Vec3.Cross(up,w).Normalized()
    let v = Vec3.Cross(w,u).Normalized()

    let lowLeftCorner = position + w - halfWidth * u - halfHeight * v
    let horizontal = 2.0 * halfWidth * u
    let vertical = 2.0 * halfHeight * v

    member this.CreateRay(u:float,v:float) = 
        Ray(position,lowLeftCorner + u*horizontal + v*vertical - position)