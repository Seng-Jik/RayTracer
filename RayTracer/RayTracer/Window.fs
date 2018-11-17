module Window

open System.Windows.Forms
open System
open Globals
open Hitable
open Ray
open Material
open Tracer
open Camera


let Gamma (col:Vec3) = 
    Vec3(Math.Sqrt(col.X),Math.Sqrt(col.Y),Math.Sqrt(col.Z))

let DisplayImage (image:Drawing.Image) = 
    use window = new Form()
    window.Size <- image.Size
    window.Text <- "RayTracer"
    window.BackgroundImage <- image
    window.DoubleClick.Add(fun _ ->
        image.Save("Result.bmp")
        System.Diagnostics.Process.Start("Result.bmp") |> ignore)
    window.ShowDialog() |> ignore

let CreateImageForTestRay (size : Drawing.Size) (camera:Camera) (objs : (IHitable*IMaterial) list) = 
    let image = new Drawing.Bitmap(size.Width,size.Height)

    let (xRecip,yRecip) = (1.0 / float size.Width,1.0 / float size.Height)

    for i in 0..size.Height-1 do
        for j in 0..size.Width-1 do
            let pxid = i*size.Width+j
            let randomSeed = System.Random(pxid)
            let random = System.Random(randomSeed.Next())
            let y = pxid / size.Width
            let x = pxid % size.Width
            let xNorm = float x / float size.Width
            let yNorm = 1.0 - float y / float size.Height

            let xTrace = xNorm + random.NextDouble() * xRecip
            let yTrace = yNorm + random.NextDouble() * yRecip
            let ray =
                camera.CreateRay(xTrace,yTrace)
            let col = GetRayColor ray objs 0 5

            image.SetPixel(x,y,Vec3ToDrawingColor (Gamma col))

    image

