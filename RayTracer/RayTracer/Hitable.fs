﻿namespace Hitable

open Ray

type IHitable = 
    abstract member Hit : Ray * float * float * IHitable list -> HitRecord option