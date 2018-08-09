namespace Hitable

open Ray

type IHitable = 
    abstract member Hit : Ray -> HitRecord option
