module Vector

[<Struct>]
type Vec3 = {
    X : float
    Y : float
    Z : float }

with
    static member (*) (b:float,a:Vec3) = {
        X = a.X * b
        Y = a.Y * b
        Z = a.Z * b }

    static member (*) (a:Vec3,b:float) =
        (*) b a

    static member (~-) a = {
        X = -a.X
        Y = -a.Y
        Z = -a.Z }

    static member (+) (a,b) = {
        X = a.X + b.X
        Y = a.Y + b.Y
        Z = a.Z + b.Z }

    static member (-) (a,b) = {
        X = a.X - b.X
        Y = a.Y - b.Y
        Z = a.Z - b.Z }

    member x.Length() =
        x.X * x.X + x.Y * x.Y + x.Z * x.Z
        |> sqrt

    member x.Normalized() = {
        X = x.X / x.Length()
        Y = x.Y / x.Length()
        Z = x.Z / x.Length() }

let Zero = {
    X = 0.
    Y = 0.
    Z = 0. }

let One = {
    X = 1.
    Y = 1.
    Z = 1. }

let Dot (a,b) =
    a.X * b.X + a.Y * b.Y + a.Z * b.Z

let Cross (a,b) = {
    X = a.Y * b.Z - a.Z * b.Y
    Y = - (a.X * b.Z - a.Z * b.X)
    Z = a.X * b.Y - a.Y * b.X }