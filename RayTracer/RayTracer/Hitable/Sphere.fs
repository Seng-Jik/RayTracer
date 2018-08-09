namespace Hitable
open Globals
open Ray
open System
open System.Collections


type Sphere(center:Vec3,radius:float) =
    interface IHitable with
        member this.Hit(ray:Ray,tmin:float,tmax:float) : HitRecord option=
            let oc = ray.Orginal - center
            let a = Vec3.Dot(ray.Direction,ray.Direction)
            let b = 2.0 * Vec3.Dot(oc,ray.Direction)
            let c = Vec3.Dot(oc,oc) - radius * radius
            let disc = b*b-4.0*a*c

            match disc < 0.0 with
            | true -> None
            | false ->
                let t1 = (-b - Math.Sqrt(float disc)) / a * 0.5;
                let t2 = lazy ((-b + Math.Sqrt(disc)) / a * 0.5);
                let t = (t1,t2)

                match t with
                | (t1,_) when t1 > tmin && t1 < tmax ->
                    let position = ray.GetPoint(t1)
                    let normal = (position - center).Normalized()

                    Some({  
                            Normal = normal
                            Position = position
                            Color = 0.5 * (normal+Vec3.One)
                            RayT = t1 })
                | (_,t2) when t2.Force() > tmin && t2.Force() < tmax ->
                    let position = ray.GetPoint(t2.Force())
                    let normal = (position - center).Normalized()

                    Some({   
                            Normal = normal
                            Position = position
                            Color = 0.5 * (normal+Vec3.One)
                            RayT = t2.Force() })
                | _ -> None
