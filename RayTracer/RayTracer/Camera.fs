module Camera

open System
open Globals
open Ray
open Material.MaterialFuncs

type Camera(position:Vec3,lookAt:Vec3,up:Vec3,vfov:float,aspect:float,r:float,focusDist:float) = 
    let radius = r * 0.5
    let unitAngle = Math.PI / 180.0 * vfov
    let halfHeight = Math.Tan(unitAngle * 0.5)
    let halfWidth = aspect * halfHeight

    let w = (lookAt - position).Normalized()
    let u = Vector.Cross(up,w).Normalized()
    let v = Vector.Cross(w,u).Normalized()

    let lowLeftCorner = position + w*focusDist - halfWidth * u * focusDist - halfHeight * v * focusDist
    let horizontal = 2.0 * halfWidth * u * focusDist
    let vertical = 2.0 * halfHeight * v * focusDist

    member this.CreateRay(u:float,v:float) rand = 
        let rd = radius * GetDiffuseDirection(rand)
        let t = rd.X * u + rd.Y * v
        let offset = Vec3(t,t,t)
        Ray(position + offset,lowLeftCorner + u*horizontal + v*vertical - position)