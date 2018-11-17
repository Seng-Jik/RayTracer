module ParallelRenderer

open System.Windows.Forms
open System
open Window
open Globals

let RenderParallel width height spp hitableList camera =
    let size = System.Drawing.Size(width,height)
    use window = new Form()
    window.Size <- size
    window.Text <- "RayTracer"
    window.BackColor <- System.Drawing.Color.Black

    window.DoubleClick.Add(fun _ ->
        window.BackgroundImage.Save("Result.bmp")
        System.Diagnostics.Process.Start("Result.bmp") |> ignore)

    let colWeight = 1.0 / float spp

    let tracedSpp = ref 0

    let imgs = Array.init System.Environment.ProcessorCount (fun x ->
        {
            Width = width
            Height = height
            Pixels = Array.init (width*height) (fun x -> Vec3(0.0,0.0,0.0)) })

    let rndSeed = Random()
    window.Shown.Add (fun _ ->
        Array.init System.Environment.ProcessorCount (fun cpuIndex ->
            async {
                let rnd = Random(rndSeed.Next())
                do! Async.SwitchToThreadPool()
                for i in 0..spp / System.Environment.ProcessorCount do
                    hitableList
                    |> Window.Render (System.Drawing.Size(width,height)) camera rnd colWeight (imgs.[cpuIndex])
                    let nowTraced = System.Threading.Interlocked.Increment tracedSpp
                    printfn "CPU %d Traced Spp %d" cpuIndex nowTraced })
        |> Async.Parallel
        |> Async.Ignore
        |> Async.StartImmediate)

    window.Shown.Add (fun _ ->
        async {
            while true do
                do! Async.Sleep 10000
                printfn "Refreshing Window..."
                let bmp =
                        imgs
                        |> ReduceImage (float !tracedSpp / float spp)
                        |> ToBitmap
                if window.BackgroundImage <> null then
                    window.BackgroundImage.Dispose()
                window.BackgroundImage <- bmp
                do! Async.Sleep 100000 }
            |> Async.StartImmediate)
    Application.Run(window)
