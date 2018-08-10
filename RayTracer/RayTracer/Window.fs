module Window

open System.Windows.Forms
open System
open Globals
open Hitable
open Ray
open Material
open Tracer


let Gamma (col:Vec3) = 
    Vec3(Math.Sqrt(col.X),Math.Sqrt(col.Y),Math.Sqrt(col.Z))


let rec GetScreenColor (ray:Ray) (objs:(IHitable*IMaterial) list) depth maxDepth =
    match Trace ray 0.0000001 Double.MaxValue objs with
    | Some(record) -> 
        let scattered = record.Material.Scatter(ray,record.HitRecord,Vec3(0.0,0.0,0.0))
        match depth with
        | x when x >= maxDepth -> Vec3(0.0,0.0,0.0)
        | _ -> 
            let (ray,atten) = scattered
            let col = GetScreenColor ray.Value objs (depth+1) maxDepth
            Vec3(col.X * atten.X,col.Y * atten.Y,col.Z * atten.Z)
    | None -> Background ray

let DisplayImage (image:Drawing.Image) = 
    use window = new Form()
    window.Size <- image.Size
    window.Text <- "RayTracer"
    window.BackgroundImage <- image
    window.ShowDialog() |> ignore

let CreateImageForTestRay (size : Drawing.Size) (spp : int) (objs : (IHitable*IMaterial) list) : Drawing.Image = 
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
                                Ray(
                                    orginal,
                                    lowLeftCorner + 
                                        horizontal * xTrace + vertical * yTrace)
                            col <- col + colWeigth * GetScreenColor ray objs 0 100
            
                        mtx.WaitOne() |> ignore
                        image.SetPixel(x,y,col |> Gamma |> Vec3ToDrawingColor)
                        mtx.ReleaseMutex()
                })
        let asyncs = Async.Parallel parallelSeq
        let! v = asyncs
        ()
    } |> Async.RunSynchronously
    upcast image

