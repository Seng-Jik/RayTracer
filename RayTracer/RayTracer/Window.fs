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

let CreateImageForTestRay (size : Drawing.Size) (spp : int) (camera:Camera) (objs : (IHitable*IMaterial) list)  : Drawing.Image = 
    let image = new Drawing.Bitmap(size.Width,size.Height)

    let (xRecip,yRecip) = (1.0 / float size.Width,1.0 / float size.Height)

    let colWeigth = 1.0 / float spp

    let mutable renderedPixel = ref 0;


    async{
        let parallelSeq = Seq.init (size.Height*size.Width) (fun pxid ->
            async {
                let randomSeed = System.Random(pxid)
                let random = System.Random(randomSeed.Next())
                let y = pxid / size.Width
                let x = pxid % size.Width
                let xNorm = float x / float size.Width
                let yNorm = 1.0 - float y / float size.Height

                let mutable col = Vec3.Zero

                for _ in seq {0..spp - 1} do
                    let xTrace = xNorm + random.NextDouble() * xRecip
                    let yTrace = yNorm + random.NextDouble() * yRecip
                    let ray =
                        camera.CreateRay(xTrace,yTrace)
                    col <- col + colWeigth * GetRayColor ray objs 0 5

                System.Threading.Interlocked.Increment(renderedPixel) |> ignore
                if !renderedPixel % 100 = 0 then
                    printfn "RenderedPixels:%A  Percent:%A"
                                !renderedPixel
                                (float !renderedPixel / float (size.Width * size.Height))
                     
                return (col |> Gamma |> Vec3ToDrawingColor,x,y) })
        let asyncs = Async.Parallel parallelSeq
        let! pixels = asyncs
        printfn "Applying Pixels..."
        pixels |> Array.iter (fun (col,x,y) ->
            image.SetPixel(x,y,col))
    } |> Async.RunSynchronously
    upcast image

