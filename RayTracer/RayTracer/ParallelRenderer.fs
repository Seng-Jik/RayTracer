module ParallelRenderer

open System.Windows.Forms
open System
open Window

let RenderParallel width height spp hitableList camera =
    let size = System.Drawing.Size(width,height)
    use window = new Form()
    window.Size <- size
    window.Text <- "RayTracer"
    window.BackColor <- System.Drawing.Color.Black

    let imgs = System.Collections.Concurrent.ConcurrentBag()
    let rndSeed = Random()
    window.Shown.Add (fun _ ->
        Array.init System.Environment.ProcessorCount (fun index ->
            async {
                let rnd = Random(rndSeed.Next())
                
                for i in 0..spp / System.Environment.ProcessorCount do
                    let ctx = System.Threading.SynchronizationContext.Current
                    do! Async.SwitchToThreadPool()
                    printfn "Tracing %d " index
                    hitableList
                    |> Window.Render (System.Drawing.Size(width,height)) camera rnd
                    |> imgs.Add

                    let bmp =
                        imgs
                        |> Seq.toArray
                        |> ReduceImage
                        |> ToBitmap
                    do! Async.SwitchToContext ctx
                    printfn "Traced %d BagCount:%d" index imgs.Count
                    window.BackgroundImage <- bmp })
        |> Async.Parallel
        |> Async.Ignore
        |> Async.StartImmediate)
    Application.Run(window)
