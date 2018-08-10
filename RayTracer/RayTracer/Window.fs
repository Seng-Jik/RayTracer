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
    window.ShowDialog() |> ignore

let CreateImageForTestRay (size : Drawing.Size) (spp : int) (camera:Camera) (objs : (IHitable*IMaterial) list)  : Drawing.Image = 
    let lowLeftCorner = Vec3(-2.0,-1.0,-1.0)
    let horizontal = Vec3(4.0,0.0,0.0)
    let vertical = Vec3(0.0,2.0,0.0)
    let orginal = Vec3(0.0,0.0,0.0)
    let image = new Drawing.Bitmap(size.Width,size.Height)

    let (xRecip,yRecip) = (1.0 / float size.Width,1.0 / float size.Height)

    use mtx = new System.Threading.Mutex()

    let colWeigth = 1.0 / float spp

    


    async{
        let parallelSeq = Seq.init size.Height (fun y ->
            async {
                let random = System.Random(y)
                for x in seq{0..size.Width - 1} do
                        let xNorm = float x / float size.Width
                        let yNorm = 1.0 - float y / float size.Height

                        let mutable col = Vec3.Zero

                        for i in seq {0..spp - 1} do
                            let xTrace = xNorm + random.NextDouble() * xRecip
                            let yTrace = yNorm + random.NextDouble() * yRecip
                            let ray =
                                camera.CreateRay(xTrace,yTrace)
                            col <- col + colWeigth * GetScreenColor ray objs 0 5
            
                        mtx.WaitOne() |> ignore
                        image.SetPixel(x,y,col |> Gamma |> Vec3ToDrawingColor)
                        mtx.ReleaseMutex()
                })
        let asyncs = Async.Parallel parallelSeq
        let! v = asyncs
        ()
    } |> Async.RunSynchronously
    upcast image

