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

    window.DoubleClick.Add(fun _ ->
        window.BackgroundImage.Save("Result.bmp")
        System.Diagnostics.Process.Start("Result.bmp") |> ignore)

    let imgs = System.Collections.Concurrent.ConcurrentBag()
    let rndSeed = Random()
    window.Shown.Add (fun _ ->
        Array.init System.Environment.ProcessorCount (fun index ->
            async {
                let rnd = Random(rndSeed.Next())
                do! Async.SwitchToThreadPool()
                for _ in 0..spp / System.Environment.ProcessorCount do
                    hitableList
                    |> Window.Render (System.Drawing.Size(width,height)) camera rnd
                    |> imgs.Add
                    printfn "Traced %d Traced Spp %d" index imgs.Count })
        |> Async.Parallel
        |> Async.Ignore
        |> Async.StartImmediate)

    window.Shown.Add (fun _ ->
        async {
            while true do
                do! Async.Sleep 10000
                printfn "Refreshing Window... %d Spp Traced." (imgs.Count)
                let bmp =
                        imgs
                        |> Seq.toArray
                        |> ReduceImage
                        |> ToBitmap
                window.BackgroundImage <- bmp }
            |> Async.StartImmediate)
    Application.Run(window)
