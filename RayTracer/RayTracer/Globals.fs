module Globals

type Vec3 = Vector.Vec3
open Vector

let Vec3 (x,y,z) = {
    X = x
    Y = y
    Z = z }


let Clamp min max (x:float) = 
    match x with
    | x when System.Double.IsNaN(x) -> 0.0
    | x when x < min -> min
    | x when x > max -> max
    | _ -> x

let Vec3ToDrawingColor (col : Vec3) =
    let (r,g,b) = (col.X * 255.,col.Y*255.,col.Z*255.)
    let clamp x = Clamp 0.0 255. x
    let (cr,cg,cb) = (clamp r,clamp g,clamp b)
    
    System.Drawing.Color.FromArgb(255,int cr,int cg,int cb)
