//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BeerApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductionProcess
    {
        public int ProcessID { get; set; }
        public int BeerID { get; set; }
        public int StepNumber { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> DurationHours { get; set; }
    
        public virtual Beer Beer { get; set; }
    }
}